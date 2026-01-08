using FinancialControl.Api.Data;
using FinancialControl.Api.DTOs;
using FinancialControl.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Api.Services;

public class TransactionService : ITransactionService
{
    private readonly AppDbContext _context;
    private readonly ILogger<TransactionService> _logger;

    public TransactionService(AppDbContext context, ILogger<TransactionService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<PagedResult<TransactionDto>> GetTransactionsAsync(Guid userId, TransactionFilterRequest filter)
    {
        var query = _context.Transactions
            .Include(t => t.Account)
            .Include(t => t.User)
            .Include(t => t.Category)
            .Where(t => t.Account.OwnerId == userId || t.Account.Members.Any(m => m.UserId == userId))
            .AsQueryable();

        if (filter.AccountId.HasValue)
            query = query.Where(t => t.AccountId == filter.AccountId.Value);

        if (filter.AccountIds != null && filter.AccountIds.Any())
            query = query.Where(t => filter.AccountIds.Contains(t.AccountId));

        if (filter.CategoryId.HasValue)
            query = query.Where(t => t.CategoryId == filter.CategoryId.Value);

        if (filter.Type.HasValue)
            query = query.Where(t => t.Type == filter.Type.Value);

        if (filter.StartDate.HasValue)
            query = query.Where(t => t.Date >= filter.StartDate.Value);

        if (filter.EndDate.HasValue)
            query = query.Where(t => t.Date <= filter.EndDate.Value);

        if (filter.MinAmount.HasValue)
            query = query.Where(t => t.Amount >= filter.MinAmount.Value);

        if (filter.MaxAmount.HasValue)
            query = query.Where(t => t.Amount <= filter.MaxAmount.Value);

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            query = query.Where(t => t.Description.Contains(filter.SearchTerm));

        var totalCount = await query.CountAsync();

        var transactions = await query
            .OrderByDescending(t => t.Date)
            .ThenByDescending(t => t.CreatedAt)
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(t => new TransactionDto(
                t.Id,
                t.AccountId,
                t.Account.Name,
                t.UserId,
                t.User.Name,
                t.CategoryId,
                t.Category.Name,
                t.Category.Color,
                t.Category.Icon,
                t.Amount,
                t.Description,
                t.Date,
                t.Type,
                t.CreatedAt,
                t.UpdatedAt
            ))
            .ToListAsync();

        var totalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize);

        return new PagedResult<TransactionDto>(transactions, totalCount, filter.Page, filter.PageSize, totalPages);
    }

    public async Task<TransactionDto?> GetTransactionByIdAsync(Guid userId, Guid transactionId)
    {
        var transaction = await _context.Transactions
            .Include(t => t.Account)
            .Include(t => t.User)
            .Include(t => t.Category)
            .Where(t => t.Id == transactionId)
            .Where(t => t.Account.OwnerId == userId || t.Account.Members.Any(m => m.UserId == userId))
            .FirstOrDefaultAsync();

        if (transaction == null)
            return null;

        return new TransactionDto(
            transaction.Id,
            transaction.AccountId,
            transaction.Account.Name,
            transaction.UserId,
            transaction.User.Name,
            transaction.CategoryId,
            transaction.Category.Name,
            transaction.Category.Color,
            transaction.Category.Icon,
            transaction.Amount,
            transaction.Description,
            transaction.Date,
            transaction.Type,
            transaction.CreatedAt,
            transaction.UpdatedAt
        );
    }

    public async Task<TransactionDto> CreateTransactionAsync(Guid userId, CreateTransactionRequest request)
    {
        var account = await _context.Accounts
            .Include(a => a.Members)
            .FirstOrDefaultAsync(a => a.Id == request.AccountId);

        if (account == null)
            throw new InvalidOperationException("Account not found");

        var hasAccess = account.OwnerId == userId || 
                       account.Members.Any(m => m.UserId == userId && (m.Role == AccountRole.Owner || m.Role == AccountRole.Editor));

        if (!hasAccess)
            throw new UnauthorizedAccessException("You don't have permission to add transactions to this account");

        var category = await _context.Categories.FindAsync(request.CategoryId);
        if (category == null)
            throw new InvalidOperationException("Category not found");

        if (category.AccountId != request.AccountId)
            throw new InvalidOperationException("Category does not belong to this account");

        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = request.AccountId,
            UserId = userId,
            CategoryId = request.CategoryId,
            Amount = request.Amount,
            Description = request.Description,
            Date = request.Date,
            Type = request.Type,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        return await GetTransactionByIdAsync(userId, transaction.Id) 
               ?? throw new InvalidOperationException("Failed to retrieve created transaction");
    }

    public async Task<TransactionDto> UpdateTransactionAsync(Guid userId, Guid transactionId, UpdateTransactionRequest request)
    {
        var transaction = await _context.Transactions
            .Include(t => t.Account)
                .ThenInclude(a => a.Members)
            .FirstOrDefaultAsync(t => t.Id == transactionId);

        if (transaction == null)
            throw new InvalidOperationException("Transaction not found");

        var hasAccess = transaction.Account.OwnerId == userId || 
                       transaction.Account.Members.Any(m => m.UserId == userId && (m.Role == AccountRole.Owner || m.Role == AccountRole.Editor));

        if (!hasAccess)
            throw new UnauthorizedAccessException("You don't have permission to update this transaction");

        var category = await _context.Categories.FindAsync(request.CategoryId);
        if (category == null)
            throw new InvalidOperationException("Category not found");

        if (category.AccountId != transaction.AccountId)
            throw new InvalidOperationException("Category does not belong to this account");

        transaction.CategoryId = request.CategoryId;
        transaction.Amount = request.Amount;
        transaction.Description = request.Description;
        transaction.Date = request.Date;
        transaction.Type = request.Type;
        transaction.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return await GetTransactionByIdAsync(userId, transaction.Id) 
               ?? throw new InvalidOperationException("Failed to retrieve updated transaction");
    }

    public async Task DeleteTransactionAsync(Guid userId, Guid transactionId)
    {
        var transaction = await _context.Transactions
            .Include(t => t.Account)
                .ThenInclude(a => a.Members)
            .FirstOrDefaultAsync(t => t.Id == transactionId);

        if (transaction == null)
            throw new InvalidOperationException("Transaction not found");

        var hasAccess = transaction.Account.OwnerId == userId || 
                       transaction.Account.Members.Any(m => m.UserId == userId && (m.Role == AccountRole.Owner || m.Role == AccountRole.Editor));

        if (!hasAccess)
            throw new UnauthorizedAccessException("You don't have permission to delete this transaction");

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();
    }
}
