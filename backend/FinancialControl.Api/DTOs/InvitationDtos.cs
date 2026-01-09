using FinancialControl.Api.Models;

namespace FinancialControl.Api.DTOs;

public record InvitationDto(
    Guid Id,
    Guid AccountId,
    string AccountName,
    Guid InvitedByUserId,
    string InvitedByUserName,
    string InvitedEmail,
    AccountRole Role,
    InvitationStatus Status,
    DateTime ExpiresAt,
    DateTime CreatedAt,
    DateTime? AcceptedAt
);

public record CreateInvitationRequest(
    Guid AccountId,
    string InvitedEmail,
    AccountRole Role
);

public record AcceptInvitationRequest(
    string Token
);

public record InvitationListDto(
    Guid Id,
    string InvitedEmail,
    AccountRole Role,
    InvitationStatus Status,
    DateTime ExpiresAt,
    DateTime CreatedAt,
    string InvitedByUserName
);
