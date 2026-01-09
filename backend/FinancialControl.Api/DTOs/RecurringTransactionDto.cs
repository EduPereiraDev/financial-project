using FinancialControl.Api.Models;

namespace FinancialControl.Api.DTOs;

public class RecurringTransactionDto
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public Guid CategoryId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Frequency { get; set; } = string.Empty;
    public int DayOfMonth { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastExecutionDate { get; set; }
    public DateTime? NextExecutionDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public CategoryDto? Category { get; set; }
}

public class RecurringTransactionCategoryDto
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateRecurringTransactionRequest
{
    public Guid AccountId { get; set; }
    public Guid CategoryId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Type { get; set; } = string.Empty; // "Income" or "Expense"
    public string Frequency { get; set; } = string.Empty; // "Daily", "Weekly", "Biweekly", "Monthly", "Quarterly", "Yearly"
    public int DayOfMonth { get; set; } // 1-31 para mensal, 1-7 para semanal
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class UpdateRecurringTransactionRequest
{
    public Guid CategoryId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Frequency { get; set; } = string.Empty;
    public int DayOfMonth { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
}
