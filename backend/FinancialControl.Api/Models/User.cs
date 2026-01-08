namespace FinancialControl.Api.Models;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public ICollection<Account> OwnedAccounts { get; set; } = new List<Account>();
    public ICollection<AccountMember> AccountMemberships { get; set; } = new List<AccountMember>();
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
