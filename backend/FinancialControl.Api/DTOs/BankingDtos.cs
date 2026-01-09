using FinancialControl.Api.Models;

namespace FinancialControl.Api.DTOs;

// BankConnection DTOs
public record BankConnectionDto(
    Guid Id,
    Guid AccountId,
    string BankName,
    string BankCode,
    BankConnectionStatus Status,
    DateTime ConnectedAt,
    DateTime? LastSyncAt,
    bool AutoSync,
    int PendingTransactionsCount
);

public record CreateBankConnectionRequest(
    Guid AccountId,
    string BankName,
    string BankCode,
    string InstitutionId,
    string ItemId
);

public record UpdateBankConnectionRequest(
    bool? AutoSync,
    BankConnectionStatus? Status
);

// BankTransaction DTOs
public record BankTransactionDto(
    Guid Id,
    Guid BankConnectionId,
    string ExternalId,
    string Description,
    decimal Amount,
    DateTime Date,
    BankTransactionType Type,
    string? Category,
    BankTransactionStatus Status,
    Guid? TransactionId
);

public record BankTransactionListDto(
    Guid Id,
    string BankName,
    string Description,
    decimal Amount,
    DateTime Date,
    BankTransactionType Type,
    BankTransactionStatus Status
);

public record ImportBankTransactionRequest(
    Guid BankTransactionId,
    Guid CategoryId,
    string? Notes
);

public record SyncBankConnectionRequest(
    Guid BankConnectionId
);

// Response DTOs
public record BankConnectionSyncResult(
    bool Success,
    int TransactionsFound,
    int NewTransactions,
    string? ErrorMessage
);
