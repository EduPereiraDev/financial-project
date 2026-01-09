using Hangfire.Dashboard;

namespace FinancialControl.Api.Infrastructure;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        // Em produção, você pode adicionar autenticação JWT aqui
        // Por enquanto, permitir acesso em desenvolvimento
        var httpContext = context.GetHttpContext();
        
        // Permitir acesso apenas em desenvolvimento ou com autenticação
        return httpContext.Request.Host.Host.Contains("localhost") || 
               httpContext.Request.Host.Host.Contains("127.0.0.1") ||
               httpContext.User.Identity?.IsAuthenticated == true;
    }
}
