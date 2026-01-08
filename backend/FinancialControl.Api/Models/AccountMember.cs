namespace FinancialControl.Api.Models;

public class AccountMember
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public Guid UserId { get; set; }
    public AccountRole Role { get; set; }
    public DateTime JoinedAt { get; set; }

    // Navigation properties
    public Account Account { get; set; } = null!;
    public User User { get; set; } = null!;
}

public enum AccountRole
{
    Owner,
    Editor,
    Viewer
}
