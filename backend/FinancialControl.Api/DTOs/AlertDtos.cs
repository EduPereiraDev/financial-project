using FinancialControl.Api.Models;

namespace FinancialControl.Api.DTOs;

public record AlertDto(
    Guid Id,
    Guid AccountId,
    Guid UserId,
    AlertType Type,
    string Name,
    string Description,
    bool IsActive,
    decimal? ThresholdAmount,
    int? ThresholdDays,
    Guid? CategoryId,
    string? CategoryName,
    DateTime CreatedAt,
    DateTime? LastTriggeredAt
);

public record CreateAlertRequest(
    Guid AccountId,
    AlertType Type,
    string Name,
    string Description,
    decimal? ThresholdAmount,
    int? ThresholdDays,
    Guid? CategoryId
);

public record UpdateAlertRequest(
    string? Name,
    string? Description,
    bool? IsActive,
    decimal? ThresholdAmount,
    int? ThresholdDays,
    Guid? CategoryId
);
