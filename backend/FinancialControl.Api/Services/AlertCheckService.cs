using FinancialControl.Api.Data;
using FinancialControl.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Api.Services;

public interface IAlertCheckService
{
    Task CheckAndTriggerAlertsAsync();
}

public class AlertCheckService : IAlertCheckService
{
    private readonly AppDbContext _context;
    private readonly INotificationService _notificationService;

    public AlertCheckService(AppDbContext context, INotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task CheckAndTriggerAlertsAsync()
    {
        var activeAlerts = await _context.Alerts
            .Where(a => a.IsActive)
            .Include(a => a.Account)
            .ToListAsync();

        foreach (var alert in activeAlerts)
        {
            bool shouldTrigger = false;
            string notificationMessage = "";

            switch (alert.Type)
            {
                case AlertType.MonthlySpendingLimit:
                    shouldTrigger = await CheckMonthlySpendingLimit(alert);
                    notificationMessage = $"Seus gastos mensais ultrapassaram o limite de {alert.Threshold:C}";
                    break;

                case AlertType.LowBalance:
                    shouldTrigger = await CheckLowBalance(alert);
                    notificationMessage = $"Saldo baixo na conta {alert.Account.Name}: abaixo de {alert.Threshold:C}";
                    break;

                case AlertType.CategoryBudgetExceeded:
                    shouldTrigger = await CheckCategoryBudget(alert);
                    notificationMessage = $"Orçamento da categoria excedido: {alert.Threshold:C}";
                    break;
            }

            if (shouldTrigger)
            {
                await CreateNotificationForAlert(alert, notificationMessage);
                
                alert.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }

    private async Task<bool> CheckMonthlySpendingLimit(Alert alert)
    {
        var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        var totalSpent = await _context.Transactions
            .Where(t => t.AccountId == alert.AccountId 
                     && t.Type == TransactionType.Expense
                     && t.Date >= startOfMonth
                     && t.Date < DateTime.UtcNow)
            .SumAsync(t => t.Amount);

        return totalSpent > alert.Threshold;
    }

    private async Task<bool> CheckLowBalance(Alert alert)
    {
        var totalIncome = await _context.Transactions
            .Where(t => t.AccountId == alert.AccountId && t.Type == TransactionType.Income)
            .SumAsync(t => t.Amount);

        var totalExpense = await _context.Transactions
            .Where(t => t.AccountId == alert.AccountId && t.Type == TransactionType.Expense)
            .SumAsync(t => t.Amount);

        var balance = totalIncome - totalExpense;

        return balance < alert.Threshold;
    }

    private async Task<bool> CheckCategoryBudget(Alert alert)
    {
        if (!alert.CategoryId.HasValue) return false;

        var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        var categorySpent = await _context.Transactions
            .Where(t => t.AccountId == alert.AccountId
                     && t.CategoryId == alert.CategoryId
                     && t.Type == TransactionType.Expense
                     && t.Date >= startOfMonth
                     && t.Date < DateTime.UtcNow)
            .SumAsync(t => t.Amount);

        return categorySpent > alert.Threshold;
    }

    private async Task CreateNotificationForAlert(Alert alert, string message)
    {
        // Verificar se já foi criada notificação recente (últimas 24h)
        var recentNotification = await _context.Notifications
            .Where(n => n.UserId == alert.UserId
                     && n.AlertId == alert.Id
                     && n.CreatedAt >= DateTime.UtcNow.AddHours(-24))
            .AnyAsync();

        if (recentNotification) return;

        await _notificationService.CreateNotificationAsync(new DTOs.CreateNotificationRequest(
            alert.UserId,
            alert.Id,
            NotificationType.Warning,
            $"Alert {alert.Type}",
            message,
            $"/alerts/{alert.Id}",
            null
        ));
    }
}
