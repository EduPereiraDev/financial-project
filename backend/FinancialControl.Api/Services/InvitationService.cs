using FinancialControl.Api.Data;
using FinancialControl.Api.DTOs;
using FinancialControl.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace FinancialControl.Api.Services;

public interface IInvitationService
{
    Task<InvitationDto> CreateInvitationAsync(Guid userId, CreateInvitationRequest request);
    Task<List<InvitationListDto>> GetAccountInvitationsAsync(Guid accountId);
    Task<InvitationDto> GetInvitationByTokenAsync(string token);
    Task<bool> AcceptInvitationAsync(Guid userId, string token);
    Task<bool> CancelInvitationAsync(Guid invitationId, Guid userId);
    Task CleanupExpiredInvitationsAsync();
}

public class InvitationService : IInvitationService
{
    private readonly AppDbContext _context;

    public InvitationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<InvitationDto> CreateInvitationAsync(Guid userId, CreateInvitationRequest request)
    {
        // Verificar se o usuário é owner da conta
        var member = await _context.AccountMembers
            .FirstOrDefaultAsync(am => am.AccountId == request.AccountId && am.UserId == userId);

        if (member == null || member.Role != AccountRole.Owner)
        {
            throw new UnauthorizedAccessException("Only account owners can send invitations");
        }

        // Verificar se o email já é membro
        var existingMember = await _context.AccountMembers
            .Include(am => am.User)
            .FirstOrDefaultAsync(am => am.AccountId == request.AccountId && am.User.Email == request.InvitedEmail);

        if (existingMember != null)
        {
            throw new InvalidOperationException("User is already a member of this account");
        }

        // Verificar se já existe convite pendente
        var existingInvitation = await _context.Invitations
            .FirstOrDefaultAsync(i => i.AccountId == request.AccountId && 
                                     i.InvitedEmail == request.InvitedEmail && 
                                     i.Status == InvitationStatus.Pending);

        if (existingInvitation != null)
        {
            throw new InvalidOperationException("There is already a pending invitation for this email");
        }

        // Criar convite
        var invitation = new Invitation
        {
            Id = Guid.NewGuid(),
            AccountId = request.AccountId,
            InvitedByUserId = userId,
            InvitedEmail = request.InvitedEmail,
            Role = request.Role,
            Status = InvitationStatus.Pending,
            Token = GenerateSecureToken(),
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        };

        _context.Invitations.Add(invitation);
        await _context.SaveChangesAsync();

        // Retornar DTO
        var account = await _context.Accounts.FindAsync(request.AccountId);
        var invitedByUser = await _context.Users.FindAsync(userId);

        return new InvitationDto(
            invitation.Id,
            invitation.AccountId,
            account!.Name,
            invitation.InvitedByUserId,
            invitedByUser!.Name,
            invitation.InvitedEmail,
            invitation.Role,
            invitation.Status,
            invitation.ExpiresAt,
            invitation.CreatedAt,
            invitation.AcceptedAt
        );
    }

    public async Task<List<InvitationListDto>> GetAccountInvitationsAsync(Guid accountId)
    {
        var invitations = await _context.Invitations
            .Include(i => i.InvitedByUser)
            .Where(i => i.AccountId == accountId)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();

        return invitations.Select(i => new InvitationListDto(
            i.Id,
            i.InvitedEmail,
            i.Role,
            i.Status,
            i.ExpiresAt,
            i.CreatedAt,
            i.InvitedByUser.Name
        )).ToList();
    }

    public async Task<InvitationDto> GetInvitationByTokenAsync(string token)
    {
        var invitation = await _context.Invitations
            .Include(i => i.Account)
            .Include(i => i.InvitedByUser)
            .FirstOrDefaultAsync(i => i.Token == token);

        if (invitation == null)
        {
            throw new KeyNotFoundException("Invitation not found");
        }

        if (invitation.Status != InvitationStatus.Pending)
        {
            throw new InvalidOperationException("This invitation is no longer valid");
        }

        if (invitation.ExpiresAt < DateTime.UtcNow)
        {
            invitation.Status = InvitationStatus.Expired;
            await _context.SaveChangesAsync();
            throw new InvalidOperationException("This invitation has expired");
        }

        return new InvitationDto(
            invitation.Id,
            invitation.AccountId,
            invitation.Account.Name,
            invitation.InvitedByUserId,
            invitation.InvitedByUser.Name,
            invitation.InvitedEmail,
            invitation.Role,
            invitation.Status,
            invitation.ExpiresAt,
            invitation.CreatedAt,
            invitation.AcceptedAt
        );
    }

    public async Task<bool> AcceptInvitationAsync(Guid userId, string token)
    {
        var invitation = await _context.Invitations
            .Include(i => i.Account)
            .FirstOrDefaultAsync(i => i.Token == token);

        if (invitation == null)
        {
            throw new KeyNotFoundException("Invitation not found");
        }

        if (invitation.Status != InvitationStatus.Pending)
        {
            throw new InvalidOperationException("This invitation is no longer valid");
        }

        if (invitation.ExpiresAt < DateTime.UtcNow)
        {
            invitation.Status = InvitationStatus.Expired;
            await _context.SaveChangesAsync();
            throw new InvalidOperationException("This invitation has expired");
        }

        // Verificar se o email do usuário corresponde
        var user = await _context.Users.FindAsync(userId);
        if (user == null || user.Email != invitation.InvitedEmail)
        {
            throw new UnauthorizedAccessException("This invitation is not for your email");
        }

        // Verificar se já é membro
        var existingMember = await _context.AccountMembers
            .FirstOrDefaultAsync(am => am.AccountId == invitation.AccountId && am.UserId == userId);

        if (existingMember != null)
        {
            throw new InvalidOperationException("You are already a member of this account");
        }

        // Adicionar como membro
        var member = new AccountMember
        {
            Id = Guid.NewGuid(),
            AccountId = invitation.AccountId,
            UserId = userId,
            Role = invitation.Role,
            JoinedAt = DateTime.UtcNow
        };

        _context.AccountMembers.Add(member);

        // Atualizar convite
        invitation.Status = InvitationStatus.Accepted;
        invitation.AcceptedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> CancelInvitationAsync(Guid invitationId, Guid userId)
    {
        var invitation = await _context.Invitations
            .FirstOrDefaultAsync(i => i.Id == invitationId);

        if (invitation == null)
        {
            throw new KeyNotFoundException("Invitation not found");
        }

        // Verificar se o usuário é owner da conta
        var member = await _context.AccountMembers
            .FirstOrDefaultAsync(am => am.AccountId == invitation.AccountId && am.UserId == userId);

        if (member == null || member.Role != AccountRole.Owner)
        {
            throw new UnauthorizedAccessException("Only account owners can cancel invitations");
        }

        if (invitation.Status != InvitationStatus.Pending)
        {
            throw new InvalidOperationException("Only pending invitations can be cancelled");
        }

        invitation.Status = InvitationStatus.Cancelled;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task CleanupExpiredInvitationsAsync()
    {
        var expiredInvitations = await _context.Invitations
            .Where(i => i.Status == InvitationStatus.Pending && i.ExpiresAt < DateTime.UtcNow)
            .ToListAsync();

        foreach (var invitation in expiredInvitations)
        {
            invitation.Status = InvitationStatus.Expired;
        }

        if (expiredInvitations.Any())
        {
            await _context.SaveChangesAsync();
        }
    }

    private static string GenerateSecureToken()
    {
        var bytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
        }
        return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
    }
}
