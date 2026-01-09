using FinancialControl.Api.Data;
using FinancialControl.Api.DTOs;
using FinancialControl.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Api.Services;

public interface IBankingService
{
    Task<BankConnectionDto> CreateConnectionAsync(Guid userId, CreateBankConnectionRequest request);
    Task<List<BankConnectionDto>> GetUserConnectionsAsync(Guid userId);
    Task<BankConnectionDto?> GetConnectionByIdAsync(Guid id, Guid userId);
    Task<bool> UpdateConnectionAsync(Guid id, Guid userId, UpdateBankConnectionRequest request);
    Task<bool> DeleteConnectionAsync(Guid id, Guid userId);
    Task<BankConnectionSyncResult> SyncConnectionAsync(Guid connectionId, Guid userId);
    Task<List<BankTransactionListDto>> GetPendingTransactionsAsync(Guid userId);
    Task<bool> ImportTransactionAsync(Guid userId, ImportBankTransactionRequest request);
    Task<bool> IgnoreTransactionAsync(Guid userId, Guid bankTransactionId);
    Task<string> CreateConnectTokenAsync(Guid userId);
}

public class BankingService : IBankingService
{
    private readonly AppDbContext _context;
    private readonly IPluggyService _pluggyService;
    private readonly ILogger<BankingService> _logger;

    public BankingService(AppDbContext context, IPluggyService pluggyService, ILogger<BankingService> logger)
    {
        _context = context;
        _pluggyService = pluggyService;
        _logger = logger;
    }

    public async Task<BankConnectionDto> CreateConnectionAsync(Guid userId, CreateBankConnectionRequest request)
    {
        var connection = new BankConnection
        {
            Id = Guid.NewGuid(),
            AccountId = request.AccountId,
            UserId = userId,
            BankName = request.BankName,
            BankCode = request.BankCode,
            InstitutionId = request.InstitutionId,
            ItemId = request.ItemId,
            Status = BankConnectionStatus.Connected,
            ConnectedAt = DateTime.UtcNow,
            AutoSync = true
        };

        _context.BankConnections.Add(connection);
        await _context.SaveChangesAsync();

        return MapToDto(connection, 0);
    }

    public async Task<List<BankConnectionDto>> GetUserConnectionsAsync(Guid userId)
    {
        var connections = await _context.BankConnections
            .Where(bc => bc.UserId == userId)
            .Include(bc => bc.BankTransactions)
            .ToListAsync();

        return connections.Select(bc => MapToDto(bc, 
            bc.BankTransactions.Count(bt => bt.Status == BankTransactionStatus.Pending)
        )).ToList();
    }

    public async Task<BankConnectionDto?> GetConnectionByIdAsync(Guid id, Guid userId)
    {
        var connection = await _context.BankConnections
            .Include(bc => bc.BankTransactions)
            .FirstOrDefaultAsync(bc => bc.Id == id && bc.UserId == userId);

        if (connection == null) return null;

        return MapToDto(connection, 
            connection.BankTransactions.Count(bt => bt.Status == BankTransactionStatus.Pending)
        );
    }

    public async Task<bool> UpdateConnectionAsync(Guid id, Guid userId, UpdateBankConnectionRequest request)
    {
        var connection = await _context.BankConnections
            .FirstOrDefaultAsync(bc => bc.Id == id && bc.UserId == userId);

        if (connection == null) return false;

        if (request.AutoSync.HasValue)
            connection.AutoSync = request.AutoSync.Value;

        if (request.Status.HasValue)
            connection.Status = request.Status.Value;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteConnectionAsync(Guid id, Guid userId)
    {
        var connection = await _context.BankConnections
            .FirstOrDefaultAsync(bc => bc.Id == id && bc.UserId == userId);

        if (connection == null) return false;

        _context.BankConnections.Remove(connection);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<BankConnectionSyncResult> SyncConnectionAsync(Guid connectionId, Guid userId)
    {
        var connection = await _context.BankConnections
            .FirstOrDefaultAsync(bc => bc.Id == connectionId && bc.UserId == userId);

        if (connection == null)
        {
            return new BankConnectionSyncResult(false, 0, 0, "Conexão não encontrada");
        }

        try
        {
            connection.Status = BankConnectionStatus.Syncing;
            await _context.SaveChangesAsync();

            // Buscar transações reais do Pluggy
            var pluggyTransactions = await _pluggyService.FetchTransactionsAsync(connection.ItemId, connection.LastSyncAt);
            
            var newTransactions = 0;
            foreach (var transaction in pluggyTransactions)
            {
                var exists = await _context.BankTransactions
                    .AnyAsync(bt => bt.ExternalId == transaction.ExternalId && bt.BankConnectionId == connectionId);

                if (!exists)
                {
                    transaction.BankConnectionId = connectionId;
                    _context.BankTransactions.Add(transaction);
                    newTransactions++;
                }
            }

            connection.LastSyncAt = DateTime.UtcNow;
            connection.Status = BankConnectionStatus.Connected;
            connection.ErrorMessage = null;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Sincronização concluída para conexão {ConnectionId}: {Total} transações, {New} novas", 
                connectionId, pluggyTransactions.Count, newTransactions);

            return new BankConnectionSyncResult(true, pluggyTransactions.Count, newTransactions, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao sincronizar conexão {ConnectionId}", connectionId);
            connection.Status = BankConnectionStatus.Error;
            connection.ErrorMessage = ex.Message;
            await _context.SaveChangesAsync();

            return new BankConnectionSyncResult(false, 0, 0, ex.Message);
        }
    }

    public async Task<List<BankTransactionListDto>> GetPendingTransactionsAsync(Guid userId)
    {
        var transactions = await _context.BankTransactions
            .Include(bt => bt.BankConnection)
            .Where(bt => bt.BankConnection.UserId == userId && bt.Status == BankTransactionStatus.Pending)
            .OrderByDescending(bt => bt.Date)
            .Take(100)
            .ToListAsync();

        return transactions.Select(bt => new BankTransactionListDto(
            bt.Id,
            bt.BankConnection.BankName,
            bt.Description,
            bt.Amount,
            bt.Date,
            bt.Type,
            bt.Status
        )).ToList();
    }

    public async Task<bool> ImportTransactionAsync(Guid userId, ImportBankTransactionRequest request)
    {
        var bankTransaction = await _context.BankTransactions
            .Include(bt => bt.BankConnection)
            .FirstOrDefaultAsync(bt => bt.Id == request.BankTransactionId && bt.BankConnection.UserId == userId);

        if (bankTransaction == null || bankTransaction.Status != BankTransactionStatus.Pending)
            return false;

        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = bankTransaction.BankConnection.AccountId,
            UserId = userId,
            CategoryId = request.CategoryId,
            Amount = Math.Abs(bankTransaction.Amount),
            Type = bankTransaction.Type == BankTransactionType.Debit ? TransactionType.Expense : TransactionType.Income,
            Description = bankTransaction.Description,
            Date = bankTransaction.Date,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Transactions.Add(transaction);
        
        bankTransaction.Status = BankTransactionStatus.Imported;
        bankTransaction.TransactionId = transaction.Id;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IgnoreTransactionAsync(Guid userId, Guid bankTransactionId)
    {
        var bankTransaction = await _context.BankTransactions
            .Include(bt => bt.BankConnection)
            .FirstOrDefaultAsync(bt => bt.Id == bankTransactionId && bt.BankConnection.UserId == userId);

        if (bankTransaction == null || bankTransaction.Status != BankTransactionStatus.Pending)
            return false;

        bankTransaction.Status = BankTransactionStatus.Ignored;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<string> CreateConnectTokenAsync(Guid userId)
    {
        try
        {
            return await _pluggyService.CreateConnectTokenAsync(userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar Connect Token para usuário {UserId}", userId);
            throw;
        }
    }

    private BankConnectionDto MapToDto(BankConnection connection, int pendingCount)
    {
        return new BankConnectionDto(
            connection.Id,
            connection.AccountId,
            connection.BankName,
            connection.BankCode,
            connection.Status,
            connection.ConnectedAt,
            connection.LastSyncAt,
            connection.AutoSync,
            pendingCount
        );
    }
}
