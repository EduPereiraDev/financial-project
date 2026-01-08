# Database Migrations

Este diretório contém as migrations do Entity Framework Core para o banco de dados PostgreSQL.

## 📋 Migrations Criadas

### InitialCreate (20260108171315)
**Data**: 08/01/2026 - 14:13  
**Status**: ✅ Criada

#### Tabelas Criadas:
1. **Users**
   - Id (uuid, PK)
   - Email (varchar(255), unique index)
   - PasswordHash (text)
   - Name (varchar(255))
   - CreatedAt (timestamp)
   - UpdatedAt (timestamp)

2. **Accounts**
   - Id (uuid, PK)
   - Name (varchar(255))
   - Type (text) - enum: Personal/Shared
   - OwnerId (uuid, FK → Users)
   - CreatedAt (timestamp)
   - Index: OwnerId

3. **AccountMembers**
   - Id (uuid, PK)
   - AccountId (uuid, FK → Accounts, CASCADE)
   - UserId (uuid, FK → Users, CASCADE)
   - Role (text) - enum: Owner/Editor/Viewer
   - JoinedAt (timestamp)
   - Unique Index: (AccountId, UserId)
   - Index: UserId

4. **Categories**
   - Id (uuid, PK)
   - AccountId (uuid, FK → Accounts, CASCADE)
   - Name (varchar(100))
   - Color (varchar(7))
   - Icon (varchar(50))
   - Type (text) - enum: Income/Expense
   - CreatedAt (timestamp)
   - Index: AccountId

5. **Transactions**
   - Id (uuid, PK)
   - AccountId (uuid, FK → Accounts, CASCADE)
   - UserId (uuid, FK → Users, RESTRICT)
   - CategoryId (uuid, FK → Categories, RESTRICT)
   - Amount (numeric(18,2))
   - Description (varchar(500))
   - Date (timestamp)
   - Type (text) - enum: Income/Expense
   - CreatedAt (timestamp)
   - UpdatedAt (timestamp)
   - Index: AccountId
   - Index: CategoryId
   - Index: Date
   - Index: UserId

#### Índices Criados:
- ✅ Users.Email (unique)
- ✅ AccountMembers.(AccountId, UserId) (unique)
- ✅ Accounts.OwnerId
- ✅ Categories.AccountId
- ✅ Transactions.AccountId
- ✅ Transactions.CategoryId
- ✅ Transactions.Date
- ✅ Transactions.UserId

#### Foreign Keys:
- ✅ Accounts.OwnerId → Users.Id (RESTRICT)
- ✅ AccountMembers.AccountId → Accounts.Id (CASCADE)
- ✅ AccountMembers.UserId → Users.Id (CASCADE)
- ✅ Categories.AccountId → Accounts.Id (CASCADE)
- ✅ Transactions.AccountId → Accounts.Id (CASCADE)
- ✅ Transactions.UserId → Users.Id (RESTRICT)
- ✅ Transactions.CategoryId → Categories.Id (RESTRICT)

---

## 🚀 Como Aplicar as Migrations

### 1. Subir PostgreSQL (Docker)
```bash
docker run --name financial-postgres \
  -e POSTGRES_PASSWORD=postgres123 \
  -e POSTGRES_DB=financialcontrol \
  -p 5432:5432 \
  -d postgres:15-alpine
```

### 2. Aplicar Migrations
```bash
dotnet ef database update
```

### 3. Verificar Banco
```bash
# Conectar ao PostgreSQL
docker exec -it financial-postgres psql -U postgres -d financialcontrol

# Listar tabelas
\dt

# Ver estrutura de uma tabela
\d "Users"
```

---

## 🔄 Comandos Úteis

### Criar Nova Migration
```bash
dotnet ef migrations add NomeDaMigration
```

### Remover Última Migration (não aplicada)
```bash
dotnet ef migrations remove
```

### Reverter Migration
```bash
dotnet ef database update NomeMigrationAnterior
```

### Gerar Script SQL
```bash
dotnet ef migrations script > migration.sql
```

### Ver Migrations Aplicadas
```bash
dotnet ef migrations list
```

---

## ⚠️ Importante

- **Sempre faça backup** antes de aplicar migrations em produção
- **Teste migrations** em ambiente de desenvolvimento primeiro
- **Revise o código SQL** gerado antes de aplicar
- **Use transactions** para migrations complexas
- **Documente** mudanças significativas no schema

---

## 📊 Status Atual

```
✅ Migrations criadas
⏳ Aguardando PostgreSQL para aplicar
📋 Próximo: Aplicar migrations no banco local
```

**Última Atualização**: 08/01/2026 - 14:15
