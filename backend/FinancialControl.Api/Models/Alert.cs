namespace FinancialControl.Api.Models;

public class Alert
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public Guid UserId { get; set; }
    public AlertType Type { get; set; }
    public decimal Threshold { get; set; }
    public Guid? CategoryId { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public Account Account { get; set; } = null!;
    public User User { get; set; } = null!;
    public Category? Category { get; set; }
}

public enum AlertType
{
    MonthlySpendingLimit = 1,      // Gastos mensais acima do limite
    LowBalance = 2,                 // Saldo baixo
    GoalDeadlineApproaching = 3,    // Meta próxima do prazo
    RecurringTransactionProcessed = 4, // Transação recorrente processada
    InvitationAccepted = 5,         // Convite aceito
    UnusualSpending = 6,            // Gasto incomum
    CategoryBudgetExceeded = 7      // Orçamento de categoria excedido
}
