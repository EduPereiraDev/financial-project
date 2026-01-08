namespace FinancialControl.Api.Models;

public class Transaction
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public Account Account { get; set; } = null!;
    public User User { get; set; } = null!;
    public Category Category { get; set; } = null!;
}

public enum TransactionType
{
    Income,
    Expense
}
