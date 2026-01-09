using FinancialControl.Api.Models;

namespace FinancialControl.Api.DTOs;

public record NotificationDto(
    Guid Id,
    Guid UserId,
    Guid? AlertId,
    NotificationType Type,
    string Title,
    string Message,
    bool IsRead,
    DateTime CreatedAt,
    DateTime? ReadAt,
    string? ActionUrl,
    string? Metadata
);

public record CreateNotificationRequest(
    Guid UserId,
    Guid? AlertId,
    NotificationType Type,
    string Title,
    string Message,
    string? ActionUrl,
    string? Metadata
);

public record NotificationListDto(
    Guid Id,
    NotificationType Type,
    string Title,
    string Message,
    bool IsRead,
    DateTime CreatedAt,
    string? ActionUrl
);
