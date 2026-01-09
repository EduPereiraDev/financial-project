using FinancialControl.Api.Data;
using FinancialControl.Api.DTOs;
using FinancialControl.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Api.Services;

public interface IBudgetService
{
    Task<BudgetDto?> GetByIdAsync(Guid id, Guid userId);
    Task<BudgetSummaryDto> GetBudgetSummaryAsync(Guid userId, int month, int year);
    Task<List<BudgetDto>> GetAllAsync(Guid userId, int? month = null, int? year = null);
    Task<BudgetDto> CreateAsync(Guid userId, CreateBudgetDto dto);
    Task<BudgetDto?> UpdateAsync(Guid id, Guid userId, UpdateBudgetDto dto);
    Task<bool> DeleteAsync(Guid id, Guid userId);
}

public class BudgetService : IBudgetService
{
    private readonly AppDbContext _context;
    private readonly ILogger<BudgetService> _logger;

    public BudgetService(AppDbContext context, ILogger<BudgetService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<BudgetDto?> GetByIdAsync(Guid id, Guid userId)
    {
        var budget = await _context.Budgets
            .Include(b => b.Category)
            .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);

        if (budget == null)
            return null;

        var spent = await CalculateSpentAsync(userId, budget.CategoryId, budget.Month, budget.Year);

        return MapToDto(budget, spent);
    }

    public async Task<BudgetSummaryDto> GetBudgetSummaryAsync(Guid userId, int month, int year)
    {
        var budgets = await _context.Budgets
            .Include(b => b.Category)
            .Where(b => b.UserId == userId && b.Month == month && b.Year == year)
            .ToListAsync();

        var budgetDtos = new List<BudgetDto>();
        decimal totalBudget = 0;
        decimal totalSpent = 0;
        int overBudgetCount = 0;

        foreach (var budget in budgets)
        {
            var spent = await CalculateSpentAsync(userId, budget.CategoryId, month, year);
            var dto = MapToDto(budget, spent);
            budgetDtos.Add(dto);

            totalBudget += budget.Amount;
            totalSpent += spent;

            if (spent > budget.Amount)
                overBudgetCount++;
        }

        return new BudgetSummaryDto
        {
            TotalBudget = totalBudget,
            TotalSpent = totalSpent,
            TotalRemaining = totalBudget - totalSpent,
            OverallPercentage = totalBudget > 0 ? (totalSpent / totalBudget) * 100 : 0,
            CategoriesCount = budgets.Count,
            OverBudgetCount = overBudgetCount,
            Budgets = budgetDtos.OrderByDescending(b => b.PercentageUsed).ToList()
        };
    }

    public async Task<List<BudgetDto>> GetAllAsync(Guid userId, int? month = null, int? year = null)
    {
        var query = _context.Budgets
            .Include(b => b.Category)
            .Where(b => b.UserId == userId);

        if (month.HasValue)
            query = query.Where(b => b.Month == month.Value);

        if (year.HasValue)
            query = query.Where(b => b.Year == year.Value);

        var budgets = await query.ToListAsync();

        var result = new List<BudgetDto>();
        foreach (var budget in budgets)
        {
            var spent = await CalculateSpentAsync(userId, budget.CategoryId, budget.Month, budget.Year);
            result.Add(MapToDto(budget, spent));
        }

        return result;
    }

    public async Task<BudgetDto> CreateAsync(Guid userId, CreateBudgetDto dto)
    {
        // Verificar se já existe orçamento para esta categoria no período
        var exists = await _context.Budgets
            .AnyAsync(b => b.UserId == userId 
                && b.CategoryId == dto.CategoryId 
                && b.Month == dto.Month 
                && b.Year == dto.Year);

        if (exists)
            throw new InvalidOperationException("Já existe um orçamento para esta categoria neste período");

        var budget = new Budget
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CategoryId = dto.CategoryId,
            Amount = dto.Amount,
            Period = Enum.Parse<BudgetPeriod>(dto.Period),
            Month = dto.Month,
            Year = dto.Year,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Budgets.Add(budget);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Orçamento criado: {BudgetId} para usuário {UserId}", budget.Id, userId);

        // Recarregar com categoria
        await _context.Entry(budget).Reference(b => b.Category).LoadAsync();

        var spent = await CalculateSpentAsync(userId, budget.CategoryId, budget.Month, budget.Year);
        return MapToDto(budget, spent);
    }

    public async Task<BudgetDto?> UpdateAsync(Guid id, Guid userId, UpdateBudgetDto dto)
    {
        var budget = await _context.Budgets
            .Include(b => b.Category)
            .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);

        if (budget == null)
            return null;

        budget.Amount = dto.Amount;
        budget.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Orçamento atualizado: {BudgetId}", id);

        var spent = await CalculateSpentAsync(userId, budget.CategoryId, budget.Month, budget.Year);
        return MapToDto(budget, spent);
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        var budget = await _context.Budgets
            .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);

        if (budget == null)
            return false;

        _context.Budgets.Remove(budget);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Orçamento excluído: {BudgetId}", id);

        return true;
    }

    private async Task<decimal> CalculateSpentAsync(Guid userId, Guid categoryId, int month, int year)
    {
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1);

        var spent = await _context.Transactions
            .Where(t => t.UserId == userId
                && t.CategoryId == categoryId
                && t.Type == TransactionType.Expense
                && t.Date >= startDate
                && t.Date < endDate)
            .SumAsync(t => t.Amount);

        return spent;
    }

    private static BudgetDto MapToDto(Budget budget, decimal spent)
    {
        var remaining = budget.Amount - spent;
        var percentageUsed = budget.Amount > 0 ? (spent / budget.Amount) * 100 : 0;

        return new BudgetDto
        {
            Id = budget.Id,
            UserId = budget.UserId,
            CategoryId = budget.CategoryId,
            CategoryName = budget.Category?.Name ?? string.Empty,
            CategoryColor = budget.Category?.Color ?? "#000000",
            Amount = budget.Amount,
            Period = budget.Period.ToString(),
            Month = budget.Month,
            Year = budget.Year,
            Spent = spent,
            Remaining = remaining,
            PercentageUsed = Math.Round(percentageUsed, 1),
            CreatedAt = budget.CreatedAt,
            UpdatedAt = budget.UpdatedAt
        };
    }
}
