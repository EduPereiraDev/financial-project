namespace FinancialControl.Api.Models;

public class Account
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public AccountType Type { get; set; }
    public Guid OwnerId { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public User Owner { get; set; } = null!;
    public ICollection<AccountMember> Members { get; set; } = new List<AccountMember>();
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
}

public enum AccountType
{
    Personal,
    Shared
}
