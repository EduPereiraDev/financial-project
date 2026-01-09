using FinancialControl.Api.DTOs;
using FinancialControl.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinancialControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }

    [HttpGet]
    public async Task<ActionResult<List<NotificationListDto>>> GetNotifications(
        [FromQuery] bool unreadOnly = false,
        [FromQuery] int limit = 50)
    {
        try
        {
            var userId = GetUserId();
            var notifications = await _notificationService.GetUserNotificationsAsync(userId, unreadOnly, limit);
            return Ok(notifications);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao buscar notificações", error = ex.Message });
        }
    }

    [HttpGet("unread-count")]
    public async Task<ActionResult<int>> GetUnreadCount()
    {
        try
        {
            var userId = GetUserId();
            var count = await _notificationService.GetUnreadCountAsync(userId);
            return Ok(new { count });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao contar notificações não lidas", error = ex.Message });
        }
    }

    [HttpPut("{id}/read")]
    public async Task<ActionResult> MarkAsRead(Guid id)
    {
        try
        {
            var userId = GetUserId();
            var success = await _notificationService.MarkAsReadAsync(id, userId);

            if (!success)
            {
                return NotFound(new { message = "Notificação não encontrada" });
            }

            return Ok(new { message = "Notificação marcada como lida" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao marcar notificação como lida", error = ex.Message });
        }
    }

    [HttpPut("read-all")]
    public async Task<ActionResult> MarkAllAsRead()
    {
        try
        {
            var userId = GetUserId();
            await _notificationService.MarkAllAsReadAsync(userId);
            return Ok(new { message = "Todas as notificações marcadas como lidas" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao marcar todas as notificações como lidas", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteNotification(Guid id)
    {
        try
        {
            var userId = GetUserId();
            var success = await _notificationService.DeleteNotificationAsync(id, userId);

            if (!success)
            {
                return NotFound(new { message = "Notificação não encontrada" });
            }

            return Ok(new { message = "Notificação excluída com sucesso" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao excluir notificação", error = ex.Message });
        }
    }
}
