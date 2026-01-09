namespace FinancialControl.Api.Models;

public class Goal
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal TargetAmount { get; set; }
    public decimal CurrentAmount { get; set; }
    public DateTime TargetDate { get; set; }
    public GoalStatus Status { get; set; }
    public GoalPriority Priority { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public ICollection<GoalContribution> Contributions { get; set; } = new List<GoalContribution>();
}

public class GoalContribution
{
    public Guid Id { get; set; }
    public Guid GoalId { get; set; }
    public decimal Amount { get; set; }
    public string? Note { get; set; }
    public DateTime ContributedAt { get; set; }

    // Navigation properties
    public Goal Goal { get; set; } = null!;
}

public enum GoalStatus
{
    Active,
    Completed,
    Cancelled,
    Paused
}

public enum GoalPriority
{
    Low,
    Medium,
    High,
    Critical
}
