namespace FinancialControl.Api.DTOs;

public record RegisterRequest(
    string Email,
    string Password,
    string Name
);

public record LoginRequest(
    string Email,
    string Password
);

public record AuthResponse(
    string Token,
    UserDto User
);

public record UserDto(
    Guid Id,
    string Email,
    string Name,
    DateTime CreatedAt
);
