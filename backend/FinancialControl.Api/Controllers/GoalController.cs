using FinancialControl.Api.DTOs;
using FinancialControl.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinancialControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GoalController : ControllerBase
{
    private readonly IGoalService _goalService;
    private readonly ILogger<GoalController> _logger;

    public GoalController(IGoalService goalService, ILogger<GoalController> logger)
    {
        _goalService = goalService;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var userId = GetUserId();
            var goal = await _goalService.GetByIdAsync(id, userId);

            if (goal == null)
                return NotFound(new { message = "Meta não encontrada" });

            return Ok(goal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar meta {GoalId}", id);
            return StatusCode(500, new { message = "Erro ao buscar meta" });
        }
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        try
        {
            var userId = GetUserId();
            var summary = await _goalService.GetSummaryAsync(userId);
            return Ok(summary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar resumo de metas");
            return StatusCode(500, new { message = "Erro ao buscar resumo" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? status = null)
    {
        try
        {
            var userId = GetUserId();
            var goals = await _goalService.GetAllAsync(userId, status);
            return Ok(goals);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar metas");
            return StatusCode(500, new { message = "Erro ao buscar metas" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGoalDto dto)
    {
        try
        {
            var userId = GetUserId();
            var goal = await _goalService.CreateAsync(userId, dto);
            return CreatedAtAction(nameof(GetById), new { id = goal.Id }, goal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar meta");
            return StatusCode(500, new { message = "Erro ao criar meta" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGoalDto dto)
    {
        try
        {
            var userId = GetUserId();
            var goal = await _goalService.UpdateAsync(id, userId, dto);

            if (goal == null)
                return NotFound(new { message = "Meta não encontrada" });

            return Ok(goal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar meta {GoalId}", id);
            return StatusCode(500, new { message = "Erro ao atualizar meta" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var userId = GetUserId();
            var success = await _goalService.DeleteAsync(id, userId);

            if (!success)
                return NotFound(new { message = "Meta não encontrada" });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir meta {GoalId}", id);
            return StatusCode(500, new { message = "Erro ao excluir meta" });
        }
    }

    [HttpPost("{id}/contributions")]
    public async Task<IActionResult> AddContribution(Guid id, [FromBody] CreateContributionDto dto)
    {
        try
        {
            var userId = GetUserId();
            var goal = await _goalService.AddContributionAsync(id, userId, dto);

            if (goal == null)
                return NotFound(new { message = "Meta não encontrada" });

            return Ok(goal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar contribuição para meta {GoalId}", id);
            return StatusCode(500, new { message = "Erro ao adicionar contribuição" });
        }
    }

    [HttpGet("{id}/contributions")]
    public async Task<IActionResult> GetContributions(Guid id)
    {
        try
        {
            var userId = GetUserId();
            var contributions = await _goalService.GetContributionsAsync(id, userId);
            return Ok(contributions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar contribuições da meta {GoalId}", id);
            return StatusCode(500, new { message = "Erro ao buscar contribuições" });
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
