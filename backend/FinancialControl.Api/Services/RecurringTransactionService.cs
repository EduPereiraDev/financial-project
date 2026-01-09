using FinancialControl.Api.Data;
using FinancialControl.Api.DTOs;
using FinancialControl.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Api.Services;

public interface IRecurringTransactionService
{
    Task<IEnumerable<RecurringTransactionDto>> GetAllByAccountAsync(Guid accountId);
    Task<RecurringTransactionDto?> GetByIdAsync(Guid id, Guid accountId);
    Task<RecurringTransactionDto> CreateAsync(CreateRecurringTransactionRequest request);
    Task<RecurringTransactionDto> UpdateAsync(Guid id, Guid accountId, UpdateRecurringTransactionRequest request);
    Task DeleteAsync(Guid id, Guid accountId);
    Task<int> ProcessDueRecurringTransactionsAsync();
}

public class RecurringTransactionService : IRecurringTransactionService
{
    private readonly AppDbContext _context;

    public RecurringTransactionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RecurringTransactionDto>> GetAllByAccountAsync(Guid accountId)
    {
        var recurringTransactions = await _context.RecurringTransactions
            .Include(rt => rt.Category)
            .Where(rt => rt.AccountId == accountId)
            .OrderByDescending(rt => rt.CreatedAt)
            .ToListAsync();

        return recurringTransactions.Select(MapToDto);
    }

    public async Task<RecurringTransactionDto?> GetByIdAsync(Guid id, Guid accountId)
    {
        var recurringTransaction = await _context.RecurringTransactions
            .Include(rt => rt.Category)
            .FirstOrDefaultAsync(rt => rt.Id == id && rt.AccountId == accountId);

        return recurringTransaction == null ? null : MapToDto(recurringTransaction);
    }

    public async Task<RecurringTransactionDto> CreateAsync(CreateRecurringTransactionRequest request)
    {
        var recurringTransaction = new RecurringTransaction
        {
            Id = Guid.NewGuid(),
            AccountId = request.AccountId,
            CategoryId = request.CategoryId,
            Description = request.Description,
            Amount = request.Amount,
            Type = Enum.Parse<TransactionType>(request.Type),
            Frequency = Enum.Parse<RecurrenceFrequency>(request.Frequency),
            DayOfMonth = request.DayOfMonth,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            IsActive = true,
            NextExecutionDate = CalculateNextExecutionDate(request.StartDate, request.Frequency, request.DayOfMonth),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.RecurringTransactions.Add(recurringTransaction);
        await _context.SaveChangesAsync();

        return MapToDto(await _context.RecurringTransactions
            .Include(rt => rt.Category)
            .FirstAsync(rt => rt.Id == recurringTransaction.Id));
    }

    public async Task<RecurringTransactionDto> UpdateAsync(Guid id, Guid accountId, UpdateRecurringTransactionRequest request)
    {
        var recurringTransaction = await _context.RecurringTransactions
            .FirstOrDefaultAsync(rt => rt.Id == id && rt.AccountId == accountId);

        if (recurringTransaction == null)
            throw new KeyNotFoundException("Recurring transaction not found");

        recurringTransaction.CategoryId = request.CategoryId;
        recurringTransaction.Description = request.Description;
        recurringTransaction.Amount = request.Amount;
        recurringTransaction.Frequency = Enum.Parse<RecurrenceFrequency>(request.Frequency);
        recurringTransaction.DayOfMonth = request.DayOfMonth;
        recurringTransaction.EndDate = request.EndDate;
        recurringTransaction.IsActive = request.IsActive;
        recurringTransaction.UpdatedAt = DateTime.UtcNow;

        // Recalcular próxima execução se mudou frequência ou dia
        if (recurringTransaction.NextExecutionDate == null || !recurringTransaction.IsActive)
        {
            recurringTransaction.NextExecutionDate = CalculateNextExecutionDate(
                DateTime.UtcNow, 
                request.Frequency, 
                request.DayOfMonth
            );
        }

        await _context.SaveChangesAsync();

        return MapToDto(await _context.RecurringTransactions
            .Include(rt => rt.Category)
            .FirstAsync(rt => rt.Id == id));
    }

    public async Task DeleteAsync(Guid id, Guid accountId)
    {
        var recurringTransaction = await _context.RecurringTransactions
            .FirstOrDefaultAsync(rt => rt.Id == id && rt.AccountId == accountId);

        if (recurringTransaction == null)
            throw new KeyNotFoundException("Recurring transaction not found");

        _context.RecurringTransactions.Remove(recurringTransaction);
        await _context.SaveChangesAsync();
    }

    public async Task<int> ProcessDueRecurringTransactionsAsync()
    {
        var today = DateTime.UtcNow.Date;
        
        var dueRecurringTransactions = await _context.RecurringTransactions
            .Include(rt => rt.Account)
            .Where(rt => rt.IsActive && 
                         rt.NextExecutionDate != null && 
                         rt.NextExecutionDate.Value.Date <= today &&
                         (rt.EndDate == null || rt.EndDate.Value.Date >= today))
            .ToListAsync();

        int processedCount = 0;

        foreach (var recurring in dueRecurringTransactions)
        {
            // Criar transação
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                AccountId = recurring.AccountId,
                UserId = recurring.Account.OwnerId,
                CategoryId = recurring.CategoryId,
                Description = $"{recurring.Description} (Recorrente)",
                Amount = recurring.Amount,
                Type = recurring.Type,
                Date = recurring.NextExecutionDate!.Value,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Transactions.Add(transaction);

            // Atualizar última execução e calcular próxima
            recurring.LastExecutionDate = recurring.NextExecutionDate;
            recurring.NextExecutionDate = CalculateNextExecutionDate(
                recurring.NextExecutionDate.Value,
                recurring.Frequency.ToString(),
                recurring.DayOfMonth
            );
            recurring.UpdatedAt = DateTime.UtcNow;

            processedCount++;
        }

        if (processedCount > 0)
        {
            await _context.SaveChangesAsync();
        }

        return processedCount;
    }

    private static DateTime CalculateNextExecutionDate(DateTime fromDate, string frequency, int dayOfMonth)
    {
        var freq = Enum.Parse<RecurrenceFrequency>(frequency);
        return CalculateNextExecutionDate(fromDate, freq, dayOfMonth);
    }

    private static DateTime CalculateNextExecutionDate(DateTime fromDate, RecurrenceFrequency frequency, int dayOfMonth)
    {
        return frequency switch
        {
            RecurrenceFrequency.Daily => fromDate.AddDays(1),
            RecurrenceFrequency.Weekly => fromDate.AddDays(7),
            RecurrenceFrequency.Biweekly => fromDate.AddDays(14),
            RecurrenceFrequency.Monthly => GetNextMonthlyDate(fromDate, dayOfMonth),
            RecurrenceFrequency.Quarterly => GetNextMonthlyDate(fromDate, dayOfMonth).AddMonths(2),
            RecurrenceFrequency.Yearly => fromDate.AddYears(1),
            _ => fromDate.AddMonths(1)
        };
    }

    private static DateTime GetNextMonthlyDate(DateTime fromDate, int dayOfMonth)
    {
        var nextMonth = fromDate.AddMonths(1);
        var daysInMonth = DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month);
        var targetDay = Math.Min(dayOfMonth, daysInMonth);
        return new DateTime(nextMonth.Year, nextMonth.Month, targetDay);
    }

    private static RecurringTransactionDto MapToDto(RecurringTransaction rt)
    {
        return new RecurringTransactionDto
        {
            Id = rt.Id,
            AccountId = rt.AccountId,
            CategoryId = rt.CategoryId,
            Description = rt.Description,
            Amount = rt.Amount,
            Type = rt.Type.ToString(),
            Frequency = rt.Frequency.ToString(),
            DayOfMonth = rt.DayOfMonth,
            StartDate = rt.StartDate,
            EndDate = rt.EndDate,
            IsActive = rt.IsActive,
            LastExecutionDate = rt.LastExecutionDate,
            NextExecutionDate = rt.NextExecutionDate,
            CreatedAt = rt.CreatedAt,
            UpdatedAt = rt.UpdatedAt,
            Category = rt.Category == null ? null : new CategoryDto(
                rt.Category.Id,
                rt.Category.AccountId,
                rt.Category.Name,
                rt.Category.Color ?? string.Empty,
                rt.Category.Icon ?? string.Empty,
                rt.Category.Type,
                rt.Category.CreatedAt
            )
        };
    }
}
