using FinancialControl.Api.Data;
using FinancialControl.Api.DTOs;
using FinancialControl.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinancialControl.Api.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(AppDbContext context, IConfiguration configuration, ILogger<AuthService> logger)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
        {
            throw new InvalidOperationException("Email already registered");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            Name = request.Name,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);

        var personalAccount = new Account
        {
            Id = Guid.NewGuid(),
            Name = "Minha Conta",
            Type = AccountType.Personal,
            OwnerId = user.Id,
            CreatedAt = DateTime.UtcNow
        };

        _context.Accounts.Add(personalAccount);

        var defaultCategories = CreateDefaultCategories(personalAccount.Id);
        _context.Categories.AddRange(defaultCategories);

        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(user);

        return new AuthResponse(
            token,
            new UserDto(user.Id, user.Email, user.Name, user.CreatedAt)
        );
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }

        var token = GenerateJwtToken(user);

        return new AuthResponse(
            token,
            new UserDto(user.Id, user.Email, user.Name, user.CreatedAt)
        );
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);
        return user == null ? null : new UserDto(user.Id, user.Email, user.Name, user.CreatedAt);
    }

    private string GenerateJwtToken(User user)
    {
        var secret = _configuration["JwtSettings:Secret"] ?? throw new InvalidOperationException("JWT Secret not configured");
        var issuer = _configuration["JwtSettings:Issuer"];
        var audience = _configuration["JwtSettings:Audience"];
        var expirationMinutes = int.Parse(_configuration["JwtSettings:ExpirationMinutes"] ?? "1440");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Name, user.Name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static List<Category> CreateDefaultCategories(Guid accountId)
    {
        var now = DateTime.UtcNow;
        return new List<Category>
        {
            new() { Id = Guid.NewGuid(), AccountId = accountId, Name = "Alimentação", Color = "#10B981", Icon = "utensils", Type = TransactionType.Expense, CreatedAt = now },
            new() { Id = Guid.NewGuid(), AccountId = accountId, Name = "Transporte", Color = "#3B82F6", Icon = "car", Type = TransactionType.Expense, CreatedAt = now },
            new() { Id = Guid.NewGuid(), AccountId = accountId, Name = "Moradia", Color = "#8B5CF6", Icon = "home", Type = TransactionType.Expense, CreatedAt = now },
            new() { Id = Guid.NewGuid(), AccountId = accountId, Name = "Saúde", Color = "#EF4444", Icon = "heart", Type = TransactionType.Expense, CreatedAt = now },
            new() { Id = Guid.NewGuid(), AccountId = accountId, Name = "Lazer", Color = "#F59E0B", Icon = "gamepad", Type = TransactionType.Expense, CreatedAt = now },
            new() { Id = Guid.NewGuid(), AccountId = accountId, Name = "Educação", Color = "#06B6D4", Icon = "book", Type = TransactionType.Expense, CreatedAt = now },
            new() { Id = Guid.NewGuid(), AccountId = accountId, Name = "Outros", Color = "#6B7280", Icon = "tag", Type = TransactionType.Expense, CreatedAt = now },
            new() { Id = Guid.NewGuid(), AccountId = accountId, Name = "Salário", Color = "#10B981", Icon = "dollar-sign", Type = TransactionType.Income, CreatedAt = now },
            new() { Id = Guid.NewGuid(), AccountId = accountId, Name = "Freelance", Color = "#3B82F6", Icon = "briefcase", Type = TransactionType.Income, CreatedAt = now },
            new() { Id = Guid.NewGuid(), AccountId = accountId, Name = "Investimentos", Color = "#8B5CF6", Icon = "trending-up", Type = TransactionType.Income, CreatedAt = now },
            new() { Id = Guid.NewGuid(), AccountId = accountId, Name = "Outros", Color = "#6B7280", Icon = "tag", Type = TransactionType.Income, CreatedAt = now }
        };
    }
}
