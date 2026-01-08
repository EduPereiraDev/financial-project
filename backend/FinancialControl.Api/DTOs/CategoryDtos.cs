using FinancialControl.Api.Models;

namespace FinancialControl.Api.DTOs;

public record CreateCategoryRequest(
    Guid AccountId,
    string Name,
    string Color,
    string Icon,
    TransactionType Type
);

public record UpdateCategoryRequest(
    string Name,
    string Color,
    string Icon
);

public record CategoryDto(
    Guid Id,
    Guid AccountId,
    string Name,
    string Color,
    string Icon,
    TransactionType Type,
    DateTime CreatedAt
);
