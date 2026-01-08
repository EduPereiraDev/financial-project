using FinancialControl.Api.DTOs;
using FinancialControl.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinancialControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly ILogger<AccountsController> _logger;

    public AccountsController(IAccountService accountService, ILogger<AccountsController> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim ?? throw new UnauthorizedAccessException("User ID not found in token"));
    }

    [HttpGet]
    public async Task<ActionResult<List<AccountDto>>> GetAccounts()
    {
        try
        {
            var userId = GetUserId();
            var accounts = await _accountService.GetUserAccountsAsync(userId);
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting accounts");
            return StatusCode(500, new { message = "An error occurred while retrieving accounts" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccountDto>> GetAccount(Guid id)
    {
        try
        {
            var userId = GetUserId();
            var account = await _accountService.GetAccountByIdAsync(userId, id);

            if (account == null)
                return NotFound(new { message = "Account not found" });

            return Ok(account);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting account {AccountId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the account" });
        }
    }

    [HttpPost]
    public async Task<ActionResult<AccountDto>> CreateAccount([FromBody] CreateAccountRequest request)
    {
        try
        {
            var userId = GetUserId();
            var account = await _accountService.CreateAccountAsync(userId, request);
            return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating account");
            return StatusCode(500, new { message = "An error occurred while creating the account" });
        }
    }

    [HttpPost("{id}/members")]
    public async Task<ActionResult<AccountDto>> InviteMember(Guid id, [FromBody] InviteMemberRequest request)
    {
        try
        {
            var userId = GetUserId();
            var account = await _accountService.InviteMemberAsync(userId, id, request);
            return Ok(account);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inviting member to account {AccountId}", id);
            return StatusCode(500, new { message = "An error occurred while inviting the member" });
        }
    }

    [HttpDelete("{id}/members/{memberId}")]
    public async Task<IActionResult> RemoveMember(Guid id, Guid memberId)
    {
        try
        {
            var userId = GetUserId();
            await _accountService.RemoveMemberAsync(userId, id, memberId);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing member from account {AccountId}", id);
            return StatusCode(500, new { message = "An error occurred while removing the member" });
        }
    }
}
