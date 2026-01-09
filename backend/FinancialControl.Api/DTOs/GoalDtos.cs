namespace FinancialControl.Api.DTOs;

public class GoalDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal TargetAmount { get; set; }
    public decimal CurrentAmount { get; set; }
    public DateTime TargetDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public decimal PercentageComplete { get; set; }
    public decimal RemainingAmount { get; set; }
    public int DaysRemaining { get; set; }
    public decimal RequiredMonthlyContribution { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<GoalContributionDto> RecentContributions { get; set; } = new();
}

public class CreateGoalDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal TargetAmount { get; set; }
    public DateTime TargetDate { get; set; }
    public string Priority { get; set; } = "Medium";
    public string? ImageUrl { get; set; }
}

public class UpdateGoalDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? TargetAmount { get; set; }
    public DateTime? TargetDate { get; set; }
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public string? ImageUrl { get; set; }
}

public class GoalContributionDto
{
    public Guid Id { get; set; }
    public Guid GoalId { get; set; }
    public decimal Amount { get; set; }
    public string? Note { get; set; }
    public DateTime ContributedAt { get; set; }
}

public class CreateContributionDto
{
    public decimal Amount { get; set; }
    public string? Note { get; set; }
}

public class GoalSummaryDto
{
    public int TotalGoals { get; set; }
    public int ActiveGoals { get; set; }
    public int CompletedGoals { get; set; }
    public decimal TotalTargetAmount { get; set; }
    public decimal TotalCurrentAmount { get; set; }
    public decimal OverallProgress { get; set; }
    public List<GoalDto> Goals { get; set; } = new();
}
