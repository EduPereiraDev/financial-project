using FinancialControl.Api.Data;
using FinancialControl.Api.DTOs;
using FinancialControl.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Api.Services;

public class AccountService : IAccountService
{
    private readonly AppDbContext _context;
    private readonly ILogger<AccountService> _logger;

    public AccountService(AppDbContext context, ILogger<AccountService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<AccountDto>> GetUserAccountsAsync(Guid userId)
    {
        var accounts = await _context.Accounts
            .Include(a => a.Owner)
            .Include(a => a.Members)
                .ThenInclude(m => m.User)
            .Where(a => a.OwnerId == userId || a.Members.Any(m => m.UserId == userId))
            .Select(a => new AccountDto(
                a.Id,
                a.Name,
                a.Type,
                a.OwnerId,
                a.Owner.Name,
                a.CreatedAt,
                a.Members.Select(m => new AccountMemberDto(
                    m.Id,
                    m.UserId,
                    m.User.Name,
                    m.User.Email,
                    m.Role,
                    m.JoinedAt
                )).ToList()
            ))
            .ToListAsync();

        return accounts;
    }

    public async Task<AccountDto?> GetAccountByIdAsync(Guid userId, Guid accountId)
    {
        var account = await _context.Accounts
            .Include(a => a.Owner)
            .Include(a => a.Members)
                .ThenInclude(m => m.User)
            .Where(a => a.Id == accountId)
            .Where(a => a.OwnerId == userId || a.Members.Any(m => m.UserId == userId))
            .FirstOrDefaultAsync();

        if (account == null)
            return null;

        return new AccountDto(
            account.Id,
            account.Name,
            account.Type,
            account.OwnerId,
            account.Owner.Name,
            account.CreatedAt,
            account.Members.Select(m => new AccountMemberDto(
                m.Id,
                m.UserId,
                m.User.Name,
                m.User.Email,
                m.Role,
                m.JoinedAt
            )).ToList()
        );
    }

    public async Task<AccountDto> CreateAccountAsync(Guid userId, CreateAccountRequest request)
    {
        var account = new Account
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Type = request.Type,
            OwnerId = userId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Accounts.Add(account);

        var ownerMember = new AccountMember
        {
            Id = Guid.NewGuid(),
            AccountId = account.Id,
            UserId = userId,
            Role = AccountRole.Owner,
            JoinedAt = DateTime.UtcNow
        };

        _context.AccountMembers.Add(ownerMember);

        var defaultCategories = CreateDefaultCategories(account.Id);
        _context.Categories.AddRange(defaultCategories);

        await _context.SaveChangesAsync();

        return await GetAccountByIdAsync(userId, account.Id) 
               ?? throw new InvalidOperationException("Failed to retrieve created account");
    }

    public async Task<AccountDto> InviteMemberAsync(Guid userId, Guid accountId, InviteMemberRequest request)
    {
        var account = await _context.Accounts
            .Include(a => a.Members)
            .FirstOrDefaultAsync(a => a.Id == accountId);

        if (account == null)
            throw new InvalidOperationException("Account not found");

        if (account.OwnerId != userId)
            throw new UnauthorizedAccessException("Only the account owner can invite members");

        var invitedUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (invitedUser == null)
            throw new InvalidOperationException("User not found with this email");

        if (account.Members.Any(m => m.UserId == invitedUser.Id))
            throw new InvalidOperationException("User is already a member of this account");

        var member = new AccountMember
        {
            Id = Guid.NewGuid(),
            AccountId = accountId,
            UserId = invitedUser.Id,
            Role = request.Role,
            JoinedAt = DateTime.UtcNow
        };

        _context.AccountMembers.Add(member);
        await _context.SaveChangesAsync();

        return await GetAccountByIdAsync(userId, accountId) 
               ?? throw new InvalidOperationException("Failed to retrieve updated account");
    }

    public async Task RemoveMemberAsync(Guid userId, Guid accountId, Guid memberId)
    {
        var account = await _context.Accounts
            .Include(a => a.Members)
            .FirstOrDefaultAsync(a => a.Id == accountId);

        if (account == null)
            throw new InvalidOperationException("Account not found");

        if (account.OwnerId != userId)
            throw new UnauthorizedAccessException("Only the account owner can remove members");

        var member = account.Members.FirstOrDefault(m => m.Id == memberId);
        if (member == null)
            throw new InvalidOperationException("Member not found");

        if (member.Role == AccountRole.Owner)
            throw new InvalidOperationException("Cannot remove the account owner");

        _context.AccountMembers.Remove(member);
        await _context.SaveChangesAsync();
    }

    private static List<Category> CreateDefaultCategories(Guid accountId)
    {
        var now = DateTime.UtcNow;
        return new List<Category>
        {
            new() { Id = Guid.NewGuid(), AccountId = accountId, Name = "Alimentação", Color = "#10B981", Icon = "utensils", Type = TransactionType.Expense, CreatedAt = now },
            new() { Id = Guid.NewGuid(), AccountId = accountId, Name = "Transporte", Color = "#3B82F6", Icon = "car", Type = TransactionType.Expense, CreatedAt = now },
            new() { Id = Guid.NewGuid(), AccountId = accountId, Name = "Moradia", Color = "#8B5CF6", Icon = "home", Type = TransactionType.Expense, CreatedAt = now },
            new() { Id = Guid.NewGuid(), AccountId = accountId, Name = "Saúde", Color = "#EF4444", Icon = "heart", Type = TransactionType.Expense, CreatedAt = now },
            new() { Id = Guid.NewGuid(), AccountId = accountId, Name = "Lazer", Color = "#F59E0B", Icon = "gamepad", Type = TransactionType.Expense, CreatedAt = now },
            new() { Id = Guid.NewGuid(), AccountId = accountId, Name = "Educação", Color = "#06B6D4", Icon = "book", Type = TransactionType.Expense, CreatedAt = now },
            new() { Id = Guid.NewGuid(), AccountId = accountId, Name = "Outros", Color = "#6B7280", Icon = "tag", Type = TransactionType.Expense, CreatedAt = now },
            new() { Id = Guid.NewGuid(), AccountId = accountId, Name = "Salário", Color = "#10B981", Icon = "dollar-sign", Type = TransactionType.Income, CreatedAt = now },
            new() { Id = Guid.NewGuid(), AccountId = accountId, Name = "Freelance", Color = "#3B82F6", Icon = "briefcase", Type = TransactionType.Income, CreatedAt = now },
            new() { Id = Guid.NewGuid(), AccountId = accountId, Name = "Investimentos", Color = "#8B5CF6", Icon = "trending-up", Type = TransactionType.Income, CreatedAt = now },
            new() { Id = Guid.NewGuid(), AccountId = accountId, Name = "Outros", Color = "#6B7280", Icon = "tag", Type = TransactionType.Income, CreatedAt = now }
        };
    }
}
