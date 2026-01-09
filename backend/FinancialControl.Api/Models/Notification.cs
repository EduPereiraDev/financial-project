namespace FinancialControl.Api.Models;

public class Notification
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? AlertId { get; set; }
    public NotificationType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }
    public string? ActionUrl { get; set; }
    public string? Metadata { get; set; } // JSON para dados adicionais

    // Navigation properties
    public User User { get; set; } = null!;
    public Alert? Alert { get; set; }
}

public enum NotificationType
{
    Info = 1,
    Warning = 2,
    Error = 3,
    Success = 4,
    Alert = 5
}
