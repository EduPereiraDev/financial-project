# 🐛 Issues do Backend em Produção

## Status Atual
Backend deployado no Render com erros 500 em múltiplos endpoints.

---

## ❌ Endpoints com Erro 500 (Internal Server Error)

### 1. Budget Controller
```
GET /api/budget/summary?month=1&year=2026 → 500
```
**Problema**: Controller existe mas está retornando erro interno
**Possíveis causas**:
- Falta de dados no banco
- Query SQL com erro
- Falta de tratamento de exceção

### 2. Goal Controller
```
GET /api/goal/summary → 500
```
**Problema**: Controller existe mas está retornando erro interno
**Possíveis causas**:
- Falta de dados no banco
- Query SQL com erro
- Falta de tratamento de exceção

### 3. Banking Controller
```
GET /api/banking/transactions/pending → 500
GET /api/banking/connections → 500
```
**Problema**: Controllers existem mas estão retornando erro interno
**Possíveis causas**:
- Integração com Pluggy não configurada
- Credenciais inválidas
- Falta de dados no banco
- Falta de tratamento de exceção

---

## ❌ Endpoints com Erro 405 (Method Not Allowed)

### 1. Invitations Controller
```
GET /api/invitations/account/ → 405
```
**Problema**: AccountId vazio na URL
**Solução**: ✅ Já corrigida no frontend (alerta de accountId faltando)
**Ação necessária**: Usuário precisa fazer logout/login

---

## ❌ Endpoints com Erro 400 (Bad Request)

### 1. Recurring Transactions Controller
```
GET /api/recurringtransactions/account/ → 400
```
**Problema**: AccountId vazio na URL
**Solução**: ✅ Já corrigida no frontend (alerta de accountId faltando)
**Ação necessária**: Usuário precisa fazer logout/login

---

## 🔍 Diagnóstico Necessário

Para resolver os erros 500, precisamos verificar os **logs do backend no Render**:

### Como Acessar Logs no Render:
1. Acesse [Render Dashboard](https://dashboard.render.com)
2. Clique no serviço `financial-control-api`
3. Vá em **Logs**
4. Procure por linhas com `[ERR]` ou `Exception`

### O Que Procurar:
- Stack traces de exceções
- Mensagens de erro SQL
- Erros de conexão com banco
- Erros de autenticação/autorização
- Erros de integração externa (Pluggy)

---

## 🛠️ Soluções Recomendadas

### 1. Adicionar Try-Catch Global
Todos os controllers devem ter tratamento de exceção:

```csharp
[HttpGet("summary")]
public async Task<IActionResult> GetSummary()
{
    try
    {
        var summary = await _service.GetSummary();
        return Ok(summary);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Erro ao buscar summary");
        return StatusCode(500, new { 
            message = "Erro interno do servidor",
            error = ex.Message 
        });
    }
}
```

### 2. Validar Dados Antes de Query
```csharp
if (accountId == Guid.Empty)
{
    return BadRequest(new { message = "AccountId inválido" });
}

var account = await _context.Accounts.FindAsync(accountId);
if (account == null)
{
    return NotFound(new { message = "Conta não encontrada" });
}
```

### 3. Retornar Array Vazio em Vez de Erro
Quando não houver dados, retornar array vazio:

```csharp
var budgets = await _context.Budgets
    .Where(b => b.AccountId == accountId)
    .ToListAsync();

// Retorna [] em vez de erro
return Ok(budgets);
```

### 4. Configurar Pluggy Corretamente
Verificar se as credenciais estão corretas nas variáveis de ambiente:
```
Pluggy__ClientId=529a570b-1a75-4a3b-9607-b78d1f39c687
Pluggy__ClientSecret=7dfb7473-97cd-4d3c-a28f-5a1810809b82
```

---

## 📊 Prioridades

### 🔴 Alta Prioridade (Bloqueadores)
1. **Budget Summary** - Usado no dashboard principal
2. **Goal Summary** - Usado no dashboard principal
3. **Invitations** - Necessário para compartilhar contas

### 🟡 Média Prioridade (Funcionalidades Avançadas)
4. **Banking Connections** - Integração bancária (opcional)
5. **Banking Transactions** - Importação automática (opcional)

### 🟢 Baixa Prioridade (Já Corrigido no Frontend)
6. **Recurring Transactions** - ✅ Alerta implementado

---

## ✅ Próximos Passos

1. **Verificar logs do Render** para identificar causa exata dos erros 500
2. **Adicionar tratamento de exceção** em todos os controllers
3. **Validar dados** antes de fazer queries
4. **Retornar arrays vazios** quando não houver dados
5. **Testar cada endpoint** individualmente
6. **Documentar APIs** com Swagger

---

## 🚀 Solução Temporária (Frontend)

Enquanto o backend não é corrigido, o frontend já está preparado:

✅ **Service Worker** - Não cacheia POST/PUT/DELETE
✅ **AccountId Alert** - Avisa quando accountId está faltando
✅ **Error Handling** - Trata erros graciosamente
✅ **Empty States** - Mostra mensagens amigáveis quando não há dados

---

## 📝 Notas

- Todos os controllers existem no código
- O problema é **runtime**, não **compilação**
- Provavelmente falta de dados ou configuração
- Logs do Render são essenciais para diagnóstico

---

**Última Atualização**: 09/01/2026 - 16:32
**Status**: Aguardando análise de logs do backend
