using FinancialControl.Api.Data;
using FinancialControl.Api.DTOs;
using FinancialControl.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Api.Services;

public interface IGoalService
{
    Task<GoalDto?> GetByIdAsync(Guid id, Guid userId);
    Task<GoalSummaryDto> GetSummaryAsync(Guid userId);
    Task<List<GoalDto>> GetAllAsync(Guid userId, string? status = null);
    Task<GoalDto> CreateAsync(Guid userId, CreateGoalDto dto);
    Task<GoalDto?> UpdateAsync(Guid id, Guid userId, UpdateGoalDto dto);
    Task<bool> DeleteAsync(Guid id, Guid userId);
    Task<GoalDto?> AddContributionAsync(Guid goalId, Guid userId, CreateContributionDto dto);
    Task<List<GoalContributionDto>> GetContributionsAsync(Guid goalId, Guid userId);
}

public class GoalService : IGoalService
{
    private readonly AppDbContext _context;
    private readonly ILogger<GoalService> _logger;

    public GoalService(AppDbContext context, ILogger<GoalService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<GoalDto?> GetByIdAsync(Guid id, Guid userId)
    {
        var goal = await _context.Goals
            .Include(g => g.Contributions.OrderByDescending(c => c.ContributedAt).Take(5))
            .FirstOrDefaultAsync(g => g.Id == id && g.UserId == userId);

        if (goal == null)
            return null;

        return MapToDto(goal);
    }

    public async Task<GoalSummaryDto> GetSummaryAsync(Guid userId)
    {
        var goals = await _context.Goals
            .Include(g => g.Contributions)
            .Where(g => g.UserId == userId)
            .ToListAsync();

        var goalDtos = goals.Select(MapToDto).ToList();

        return new GoalSummaryDto
        {
            TotalGoals = goals.Count,
            ActiveGoals = goals.Count(g => g.Status == GoalStatus.Active),
            CompletedGoals = goals.Count(g => g.Status == GoalStatus.Completed),
            TotalTargetAmount = goals.Sum(g => g.TargetAmount),
            TotalCurrentAmount = goals.Sum(g => g.CurrentAmount),
            OverallProgress = goals.Sum(g => g.TargetAmount) > 0 
                ? (goals.Sum(g => g.CurrentAmount) / goals.Sum(g => g.TargetAmount)) * 100 
                : 0,
            Goals = goalDtos.OrderByDescending(g => g.Priority).ThenBy(g => g.DaysRemaining).ToList()
        };
    }

    public async Task<List<GoalDto>> GetAllAsync(Guid userId, string? status = null)
    {
        var query = _context.Goals
            .Include(g => g.Contributions.OrderByDescending(c => c.ContributedAt).Take(5))
            .Where(g => g.UserId == userId);

        if (!string.IsNullOrEmpty(status) && Enum.TryParse<GoalStatus>(status, out var goalStatus))
        {
            query = query.Where(g => g.Status == goalStatus);
        }

        var goals = await query.ToListAsync();
        return goals.Select(MapToDto).ToList();
    }

    public async Task<GoalDto> CreateAsync(Guid userId, CreateGoalDto dto)
    {
        var goal = new Goal
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = dto.Name,
            Description = dto.Description,
            TargetAmount = dto.TargetAmount,
            CurrentAmount = 0,
            TargetDate = dto.TargetDate,
            Status = GoalStatus.Active,
            Priority = Enum.Parse<GoalPriority>(dto.Priority),
            ImageUrl = dto.ImageUrl,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Goals.Add(goal);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Meta criada: {GoalId} para usuário {UserId}", goal.Id, userId);

        return MapToDto(goal);
    }

    public async Task<GoalDto?> UpdateAsync(Guid id, Guid userId, UpdateGoalDto dto)
    {
        var goal = await _context.Goals
            .Include(g => g.Contributions)
            .FirstOrDefaultAsync(g => g.Id == id && g.UserId == userId);

        if (goal == null)
            return null;

        if (!string.IsNullOrEmpty(dto.Name))
            goal.Name = dto.Name;

        if (!string.IsNullOrEmpty(dto.Description))
            goal.Description = dto.Description;

        if (dto.TargetAmount.HasValue)
            goal.TargetAmount = dto.TargetAmount.Value;

        if (dto.TargetDate.HasValue)
            goal.TargetDate = dto.TargetDate.Value;

        if (!string.IsNullOrEmpty(dto.Status) && Enum.TryParse<GoalStatus>(dto.Status, out var status))
            goal.Status = status;

        if (!string.IsNullOrEmpty(dto.Priority) && Enum.TryParse<GoalPriority>(dto.Priority, out var priority))
            goal.Priority = priority;

        if (dto.ImageUrl != null)
            goal.ImageUrl = dto.ImageUrl;

        goal.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Meta atualizada: {GoalId}", id);

        return MapToDto(goal);
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        var goal = await _context.Goals
            .FirstOrDefaultAsync(g => g.Id == id && g.UserId == userId);

        if (goal == null)
            return false;

        _context.Goals.Remove(goal);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Meta excluída: {GoalId}", id);

        return true;
    }

    public async Task<GoalDto?> AddContributionAsync(Guid goalId, Guid userId, CreateContributionDto dto)
    {
        var goal = await _context.Goals
            .Include(g => g.Contributions)
            .FirstOrDefaultAsync(g => g.Id == goalId && g.UserId == userId);

        if (goal == null)
            return null;

        var contribution = new GoalContribution
        {
            Id = Guid.NewGuid(),
            GoalId = goalId,
            Amount = dto.Amount,
            Note = dto.Note,
            ContributedAt = DateTime.UtcNow
        };

        _context.GoalContributions.Add(contribution);

        // Atualizar valor atual da meta
        goal.CurrentAmount += dto.Amount;
        goal.UpdatedAt = DateTime.UtcNow;

        // Verificar se a meta foi completada
        if (goal.CurrentAmount >= goal.TargetAmount && goal.Status == GoalStatus.Active)
        {
            goal.Status = GoalStatus.Completed;
            _logger.LogInformation("Meta completada: {GoalId}", goalId);
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Contribuição adicionada: {ContributionId} para meta {GoalId}", contribution.Id, goalId);

        // Recarregar com contribuições recentes
        await _context.Entry(goal).Collection(g => g.Contributions).LoadAsync();
        
        return MapToDto(goal);
    }

    public async Task<List<GoalContributionDto>> GetContributionsAsync(Guid goalId, Guid userId)
    {
        var goal = await _context.Goals
            .FirstOrDefaultAsync(g => g.Id == goalId && g.UserId == userId);

        if (goal == null)
            return new List<GoalContributionDto>();

        var contributions = await _context.GoalContributions
            .Where(c => c.GoalId == goalId)
            .OrderByDescending(c => c.ContributedAt)
            .ToListAsync();

        return contributions.Select(c => new GoalContributionDto
        {
            Id = c.Id,
            GoalId = c.GoalId,
            Amount = c.Amount,
            Note = c.Note,
            ContributedAt = c.ContributedAt
        }).ToList();
    }

    private static GoalDto MapToDto(Goal goal)
    {
        var percentageComplete = goal.TargetAmount > 0 
            ? Math.Min((goal.CurrentAmount / goal.TargetAmount) * 100, 100) 
            : 0;

        var remainingAmount = Math.Max(goal.TargetAmount - goal.CurrentAmount, 0);

        var daysRemaining = (goal.TargetDate.Date - DateTime.UtcNow.Date).Days;

        var monthsRemaining = Math.Max(daysRemaining / 30.0, 0.1);
        var requiredMonthlyContribution = remainingAmount / (decimal)monthsRemaining;

        return new GoalDto
        {
            Id = goal.Id,
            UserId = goal.UserId,
            Name = goal.Name,
            Description = goal.Description,
            TargetAmount = goal.TargetAmount,
            CurrentAmount = goal.CurrentAmount,
            TargetDate = goal.TargetDate,
            Status = goal.Status.ToString(),
            Priority = goal.Priority.ToString(),
            ImageUrl = goal.ImageUrl,
            PercentageComplete = Math.Round(percentageComplete, 1),
            RemainingAmount = remainingAmount,
            DaysRemaining = daysRemaining,
            RequiredMonthlyContribution = Math.Round(requiredMonthlyContribution, 2),
            CreatedAt = goal.CreatedAt,
            UpdatedAt = goal.UpdatedAt,
            RecentContributions = goal.Contributions
                .OrderByDescending(c => c.ContributedAt)
                .Take(5)
                .Select(c => new GoalContributionDto
                {
                    Id = c.Id,
                    GoalId = c.GoalId,
                    Amount = c.Amount,
                    Note = c.Note,
                    ContributedAt = c.ContributedAt
                })
                .ToList()
        };
    }
}
