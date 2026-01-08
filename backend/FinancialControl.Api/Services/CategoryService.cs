using FinancialControl.Api.Data;
using FinancialControl.Api.DTOs;
using FinancialControl.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialControl.Api.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(AppDbContext context, ILogger<CategoryService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<CategoryDto>> GetCategoriesAsync(Guid userId, Guid accountId)
    {
        var account = await _context.Accounts
            .Include(a => a.Members)
            .FirstOrDefaultAsync(a => a.Id == accountId);

        if (account == null)
            throw new InvalidOperationException("Account not found");

        var hasAccess = account.OwnerId == userId || account.Members.Any(m => m.UserId == userId);
        if (!hasAccess)
            throw new UnauthorizedAccessException("You don't have access to this account");

        var categories = await _context.Categories
            .Where(c => c.AccountId == accountId)
            .Select(c => new CategoryDto(
                c.Id,
                c.AccountId,
                c.Name,
                c.Color,
                c.Icon,
                c.Type,
                c.CreatedAt
            ))
            .ToListAsync();

        return categories;
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(Guid userId, Guid categoryId)
    {
        var category = await _context.Categories
            .Include(c => c.Account)
                .ThenInclude(a => a.Members)
            .FirstOrDefaultAsync(c => c.Id == categoryId);

        if (category == null)
            return null;

        var hasAccess = category.Account.OwnerId == userId || 
                       category.Account.Members.Any(m => m.UserId == userId);

        if (!hasAccess)
            return null;

        return new CategoryDto(
            category.Id,
            category.AccountId,
            category.Name,
            category.Color,
            category.Icon,
            category.Type,
            category.CreatedAt
        );
    }

    public async Task<CategoryDto> CreateCategoryAsync(Guid userId, CreateCategoryRequest request)
    {
        var account = await _context.Accounts
            .Include(a => a.Members)
            .FirstOrDefaultAsync(a => a.Id == request.AccountId);

        if (account == null)
            throw new InvalidOperationException("Account not found");

        var hasAccess = account.OwnerId == userId || 
                       account.Members.Any(m => m.UserId == userId && (m.Role == AccountRole.Owner || m.Role == AccountRole.Editor));

        if (!hasAccess)
            throw new UnauthorizedAccessException("You don't have permission to create categories in this account");

        var category = new Category
        {
            Id = Guid.NewGuid(),
            AccountId = request.AccountId,
            Name = request.Name,
            Color = request.Color,
            Icon = request.Icon,
            Type = request.Type,
            CreatedAt = DateTime.UtcNow
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return new CategoryDto(
            category.Id,
            category.AccountId,
            category.Name,
            category.Color,
            category.Icon,
            category.Type,
            category.CreatedAt
        );
    }

    public async Task<CategoryDto> UpdateCategoryAsync(Guid userId, Guid categoryId, UpdateCategoryRequest request)
    {
        var category = await _context.Categories
            .Include(c => c.Account)
                .ThenInclude(a => a.Members)
            .FirstOrDefaultAsync(c => c.Id == categoryId);

        if (category == null)
            throw new InvalidOperationException("Category not found");

        var hasAccess = category.Account.OwnerId == userId || 
                       category.Account.Members.Any(m => m.UserId == userId && (m.Role == AccountRole.Owner || m.Role == AccountRole.Editor));

        if (!hasAccess)
            throw new UnauthorizedAccessException("You don't have permission to update this category");

        category.Name = request.Name;
        category.Color = request.Color;
        category.Icon = request.Icon;

        await _context.SaveChangesAsync();

        return new CategoryDto(
            category.Id,
            category.AccountId,
            category.Name,
            category.Color,
            category.Icon,
            category.Type,
            category.CreatedAt
        );
    }

    public async Task DeleteCategoryAsync(Guid userId, Guid categoryId)
    {
        var category = await _context.Categories
            .Include(c => c.Account)
                .ThenInclude(a => a.Members)
            .Include(c => c.Transactions)
            .FirstOrDefaultAsync(c => c.Id == categoryId);

        if (category == null)
            throw new InvalidOperationException("Category not found");

        var hasAccess = category.Account.OwnerId == userId || 
                       category.Account.Members.Any(m => m.UserId == userId && (m.Role == AccountRole.Owner || m.Role == AccountRole.Editor));

        if (!hasAccess)
            throw new UnauthorizedAccessException("You don't have permission to delete this category");

        if (category.Transactions.Any())
            throw new InvalidOperationException("Cannot delete category with existing transactions");

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }
}
