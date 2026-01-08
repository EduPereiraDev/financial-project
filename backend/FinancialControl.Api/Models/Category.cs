namespace FinancialControl.Api.Models;

public class Category
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = "#6B7280";
    public string Icon { get; set; } = "tag";
    public TransactionType Type { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public Account Account { get; set; } = null!;
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
