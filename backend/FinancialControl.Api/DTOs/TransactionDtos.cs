using FinancialControl.Api.Models;

namespace FinancialControl.Api.DTOs;

public record CreateTransactionRequest(
    Guid AccountId,
    Guid CategoryId,
    decimal Amount,
    string Description,
    DateTime Date,
    TransactionType Type
);

public record UpdateTransactionRequest(
    Guid CategoryId,
    decimal Amount,
    string Description,
    DateTime Date,
    TransactionType Type
);

public record TransactionDto(
    Guid Id,
    Guid AccountId,
    string AccountName,
    Guid UserId,
    string UserName,
    Guid CategoryId,
    string CategoryName,
    string CategoryColor,
    string CategoryIcon,
    decimal Amount,
    string Description,
    DateTime Date,
    TransactionType Type,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record TransactionFilterRequest(
    Guid? AccountId,
    List<Guid>? AccountIds,
    Guid? CategoryId,
    TransactionType? Type,
    DateTime? StartDate,
    DateTime? EndDate,
    decimal? MinAmount,
    decimal? MaxAmount,
    string? SearchTerm,
    int Page = 1,
    int PageSize = 25
);

public record PagedResult<T>(
    List<T> Items,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages
);
