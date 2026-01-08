# Financial Control API - Backend

API REST para controle financeiro pessoal e compartilhado.

## 🚀 Tecnologias

- .NET 9.0
- Entity Framework Core 9.0
- PostgreSQL 15
- JWT Authentication
- BCrypt para hash de senhas
- Serilog para logging
- Swagger/OpenAPI

## 📋 Pré-requisitos

- .NET 9 SDK
- PostgreSQL 15+
- Docker (opcional)

## 🔧 Configuração

### 1. Database

```bash
# Com Docker
docker run --name financial-postgres -e POSTGRES_PASSWORD=postgres123 -e POSTGRES_DB=financialcontrol -p 5432:5432 -d postgres:15-alpine

# Ou instale PostgreSQL localmente
```

### 2. Configurar appsettings

Copie `.env.example` e ajuste as variáveis conforme necessário.

### 3. Migrations

```bash
# Criar migration inicial
dotnet ef migrations add InitialCreate

# Aplicar migrations
dotnet ef database update
```

### 4. Executar

```bash
dotnet run
```

API estará disponível em: `http://localhost:5000`
Swagger UI: `http://localhost:5000/swagger`

## 📚 Endpoints

### Auth
- `POST /api/auth/register` - Registrar novo usuário
- `POST /api/auth/login` - Login

### Accounts (em desenvolvimento)
- `GET /api/accounts` - Listar contas do usuário
- `POST /api/accounts` - Criar conta compartilhada
- `POST /api/accounts/{id}/members` - Convidar membro

### Transactions (em desenvolvimento)
- `GET /api/transactions` - Listar transações
- `POST /api/transactions` - Criar transação
- `PUT /api/transactions/{id}` - Atualizar transação
- `DELETE /api/transactions/{id}` - Deletar transação

### Categories (em desenvolvimento)
- `GET /api/categories` - Listar categorias
- `POST /api/categories` - Criar categoria
- `PUT /api/categories/{id}` - Atualizar categoria
- `DELETE /api/categories/{id}` - Deletar categoria

## 🏗️ Estrutura do Projeto

```
FinancialControl.Api/
├── Controllers/        # API Controllers
├── Data/              # DbContext e Migrations
├── DTOs/              # Data Transfer Objects
├── Models/            # Entidades do domínio
├── Services/          # Lógica de negócio
├── appsettings.json   # Configurações
└── Program.cs         # Entry point
```

## 🔐 Autenticação

A API usa JWT Bearer tokens. Após login/registro, inclua o token no header:

```
Authorization: Bearer {seu-token}
```

## 📝 Modelos de Dados

### User
- Email único
- Senha com hash BCrypt
- Nome

### Account
- Personal (conta pessoal)
- Shared (conta compartilhada)
- Owner e Members

### Transaction
- Vinculada a uma Account
- Categoria
- Tipo (Income/Expense)
- Valor, descrição, data

### Category
- Nome, cor, ícone
- Tipo (Income/Expense)
- Vinculada a uma Account

## 🧪 Testes

```bash
# Executar testes (quando implementados)
dotnet test
```

## 📦 Deploy

Ver `docs/ADR/002-hospedagem-gratuita.md` para instruções de deploy no Railway.

## 🤝 Contribuindo

1. Fork o projeto
2. Crie uma branch (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto é privado e de uso pessoal.
