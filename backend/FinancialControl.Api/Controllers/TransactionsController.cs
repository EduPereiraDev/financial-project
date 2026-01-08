using FinancialControl.Api.DTOs;
using FinancialControl.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinancialControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly ILogger<TransactionsController> _logger;

    public TransactionsController(ITransactionService transactionService, ILogger<TransactionsController> logger)
    {
        _transactionService = transactionService;
        _logger = logger;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim ?? throw new UnauthorizedAccessException("User ID not found in token"));
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<TransactionDto>>> GetTransactions([FromQuery] TransactionFilterRequest filter)
    {
        try
        {
            var userId = GetUserId();
            var result = await _transactionService.GetTransactionsAsync(userId, filter);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting transactions");
            return StatusCode(500, new { message = "An error occurred while retrieving transactions" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionDto>> GetTransaction(Guid id)
    {
        try
        {
            var userId = GetUserId();
            var transaction = await _transactionService.GetTransactionByIdAsync(userId, id);

            if (transaction == null)
                return NotFound(new { message = "Transaction not found" });

            return Ok(transaction);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting transaction {TransactionId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the transaction" });
        }
    }

    [HttpPost]
    public async Task<ActionResult<TransactionDto>> CreateTransaction([FromBody] CreateTransactionRequest request)
    {
        try
        {
            var userId = GetUserId();
            var transaction = await _transactionService.CreateTransactionAsync(userId, request);
            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, transaction);
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
            _logger.LogError(ex, "Error creating transaction");
            return StatusCode(500, new { message = "An error occurred while creating the transaction" });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TransactionDto>> UpdateTransaction(Guid id, [FromBody] UpdateTransactionRequest request)
    {
        try
        {
            var userId = GetUserId();
            var transaction = await _transactionService.UpdateTransactionAsync(userId, id, request);
            return Ok(transaction);
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
            _logger.LogError(ex, "Error updating transaction {TransactionId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the transaction" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTransaction(Guid id)
    {
        try
        {
            var userId = GetUserId();
            await _transactionService.DeleteTransactionAsync(userId, id);
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
            _logger.LogError(ex, "Error deleting transaction {TransactionId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the transaction" });
        }
    }
}
