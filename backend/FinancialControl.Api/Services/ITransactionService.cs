using FinancialControl.Api.DTOs;

namespace FinancialControl.Api.Services;

public interface ITransactionService
{
    Task<PagedResult<TransactionDto>> GetTransactionsAsync(Guid userId, TransactionFilterRequest filter);
    Task<TransactionDto?> GetTransactionByIdAsync(Guid userId, Guid transactionId);
    Task<TransactionDto> CreateTransactionAsync(Guid userId, CreateTransactionRequest request);
    Task<TransactionDto> UpdateTransactionAsync(Guid userId, Guid transactionId, UpdateTransactionRequest request);
    Task DeleteTransactionAsync(Guid userId, Guid transactionId);
}
