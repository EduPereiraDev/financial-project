# 📡 API Endpoints - Financial Control

**Base URL**: `http://localhost:5000/api`  
**Autenticação**: JWT Bearer Token (exceto endpoints de Auth)

---

## 🔐 Authentication

### POST /api/auth/register
Registra um novo usuário e cria automaticamente uma conta pessoal com 11 categorias padrão.

**Request Body**:
```json
{
  "email": "user@example.com",
  "password": "SecurePass123!",
  "name": "John Doe"
}
```

**Response** (201 Created):
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "email": "user@example.com",
    "name": "John Doe",
    "createdAt": "2026-01-08T17:00:00Z"
  }
}
```

**Erros**:
- `400 Bad Request`: Email já registrado
- `500 Internal Server Error`: Erro no servidor

---

### POST /api/auth/login
Autentica um usuário e retorna um token JWT.

**Request Body**:
```json
{
  "email": "user@example.com",
  "password": "SecurePass123!"
}
```

**Response** (200 OK):
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "email": "user@example.com",
    "name": "John Doe",
    "createdAt": "2026-01-08T17:00:00Z"
  }
}
```

**Erros**:
- `401 Unauthorized`: Credenciais inválidas
- `500 Internal Server Error`: Erro no servidor

---

## 💰 Transactions

**Autenticação**: Requerida (Bearer Token)

### GET /api/transactions
Lista transações do usuário com filtros e paginação.

**Query Parameters**:
- `accountId` (Guid, opcional): Filtrar por conta específica
- `accountIds` (Guid[], opcional): Filtrar por múltiplas contas
- `categoryId` (Guid, opcional): Filtrar por categoria
- `type` (string, opcional): "Income" ou "Expense"
- `startDate` (DateTime, opcional): Data inicial
- `endDate` (DateTime, opcional): Data final
- `minAmount` (decimal, opcional): Valor mínimo
- `maxAmount` (decimal, opcional): Valor máximo
- `searchTerm` (string, opcional): Busca na descrição
- `page` (int, default: 1): Número da página
- `pageSize` (int, default: 25, max: 100): Itens por página

**Response** (200 OK):
```json
{
  "items": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "accountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "accountName": "Minha Conta",
      "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "userName": "John Doe",
      "categoryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "categoryName": "Alimentação",
      "categoryColor": "#10B981",
      "categoryIcon": "utensils",
      "amount": 150.50,
      "description": "Supermercado",
      "date": "2026-01-08T17:00:00Z",
      "type": "Expense",
      "createdAt": "2026-01-08T17:00:00Z",
      "updatedAt": "2026-01-08T17:00:00Z"
    }
  ],
  "totalCount": 100,
  "page": 1,
  "pageSize": 25,
  "totalPages": 4
}
```

---

### GET /api/transactions/{id}
Obtém uma transação específica.

**Response** (200 OK):
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "accountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "accountName": "Minha Conta",
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "userName": "John Doe",
  "categoryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "categoryName": "Alimentação",
  "categoryColor": "#10B981",
  "categoryIcon": "utensils",
  "amount": 150.50,
  "description": "Supermercado",
  "date": "2026-01-08T17:00:00Z",
  "type": "Expense",
  "createdAt": "2026-01-08T17:00:00Z",
  "updatedAt": "2026-01-08T17:00:00Z"
}
```

**Erros**:
- `404 Not Found`: Transação não encontrada ou sem acesso

---

### POST /api/transactions
Cria uma nova transação.

**Permissões**: Owner ou Editor da conta

**Request Body**:
```json
{
  "accountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "categoryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "amount": 150.50,
  "description": "Supermercado",
  "date": "2026-01-08T17:00:00Z",
  "type": "Expense"
}
```

**Response** (201 Created):
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "accountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "accountName": "Minha Conta",
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "userName": "John Doe",
  "categoryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "categoryName": "Alimentação",
  "categoryColor": "#10B981",
  "categoryIcon": "utensils",
  "amount": 150.50,
  "description": "Supermercado",
  "date": "2026-01-08T17:00:00Z",
  "type": "Expense",
  "createdAt": "2026-01-08T17:00:00Z",
  "updatedAt": "2026-01-08T17:00:00Z"
}
```

**Erros**:
- `400 Bad Request`: Conta ou categoria não encontrada
- `403 Forbidden`: Sem permissão (apenas Owner/Editor)

---

### PUT /api/transactions/{id}
Atualiza uma transação existente.

**Permissões**: Owner ou Editor da conta

**Request Body**:
```json
{
  "categoryId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "amount": 175.00,
  "description": "Supermercado - Atualizado",
  "date": "2026-01-08T17:00:00Z",
  "type": "Expense"
}
```

**Response** (200 OK): Mesma estrutura do GET

**Erros**:
- `400 Bad Request`: Transação ou categoria não encontrada
- `403 Forbidden`: Sem permissão

---

### DELETE /api/transactions/{id}
Exclui uma transação.

**Permissões**: Owner ou Editor da conta

**Response** (204 No Content)

**Erros**:
- `400 Bad Request`: Transação não encontrada
- `403 Forbidden`: Sem permissão

---

## 🏦 Accounts

**Autenticação**: Requerida (Bearer Token)

### GET /api/accounts
Lista todas as contas do usuário (pessoais e compartilhadas).

**Response** (200 OK):
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "Minha Conta",
    "type": "Personal",
    "ownerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "ownerName": "John Doe",
    "createdAt": "2026-01-08T17:00:00Z",
    "members": [
      {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        "userName": "John Doe",
        "userEmail": "john@example.com",
        "role": "Owner",
        "joinedAt": "2026-01-08T17:00:00Z"
      }
    ]
  }
]
```

---

### GET /api/accounts/{id}
Obtém detalhes de uma conta específica.

**Response** (200 OK): Mesma estrutura do item no GET /api/accounts

**Erros**:
- `404 Not Found`: Conta não encontrada ou sem acesso

---

### POST /api/accounts
Cria uma nova conta (pessoal ou compartilhada).

**Request Body**:
```json
{
  "name": "Conta Compartilhada",
  "type": "Shared"
}
```

**Response** (201 Created): Mesma estrutura do GET

**Nota**: Cria automaticamente 11 categorias padrão para a nova conta.

---

### POST /api/accounts/{id}/members
Convida um membro para a conta.

**Permissões**: Apenas Owner

**Request Body**:
```json
{
  "email": "member@example.com",
  "role": "Editor"
}
```

**Roles disponíveis**:
- `Owner`: Controle total (convidar, remover, editar)
- `Editor`: Pode criar/editar/excluir transações e categorias
- `Viewer`: Apenas visualização

**Response** (200 OK): Conta atualizada com novo membro

**Erros**:
- `400 Bad Request`: Usuário não encontrado ou já é membro
- `403 Forbidden`: Apenas Owner pode convidar

---

### DELETE /api/accounts/{id}/members/{memberId}
Remove um membro da conta.

**Permissões**: Apenas Owner

**Response** (204 No Content)

**Erros**:
- `400 Bad Request`: Membro não encontrado ou tentativa de remover Owner
- `403 Forbidden`: Apenas Owner pode remover

---

## 🏷️ Categories

**Autenticação**: Requerida (Bearer Token)

### GET /api/categories?accountId={id}
Lista categorias de uma conta específica.

**Query Parameters**:
- `accountId` (Guid, obrigatório): ID da conta

**Response** (200 OK):
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "accountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "name": "Alimentação",
    "color": "#10B981",
    "icon": "utensils",
    "type": "Expense",
    "createdAt": "2026-01-08T17:00:00Z"
  }
]
```

**Erros**:
- `400 Bad Request`: Conta não encontrada
- `403 Forbidden`: Sem acesso à conta

---

### GET /api/categories/{id}
Obtém uma categoria específica.

**Response** (200 OK): Mesma estrutura do item no GET

**Erros**:
- `404 Not Found`: Categoria não encontrada ou sem acesso

---

### POST /api/categories
Cria uma nova categoria.

**Permissões**: Owner ou Editor da conta

**Request Body**:
```json
{
  "accountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "Transporte",
  "color": "#3B82F6",
  "icon": "car",
  "type": "Expense"
}
```

**Response** (201 Created): Mesma estrutura do GET

**Erros**:
- `400 Bad Request`: Conta não encontrada
- `403 Forbidden`: Sem permissão

---

### PUT /api/categories/{id}
Atualiza uma categoria existente.

**Permissões**: Owner ou Editor da conta

**Request Body**:
```json
{
  "name": "Transporte Atualizado",
  "color": "#3B82F6",
  "icon": "car"
}
```

**Response** (200 OK): Categoria atualizada

**Erros**:
- `400 Bad Request`: Categoria não encontrada
- `403 Forbidden`: Sem permissão

---

### DELETE /api/categories/{id}
Exclui uma categoria.

**Permissões**: Owner ou Editor da conta

**Response** (204 No Content)

**Erros**:
- `400 Bad Request`: Categoria não encontrada ou possui transações vinculadas
- `403 Forbidden`: Sem permissão

**Nota**: Não é possível excluir categorias que possuem transações vinculadas.

---

## 🔒 Autenticação JWT

### Header Format
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Token Claims
- `sub`: User ID (Guid)
- `email`: User email
- `name`: User name
- `jti`: Token ID (Guid)
- `exp`: Expiration timestamp

### Token Expiration
- **Padrão**: 7 dias (configurável em appsettings.json)

---

## 📊 Códigos de Status HTTP

- `200 OK`: Requisição bem-sucedida
- `201 Created`: Recurso criado com sucesso
- `204 No Content`: Operação bem-sucedida sem conteúdo de retorno
- `400 Bad Request`: Dados inválidos ou regra de negócio violada
- `401 Unauthorized`: Token ausente ou inválido
- `403 Forbidden`: Sem permissão para a operação
- `404 Not Found`: Recurso não encontrado
- `500 Internal Server Error`: Erro no servidor

---

## 🎯 Categorias Padrão

Ao criar uma conta (registro ou nova conta), 11 categorias são criadas automaticamente:

**Despesas** (7):
1. Alimentação (#10B981, utensils)
2. Transporte (#3B82F6, car)
3. Moradia (#8B5CF6, home)
4. Saúde (#EF4444, heart)
5. Lazer (#F59E0B, gamepad)
6. Educação (#06B6D4, book)
7. Outros (#6B7280, tag)

**Receitas** (4):
1. Salário (#10B981, dollar-sign)
2. Freelance (#3B82F6, briefcase)
3. Investimentos (#8B5CF6, trending-up)
4. Outros (#6B7280, tag)

---

## 🧪 Testando a API

### Swagger UI
Acesse: `http://localhost:5000/swagger`

### Exemplo de Fluxo Completo

1. **Registrar usuário**:
   ```bash
   POST /api/auth/register
   ```

2. **Fazer login** (obter token):
   ```bash
   POST /api/auth/login
   ```

3. **Listar contas** (usar token):
   ```bash
   GET /api/accounts
   Authorization: Bearer {token}
   ```

4. **Criar transação**:
   ```bash
   POST /api/transactions
   Authorization: Bearer {token}
   ```

5. **Listar transações com filtros**:
   ```bash
   GET /api/transactions?type=Expense&page=1&pageSize=10
   Authorization: Bearer {token}
   ```

---

**Última Atualização**: 08/01/2026 - 14:25  
**Versão da API**: v1.0  
**Total de Endpoints**: 16
