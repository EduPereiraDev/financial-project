namespace FinancialControl.Api.Models;

public class BankConnection
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public Guid UserId { get; set; }
    public string BankName { get; set; } = string.Empty;
    public string BankCode { get; set; } = string.Empty;
    public string InstitutionId { get; set; } = string.Empty; // ID do provedor (Pluggy/Belvo)
    public string ItemId { get; set; } = string.Empty; // ID da conexão no provedor
    public BankConnectionStatus Status { get; set; }
    public DateTime ConnectedAt { get; set; }
    public DateTime? LastSyncAt { get; set; }
    public string? ErrorMessage { get; set; }
    public bool AutoSync { get; set; } = true;
    public string? Metadata { get; set; } // JSON para dados adicionais

    // Navigation properties
    public Account Account { get; set; } = null!;
    public User User { get; set; } = null!;
    public ICollection<BankTransaction> BankTransactions { get; set; } = new List<BankTransaction>();
}

public enum BankConnectionStatus
{
    Connected = 1,
    Disconnected = 2,
    Error = 3,
    Syncing = 4,
    PendingAuth = 5
}
