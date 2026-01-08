using FinancialControl.Api.DTOs;

namespace FinancialControl.Api.Services;

public interface IAccountService
{
    Task<List<AccountDto>> GetUserAccountsAsync(Guid userId);
    Task<AccountDto?> GetAccountByIdAsync(Guid userId, Guid accountId);
    Task<AccountDto> CreateAccountAsync(Guid userId, CreateAccountRequest request);
    Task<AccountDto> InviteMemberAsync(Guid userId, Guid accountId, InviteMemberRequest request);
    Task RemoveMemberAsync(Guid userId, Guid accountId, Guid memberId);
}
