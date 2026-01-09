using FinancialControl.Api.DTOs;
using FinancialControl.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinancialControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InvitationsController : ControllerBase
{
    private readonly IInvitationService _invitationService;

    public InvitationsController(IInvitationService invitationService)
    {
        _invitationService = invitationService;
    }

    [HttpPost]
    public async Task<ActionResult<InvitationDto>> CreateInvitation([FromBody] CreateInvitationRequest request)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            var invitation = await _invitationService.CreateInvitationAsync(userId, request);
            return Ok(invitation);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("account/{accountId}")]
    public async Task<ActionResult<List<InvitationListDto>>> GetAccountInvitations(Guid accountId)
    {
        var invitations = await _invitationService.GetAccountInvitationsAsync(accountId);
        return Ok(invitations);
    }

    [HttpGet("token/{token}")]
    [AllowAnonymous]
    public async Task<ActionResult<InvitationDto>> GetInvitationByToken(string token)
    {
        try
        {
            var invitation = await _invitationService.GetInvitationByTokenAsync(token);
            return Ok(invitation);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("accept")]
    public async Task<ActionResult> AcceptInvitation([FromBody] AcceptInvitationRequest request)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            await _invitationService.AcceptInvitationAsync(userId, request.Token);
            return Ok(new { message = "Invitation accepted successfully" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{invitationId}")]
    public async Task<ActionResult> CancelInvitation(Guid invitationId)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException());
            await _invitationService.CancelInvitationAsync(invitationId, userId);
            return Ok(new { message = "Invitation cancelled successfully" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
