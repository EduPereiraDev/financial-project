namespace FinancialControl.Api.Models;

public class BankTransaction
{
    public Guid Id { get; set; }
    public Guid BankConnectionId { get; set; }
    public string ExternalId { get; set; } = string.Empty; // ID da transação no banco
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public BankTransactionType Type { get; set; }
    public string? Category { get; set; }
    public BankTransactionStatus Status { get; set; }
    public Guid? TransactionId { get; set; } // Link para Transaction se foi importada
    public DateTime CreatedAt { get; set; }
    public string? Metadata { get; set; } // JSON para dados adicionais do banco

    // Navigation properties
    public BankConnection BankConnection { get; set; } = null!;
    public Transaction? Transaction { get; set; }
}

public enum BankTransactionType
{
    Debit = 1,
    Credit = 2
}

public enum BankTransactionStatus
{
    Pending = 1,      // Aguardando importação
    Imported = 2,     // Já importada como Transaction
    Ignored = 3,      // Usuário optou por ignorar
    Duplicate = 4     // Detectada como duplicata
}
