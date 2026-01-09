using FinancialControl.Api.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FinancialControl.Api.Services;

public interface IPluggyService
{
    Task<string> CreateConnectTokenAsync(Guid userId);
    Task<List<BankTransaction>> FetchTransactionsAsync(string itemId, DateTime? from = null);
    Task<PluggyItemResponse?> GetItemAsync(string itemId);
    Task DeleteItemAsync(string itemId);
}

public class PluggyService : IPluggyService
{
    private readonly HttpClient _httpClient;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly ILogger<PluggyService> _logger;
    private string? _apiKey;
    private DateTime _apiKeyExpiration;

    public PluggyService(IConfiguration configuration, ILogger<PluggyService> logger, IHttpClientFactory httpClientFactory)
    {
        _clientId = configuration["Pluggy:ClientId"] ?? throw new InvalidOperationException("Pluggy:ClientId não configurado. Configure no appsettings.json ou via variáveis de ambiente.");
        _clientSecret = configuration["Pluggy:ClientSecret"] ?? throw new InvalidOperationException("Pluggy:ClientSecret não configurado. Configure no appsettings.json ou via variáveis de ambiente.");
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("https://api.pluggy.ai/");
        _logger = logger;
    }

    private async Task<string> GetApiKeyAsync()
    {
        if (!string.IsNullOrEmpty(_apiKey) && DateTime.UtcNow < _apiKeyExpiration)
            return _apiKey;

        try
        {
            var authRequest = new { clientId = _clientId, clientSecret = _clientSecret };
            var response = await _httpClient.PostAsJsonAsync("auth", authRequest);
            
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Falha na autenticação Pluggy: {response.StatusCode} - {error}");
            }

            var result = await response.Content.ReadFromJsonAsync<PluggyAuthResponse>();
            _apiKey = result?.ApiKey ?? throw new InvalidOperationException("API Key não retornada pelo Pluggy");
            _apiKeyExpiration = DateTime.UtcNow.AddHours(1);
            
            _logger.LogInformation("Autenticação Pluggy realizada com sucesso");
            return _apiKey;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao autenticar no Pluggy");
            throw;
        }
    }

    public async Task<string> CreateConnectTokenAsync(Guid userId)
    {
        try
        {
            var apiKey = await GetApiKeyAsync();
            
            var request = new HttpRequestMessage(HttpMethod.Post, "connect_token");
            request.Headers.Add("X-API-KEY", apiKey);
            request.Content = JsonContent.Create(new { clientUserId = userId.ToString() });

            var response = await _httpClient.SendAsync(request);
            
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Falha ao criar Connect Token: {response.StatusCode} - {error}");
            }

            var result = await response.Content.ReadFromJsonAsync<PluggyConnectTokenResponse>();
            var token = result?.AccessToken ?? throw new InvalidOperationException("Connect Token não retornado");
            
            _logger.LogInformation("Connect Token criado com sucesso para usuário {UserId}", userId);
            return token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar Connect Token para usuário {UserId}", userId);
            throw;
        }
    }

    public async Task<List<BankTransaction>> FetchTransactionsAsync(string itemId, DateTime? from = null)
    {
        try
        {
            var apiKey = await GetApiKeyAsync();
            var fromDate = from ?? DateTime.UtcNow.AddMonths(-3);
            
            // Buscar contas do item
            var accountsRequest = new HttpRequestMessage(HttpMethod.Get, $"accounts?itemId={itemId}");
            accountsRequest.Headers.Add("X-API-KEY", apiKey);

            var accountsResponse = await _httpClient.SendAsync(accountsRequest);
            
            if (!accountsResponse.IsSuccessStatusCode)
            {
                var error = await accountsResponse.Content.ReadAsStringAsync();
                _logger.LogWarning("Falha ao buscar contas do item {ItemId}: {Error}", itemId, error);
                return new List<BankTransaction>();
            }

            var accounts = await accountsResponse.Content.ReadFromJsonAsync<PluggyAccountsResponse>();
            var transactions = new List<BankTransaction>();

            if (accounts?.Results == null || !accounts.Results.Any())
            {
                _logger.LogInformation("Nenhuma conta encontrada para o item {ItemId}", itemId);
                return transactions;
            }

            // Buscar transações de cada conta
            foreach (var account in accounts.Results)
            {
                var transRequest = new HttpRequestMessage(HttpMethod.Get, 
                    $"transactions?accountId={account.Id}&from={fromDate:yyyy-MM-dd}&to={DateTime.UtcNow:yyyy-MM-dd}");
                transRequest.Headers.Add("X-API-KEY", apiKey);

                var transResponse = await _httpClient.SendAsync(transRequest);
                
                if (!transResponse.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Falha ao buscar transações da conta {AccountId}", account.Id);
                    continue;
                }

                var accountTransactions = await transResponse.Content.ReadFromJsonAsync<PluggyTransactionsResponse>();
                
                if (accountTransactions?.Results == null) continue;

                foreach (var transaction in accountTransactions.Results)
                {
                    transactions.Add(new BankTransaction
                    {
                        Id = Guid.NewGuid(),
                        ExternalId = transaction.Id,
                        Description = transaction.Description ?? "Transação bancária",
                        Amount = Math.Abs((decimal)transaction.Amount),
                        Date = transaction.Date,
                        Type = transaction.Amount < 0 ? BankTransactionType.Debit : BankTransactionType.Credit,
                        Status = BankTransactionStatus.Pending,
                        Category = transaction.Category,
                        CreatedAt = DateTime.UtcNow,
                        Metadata = JsonSerializer.Serialize(new
                        {
                            accountId = account.Id,
                            accountName = account.Name,
                            accountType = account.Type,
                            balance = account.Balance,
                            currencyCode = transaction.CurrencyCode,
                            merchantName = transaction.Merchant?.Name
                        })
                    });
                }
            }

            _logger.LogInformation("Buscadas {Count} transações do item {ItemId}", transactions.Count, itemId);
            return transactions;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar transações do item {ItemId}", itemId);
            throw;
        }
    }

    public async Task<PluggyItemResponse?> GetItemAsync(string itemId)
    {
        try
        {
            var apiKey = await GetApiKeyAsync();
            
            var request = new HttpRequestMessage(HttpMethod.Get, $"items/{itemId}");
            request.Headers.Add("X-API-KEY", apiKey);

            var response = await _httpClient.SendAsync(request);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Falha ao buscar item {ItemId}: {StatusCode}", itemId, response.StatusCode);
                return null;
            }

            var item = await response.Content.ReadFromJsonAsync<PluggyItemResponse>();
            _logger.LogInformation("Item {ItemId} recuperado com sucesso", itemId);
            return item;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar item {ItemId}", itemId);
            return null;
        }
    }

    public async Task DeleteItemAsync(string itemId)
    {
        try
        {
            var apiKey = await GetApiKeyAsync();
            
            var request = new HttpRequestMessage(HttpMethod.Delete, $"items/{itemId}");
            request.Headers.Add("X-API-KEY", apiKey);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            
            _logger.LogInformation("Item {ItemId} deletado com sucesso", itemId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar item {ItemId}", itemId);
            throw;
        }
    }
}

// DTOs para Pluggy API
public class PluggyAuthResponse
{
    [JsonPropertyName("apiKey")]
    public string ApiKey { get; set; } = string.Empty;
}

public class PluggyConnectTokenResponse
{
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; } = string.Empty;
}

public class PluggyAccountsResponse
{
    [JsonPropertyName("results")]
    public List<PluggyAccount> Results { get; set; } = new();
}

public class PluggyAccount
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
    
    [JsonPropertyName("balance")]
    public double Balance { get; set; }
}

public class PluggyTransactionsResponse
{
    [JsonPropertyName("results")]
    public List<PluggyTransaction> Results { get; set; } = new();
}

public class PluggyTransaction
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    [JsonPropertyName("amount")]
    public double Amount { get; set; }
    
    [JsonPropertyName("date")]
    public DateTime Date { get; set; }
    
    [JsonPropertyName("category")]
    public string? Category { get; set; }
    
    [JsonPropertyName("currencyCode")]
    public string CurrencyCode { get; set; } = "BRL";
    
    [JsonPropertyName("merchant")]
    public PluggyMerchant? Merchant { get; set; }
}

public class PluggyMerchant
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class PluggyItemResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
    
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }
    
    [JsonPropertyName("lastUpdatedAt")]
    public DateTime? LastUpdatedAt { get; set; }
    
    [JsonPropertyName("connector")]
    public PluggyConnectorResponse? Connector { get; set; }
}

public class PluggyConnectorResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("imageUrl")]
    public string ImageUrl { get; set; } = string.Empty;
}
