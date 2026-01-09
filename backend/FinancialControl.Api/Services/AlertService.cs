using FinancialControl.Api.Data;
using FinancialControl.Api.DTOs;
using FinancialControl.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Api.Services;

public interface IAlertService
{
    Task<AlertDto> CreateAlertAsync(Guid userId, CreateAlertRequest request);
    Task<List<AlertDto>> GetUserAlertsAsync(Guid userId, Guid accountId);
    Task<AlertDto?> GetAlertByIdAsync(Guid alertId, Guid userId);
    Task<AlertDto?> UpdateAlertAsync(Guid alertId, Guid userId, UpdateAlertRequest request);
    Task<bool> DeleteAlertAsync(Guid alertId, Guid userId);
    Task<bool> ToggleAlertAsync(Guid alertId, Guid userId);
}

public class AlertService : IAlertService
{
    private readonly AppDbContext _context;

    public AlertService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AlertDto> CreateAlertAsync(Guid userId, CreateAlertRequest request)
    {
        var alert = new Alert
        {
            Id = Guid.NewGuid(),
            AccountId = request.AccountId,
            UserId = userId,
            Type = request.Type,
            Name = request.Name,
            Description = request.Description,
            IsActive = true,
            ThresholdAmount = request.ThresholdAmount,
            ThresholdDays = request.ThresholdDays,
            CategoryId = request.CategoryId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Alerts.Add(alert);
        await _context.SaveChangesAsync();

        return await MapToDto(alert);
    }

    public async Task<List<AlertDto>> GetUserAlertsAsync(Guid userId, Guid accountId)
    {
        var alerts = await _context.Alerts
            .Include(a => a.Category)
            .Where(a => a.UserId == userId && a.AccountId == accountId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        return alerts.Select(MapToDto).Select(t => t.Result).ToList();
    }

    public async Task<AlertDto?> GetAlertByIdAsync(Guid alertId, Guid userId)
    {
        var alert = await _context.Alerts
            .Include(a => a.Category)
            .FirstOrDefaultAsync(a => a.Id == alertId && a.UserId == userId);

        return alert == null ? null : await MapToDto(alert);
    }

    public async Task<AlertDto?> UpdateAlertAsync(Guid alertId, Guid userId, UpdateAlertRequest request)
    {
        var alert = await _context.Alerts
            .FirstOrDefaultAsync(a => a.Id == alertId && a.UserId == userId);

        if (alert == null)
        {
            return null;
        }

        if (request.Name != null) alert.Name = request.Name;
        if (request.Description != null) alert.Description = request.Description;
        if (request.IsActive.HasValue) alert.IsActive = request.IsActive.Value;
        if (request.ThresholdAmount.HasValue) alert.ThresholdAmount = request.ThresholdAmount;
        if (request.ThresholdDays.HasValue) alert.ThresholdDays = request.ThresholdDays;
        if (request.CategoryId.HasValue) alert.CategoryId = request.CategoryId;

        await _context.SaveChangesAsync();

        return await MapToDto(alert);
    }

    public async Task<bool> DeleteAlertAsync(Guid alertId, Guid userId)
    {
        var alert = await _context.Alerts
            .FirstOrDefaultAsync(a => a.Id == alertId && a.UserId == userId);

        if (alert == null)
        {
            return false;
        }

        _context.Alerts.Remove(alert);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleAlertAsync(Guid alertId, Guid userId)
    {
        var alert = await _context.Alerts
            .FirstOrDefaultAsync(a => a.Id == alertId && a.UserId == userId);

        if (alert == null)
        {
            return false;
        }

        alert.IsActive = !alert.IsActive;
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<AlertDto> MapToDto(Alert alert)
    {
        string? categoryName = null;
        if (alert.CategoryId.HasValue)
        {
            var category = await _context.Categories.FindAsync(alert.CategoryId.Value);
            categoryName = category?.Name;
        }

        return new AlertDto(
            alert.Id,
            alert.AccountId,
            alert.UserId,
            alert.Type,
            alert.Name,
            alert.Description,
            alert.IsActive,
            alert.ThresholdAmount,
            alert.ThresholdDays,
            alert.CategoryId,
            categoryName,
            alert.CreatedAt,
            alert.LastTriggeredAt
        );
    }
}
