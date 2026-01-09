using FinancialControl.Api.DTOs;
using FinancialControl.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinancialControl.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BankingController : ControllerBase
{
    private readonly IBankingService _bankingService;

    public BankingController(IBankingService bankingService)
    {
        _bankingService = bankingService;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }

    [HttpPost("connections")]
    public async Task<ActionResult<BankConnectionDto>> CreateConnection([FromBody] CreateBankConnectionRequest request)
    {
        try
        {
            var userId = GetUserId();
            var connection = await _bankingService.CreateConnectionAsync(userId, request);
            return CreatedAtAction(nameof(GetConnection), new { id = connection.Id }, connection);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao criar conexão bancária", error = ex.Message });
        }
    }

    [HttpGet("connections")]
    public async Task<ActionResult<List<BankConnectionDto>>> GetConnections()
    {
        try
        {
            var userId = GetUserId();
            var connections = await _bankingService.GetUserConnectionsAsync(userId);
            return Ok(connections);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao buscar conexões", error = ex.Message });
        }
    }

    [HttpGet("connections/{id}")]
    public async Task<ActionResult<BankConnectionDto>> GetConnection(Guid id)
    {
        try
        {
            var userId = GetUserId();
            var connection = await _bankingService.GetConnectionByIdAsync(id, userId);

            if (connection == null)
            {
                return NotFound(new { message = "Conexão não encontrada" });
            }

            return Ok(connection);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao buscar conexão", error = ex.Message });
        }
    }

    [HttpPut("connections/{id}")]
    public async Task<ActionResult> UpdateConnection(Guid id, [FromBody] UpdateBankConnectionRequest request)
    {
        try
        {
            var userId = GetUserId();
            var success = await _bankingService.UpdateConnectionAsync(id, userId, request);

            if (!success)
            {
                return NotFound(new { message = "Conexão não encontrada" });
            }

            return Ok(new { message = "Conexão atualizada com sucesso" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao atualizar conexão", error = ex.Message });
        }
    }

    [HttpDelete("connections/{id}")]
    public async Task<ActionResult> DeleteConnection(Guid id)
    {
        try
        {
            var userId = GetUserId();
            var success = await _bankingService.DeleteConnectionAsync(id, userId);

            if (!success)
            {
                return NotFound(new { message = "Conexão não encontrada" });
            }

            return Ok(new { message = "Conexão excluída com sucesso" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao excluir conexão", error = ex.Message });
        }
    }

    [HttpPost("connections/{id}/sync")]
    public async Task<ActionResult<BankConnectionSyncResult>> SyncConnection(Guid id)
    {
        try
        {
            var userId = GetUserId();
            var result = await _bankingService.SyncConnectionAsync(id, userId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao sincronizar conexão", error = ex.Message });
        }
    }

    [HttpGet("transactions/pending")]
    public async Task<ActionResult<List<BankTransactionListDto>>> GetPendingTransactions()
    {
        try
        {
            var userId = GetUserId();
            var transactions = await _bankingService.GetPendingTransactionsAsync(userId);
            return Ok(transactions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao buscar transações pendentes", error = ex.Message });
        }
    }

    [HttpPost("transactions/import")]
    public async Task<ActionResult> ImportTransaction([FromBody] ImportBankTransactionRequest request)
    {
        try
        {
            var userId = GetUserId();
            var success = await _bankingService.ImportTransactionAsync(userId, request);

            if (!success)
            {
                return BadRequest(new { message = "Não foi possível importar a transação" });
            }

            return Ok(new { message = "Transação importada com sucesso" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao importar transação", error = ex.Message });
        }
    }

    [HttpPost("transactions/{id}/ignore")]
    public async Task<ActionResult> IgnoreTransaction(Guid id)
    {
        try
        {
            var userId = GetUserId();
            var success = await _bankingService.IgnoreTransactionAsync(userId, id);

            if (!success)
            {
                return BadRequest(new { message = "Não foi possível ignorar a transação" });
            }

            return Ok(new { message = "Transação ignorada com sucesso" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao ignorar transação", error = ex.Message });
        }
    }

    [HttpPost("connect-token")]
    public async Task<ActionResult<object>> CreateConnectToken()
    {
        try
        {
            var userId = GetUserId();
            var token = await _bankingService.CreateConnectTokenAsync(userId);
            return Ok(new { accessToken = token });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro ao criar Connect Token", error = ex.Message });
        }
    }
}
