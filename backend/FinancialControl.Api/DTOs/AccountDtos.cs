using FinancialControl.Api.Models;

namespace FinancialControl.Api.DTOs;

public record CreateAccountRequest(
    string Name,
    AccountType Type
);

public record AccountDto(
    Guid Id,
    string Name,
    AccountType Type,
    Guid OwnerId,
    string OwnerName,
    DateTime CreatedAt,
    List<AccountMemberDto> Members
);

public record AccountMemberDto(
    Guid Id,
    Guid UserId,
    string UserName,
    string UserEmail,
    AccountRole Role,
    DateTime JoinedAt
);

public record InviteMemberRequest(
    string Email,
    AccountRole Role
);
