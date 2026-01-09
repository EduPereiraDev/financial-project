namespace FinancialControl.Api.DTOs;

public class DashboardStatsDto
{
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal Balance { get; set; }
    public decimal PreviousMonthIncome { get; set; }
    public decimal PreviousMonthExpenses { get; set; }
    public List<MonthlyDataDto> MonthlyData { get; set; } = new();
    public List<CategoryExpenseDto> CategoryExpenses { get; set; } = new();
    public List<DailyBalanceDto> DailyBalance { get; set; } = new();
}

public class MonthlyDataDto
{
    public string Month { get; set; } = string.Empty;
    public decimal Income { get; set; }
    public decimal Expenses { get; set; }
}

public class CategoryExpenseDto
{
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Color { get; set; } = string.Empty;
}

public class DailyBalanceDto
{
    public string Date { get; set; } = string.Empty;
    public decimal Balance { get; set; }
}
