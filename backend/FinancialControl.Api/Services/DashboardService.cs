using FinancialControl.Api.Data;
using FinancialControl.Api.DTOs;
using FinancialControl.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Api.Services;

public interface IDashboardService
{
    Task<DashboardStatsDto> GetDashboardStatsAsync(Guid userId, int months = 6);
}

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _context;
    private readonly ILogger<DashboardService> _logger;

    public DashboardService(AppDbContext context, ILogger<DashboardService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<DashboardStatsDto> GetDashboardStatsAsync(Guid userId, int months = 6)
    {
        try
        {
            var now = DateTime.UtcNow;
            var startDate = now.AddMonths(-months);
            var currentMonthStart = new DateTime(now.Year, now.Month, 1);
            var previousMonthStart = currentMonthStart.AddMonths(-1);

            // Buscar todas as transações do período
            var transactions = await _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.UserId == userId && t.Date >= startDate)
                .OrderBy(t => t.Date)
                .ToListAsync();

            // Transações do mês atual
            var currentMonthTransactions = transactions
                .Where(t => t.Date >= currentMonthStart)
                .ToList();

            // Transações do mês anterior
            var previousMonthTransactions = transactions
                .Where(t => t.Date >= previousMonthStart && t.Date < currentMonthStart)
                .ToList();

            // Calcular totais do mês atual
            var totalIncome = currentMonthTransactions
                .Where(t => t.Type == TransactionType.Income)
                .Sum(t => t.Amount);

            var totalExpenses = currentMonthTransactions
                .Where(t => t.Type == TransactionType.Expense)
                .Sum(t => t.Amount);

            // Calcular totais do mês anterior
            var previousMonthIncome = previousMonthTransactions
                .Where(t => t.Type == TransactionType.Income)
                .Sum(t => t.Amount);

            var previousMonthExpenses = previousMonthTransactions
                .Where(t => t.Type == TransactionType.Expense)
                .Sum(t => t.Amount);

            // Dados mensais (últimos 6 meses)
            var monthlyData = new List<MonthlyDataDto>();
            for (int i = months - 1; i >= 0; i--)
            {
                var monthStart = now.AddMonths(-i);
                var monthEnd = monthStart.AddMonths(1);
                var monthName = monthStart.ToString("MMM/yy");

                var monthTransactions = transactions
                    .Where(t => t.Date >= monthStart && t.Date < monthEnd)
                    .ToList();

                monthlyData.Add(new MonthlyDataDto
                {
                    Month = monthName,
                    Income = monthTransactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount),
                    Expenses = monthTransactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount)
                });
            }

            // Gastos por categoria (mês atual)
            var categoryColors = new Dictionary<string, string>
            {
                { "Alimentação", "#FF6384" },
                { "Transporte", "#36A2EB" },
                { "Moradia", "#FFCE56" },
                { "Saúde", "#4BC0C0" },
                { "Educação", "#9966FF" },
                { "Lazer", "#FF9F40" },
                { "Outros", "#C9CBCF" }
            };

            var categoryExpenses = currentMonthTransactions
                .Where(t => t.Type == TransactionType.Expense)
                .GroupBy(t => t.Category?.Name ?? "Sem Categoria")
                .Select(g => new CategoryExpenseDto
                {
                    Category = g.Key,
                    Amount = g.Sum(t => t.Amount),
                    Color = categoryColors.ContainsKey(g.Key) ? categoryColors[g.Key] : "#95A5A6"
                })
                .OrderByDescending(c => c.Amount)
                .ToList();

            // Evolução do saldo (últimos 30 dias)
            var dailyBalance = new List<DailyBalanceDto>();
            var last30Days = now.AddDays(-30);
            var runningBalance = 0m;

            // Calcular saldo inicial (antes dos últimos 30 dias)
            var initialBalance = transactions
                .Where(t => t.Date < last30Days)
                .Sum(t => t.Type == TransactionType.Income ? t.Amount : -t.Amount);

            runningBalance = initialBalance;

            for (int i = 30; i >= 0; i--)
            {
                var date = now.AddDays(-i).Date;
                var dayTransactions = transactions
                    .Where(t => t.Date.Date == date)
                    .ToList();

                foreach (var transaction in dayTransactions)
                {
                    runningBalance += transaction.Type == TransactionType.Income ? transaction.Amount : -transaction.Amount;
                }

                dailyBalance.Add(new DailyBalanceDto
                {
                    Date = date.ToString("dd/MM"),
                    Balance = runningBalance
                });
            }

            return new DashboardStatsDto
            {
                TotalIncome = totalIncome,
                TotalExpenses = totalExpenses,
                Balance = totalIncome - totalExpenses,
                PreviousMonthIncome = previousMonthIncome,
                PreviousMonthExpenses = previousMonthExpenses,
                MonthlyData = monthlyData,
                CategoryExpenses = categoryExpenses,
                DailyBalance = dailyBalance
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar estatísticas do dashboard para usuário {UserId}", userId);
            throw;
        }
    }
}
