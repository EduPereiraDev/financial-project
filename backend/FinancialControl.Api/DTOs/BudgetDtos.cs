namespace FinancialControl.Api.DTOs;

public class BudgetDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryColor { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Period { get; set; } = string.Empty;
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal Spent { get; set; }
    public decimal Remaining { get; set; }
    public decimal PercentageUsed { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateBudgetDto
{
    public Guid CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string Period { get; set; } = "Monthly";
    public int Month { get; set; }
    public int Year { get; set; }
}

public class UpdateBudgetDto
{
    public decimal Amount { get; set; }
}

public class BudgetSummaryDto
{
    public decimal TotalBudget { get; set; }
    public decimal TotalSpent { get; set; }
    public decimal TotalRemaining { get; set; }
    public decimal OverallPercentage { get; set; }
    public int CategoriesCount { get; set; }
    public int OverBudgetCount { get; set; }
    public List<BudgetDto> Budgets { get; set; } = new();
}
