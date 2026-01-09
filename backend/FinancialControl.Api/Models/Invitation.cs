namespace FinancialControl.Api.Models;

public class Invitation
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public Guid InvitedByUserId { get; set; }
    public string InvitedEmail { get; set; } = string.Empty;
    public AccountRole Role { get; set; }
    public InvitationStatus Status { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? AcceptedAt { get; set; }

    // Navigation properties
    public Account Account { get; set; } = null!;
    public User InvitedByUser { get; set; } = null!;
}

public enum InvitationStatus
{
    Pending = 1,
    Accepted = 2,
    Rejected = 3,
    Expired = 4,
    Cancelled = 5
}
