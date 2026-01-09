namespace FinancialControl.Api.Models;

public class RecurringTransaction
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public Guid CategoryId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public RecurrenceFrequency Frequency { get; set; }
    public int DayOfMonth { get; set; } // 1-31 para mensal, 1-7 para semanal
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? LastExecutionDate { get; set; }
    public DateTime? NextExecutionDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public Account Account { get; set; } = null!;
    public Category Category { get; set; } = null!;
}

public enum RecurrenceFrequency
{
    Daily = 1,
    Weekly = 2,
    Biweekly = 3,
    Monthly = 4,
    Quarterly = 5,
    Yearly = 6
}
