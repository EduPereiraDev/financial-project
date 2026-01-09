using FinancialControl.Api.DTOs;
using FinancialControl.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinancialControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AlertsController : ControllerBase
{
    private readonly IAlertService _alertService;

    public AlertsController(IAlertService alertService)
    {
        _alertService = alertService;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }

    [HttpPost]
    public async Task<ActionResult<AlertDto>> CreateAlert([FromBody] CreateAlertRequest request)
    {
        try
        {
            var userId = GetUserId();
            var alert = await _alertService.CreateAlertAsync(userId, request);
            return CreatedAtAction(nameof(GetAlert), new { id = alert.Id }, alert);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao criar alerta", error = ex.Message });
        }
    }

    [HttpGet("account/{accountId}")]
    public async Task<ActionResult<List<AlertDto>>> GetAccountAlerts(Guid accountId)
    {
        try
        {
            var userId = GetUserId();
            var alerts = await _alertService.GetUserAlertsAsync(userId, accountId);
            return Ok(alerts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao buscar alertas", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AlertDto>> GetAlert(Guid id)
    {
        try
        {
            var userId = GetUserId();
            var alert = await _alertService.GetAlertByIdAsync(id, userId);

            if (alert == null)
            {
                return NotFound(new { message = "Alerta não encontrado" });
            }

            return Ok(alert);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao buscar alerta", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AlertDto>> UpdateAlert(Guid id, [FromBody] UpdateAlertRequest request)
    {
        try
        {
            var userId = GetUserId();
            var alert = await _alertService.UpdateAlertAsync(id, userId, request);

            if (alert == null)
            {
                return NotFound(new { message = "Alerta não encontrado" });
            }

            return Ok(alert);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao atualizar alerta", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAlert(Guid id)
    {
        try
        {
            var userId = GetUserId();
            var success = await _alertService.DeleteAlertAsync(id, userId);

            if (!success)
            {
                return NotFound(new { message = "Alerta não encontrado" });
            }

            return Ok(new { message = "Alerta excluído com sucesso" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao excluir alerta", error = ex.Message });
        }
    }

    [HttpPut("{id}/toggle")]
    public async Task<ActionResult> ToggleAlert(Guid id)
    {
        try
        {
            var userId = GetUserId();
            var success = await _alertService.ToggleAlertAsync(id, userId);

            if (!success)
            {
                return NotFound(new { message = "Alerta não encontrado" });
            }

            return Ok(new { message = "Status do alerta alterado com sucesso" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao alterar status do alerta", error = ex.Message });
        }
    }
}
