using FinancialControl.Api.DTOs;
using FinancialControl.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinancialControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BudgetController : ControllerBase
{
    private readonly IBudgetService _budgetService;
    private readonly ILogger<BudgetController> _logger;

    public BudgetController(IBudgetService budgetService, ILogger<BudgetController> logger)
    {
        _budgetService = budgetService;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var userId = GetUserId();
            var budget = await _budgetService.GetByIdAsync(id, userId);

            if (budget == null)
                return NotFound(new { message = "Orçamento não encontrado" });

            return Ok(budget);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar orçamento {BudgetId}", id);
            return StatusCode(500, new { message = "Erro ao buscar orçamento" });
        }
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary([FromQuery] int? month = null, [FromQuery] int? year = null)
    {
        try
        {
            var userId = GetUserId();
            var now = DateTime.UtcNow;
            var targetMonth = month ?? now.Month;
            var targetYear = year ?? now.Year;

            var summary = await _budgetService.GetBudgetSummaryAsync(userId, targetMonth, targetYear);
            return Ok(summary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar resumo de orçamentos");
            return StatusCode(500, new { message = "Erro ao buscar resumo" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? month = null, [FromQuery] int? year = null)
    {
        try
        {
            var userId = GetUserId();
            var budgets = await _budgetService.GetAllAsync(userId, month, year);
            return Ok(budgets);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar orçamentos");
            return StatusCode(500, new { message = "Erro ao buscar orçamentos" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBudgetDto dto)
    {
        try
        {
            var userId = GetUserId();
            var budget = await _budgetService.CreateAsync(userId, dto);
            return CreatedAtAction(nameof(GetById), new { id = budget.Id }, budget);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar orçamento");
            return StatusCode(500, new { message = "Erro ao criar orçamento" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBudgetDto dto)
    {
        try
        {
            var userId = GetUserId();
            var budget = await _budgetService.UpdateAsync(id, userId, dto);

            if (budget == null)
                return NotFound(new { message = "Orçamento não encontrado" });

            return Ok(budget);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar orçamento {BudgetId}", id);
            return StatusCode(500, new { message = "Erro ao atualizar orçamento" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var userId = GetUserId();
            var success = await _budgetService.DeleteAsync(id, userId);

            if (!success)
                return NotFound(new { message = "Orçamento não encontrado" });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir orçamento {BudgetId}", id);
            return StatusCode(500, new { message = "Erro ao excluir orçamento" });
        }
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
        {
            throw new UnauthorizedAccessException("Usuário não autenticado");
        }
        return userId;
    }
}
