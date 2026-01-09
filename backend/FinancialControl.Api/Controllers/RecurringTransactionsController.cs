using FinancialControl.Api.DTOs;
using FinancialControl.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialControl.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RecurringTransactionsController : ControllerBase
{
    private readonly IRecurringTransactionService _recurringTransactionService;

    public RecurringTransactionsController(IRecurringTransactionService recurringTransactionService)
    {
        _recurringTransactionService = recurringTransactionService;
    }

    [HttpGet("account/{accountId}")]
    public async Task<ActionResult<IEnumerable<RecurringTransactionDto>>> GetByAccount(Guid accountId)
    {
        try
        {
            var recurringTransactions = await _recurringTransactionService.GetAllByAccountAsync(accountId);
            return Ok(recurringTransactions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving recurring transactions", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RecurringTransactionDto>> GetById(Guid id, [FromQuery] Guid accountId)
    {
        try
        {
            var recurringTransaction = await _recurringTransactionService.GetByIdAsync(id, accountId);
            
            if (recurringTransaction == null)
                return NotFound(new { message = "Recurring transaction not found" });

            return Ok(recurringTransaction);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the recurring transaction", error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<RecurringTransactionDto>> Create([FromBody] CreateRecurringTransactionRequest request)
    {
        try
        {
            var recurringTransaction = await _recurringTransactionService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = recurringTransaction.Id, accountId = recurringTransaction.AccountId }, recurringTransaction);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the recurring transaction", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<RecurringTransactionDto>> Update(Guid id, [FromQuery] Guid accountId, [FromBody] UpdateRecurringTransactionRequest request)
    {
        try
        {
            var recurringTransaction = await _recurringTransactionService.UpdateAsync(id, accountId, request);
            return Ok(recurringTransaction);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the recurring transaction", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id, [FromQuery] Guid accountId)
    {
        try
        {
            await _recurringTransactionService.DeleteAsync(id, accountId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the recurring transaction", error = ex.Message });
        }
    }

    [HttpPost("process")]
    public async Task<ActionResult<object>> ProcessDueTransactions()
    {
        try
        {
            var processedCount = await _recurringTransactionService.ProcessDueRecurringTransactionsAsync();
            return Ok(new { message = $"Processed {processedCount} recurring transactions", count = processedCount });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while processing recurring transactions", error = ex.Message });
        }
    }
}
