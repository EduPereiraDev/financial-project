# 🚀 Deploy para Supabase - Guia Completo

## 📋 Visão Geral

Este guia explica como aplicar as migrações do Entity Framework Core no banco de dados PostgreSQL do Supabase em produção.

---

## 🗄️ Migrações Existentes

O projeto possui **7 migrações** que precisam ser aplicadas no Supabase:

1. ✅ **InitialCreate** (20260108171315)
   - Users, Accounts, AccountMembers, Categories, Transactions

2. ✅ **AddRecurringTransactions** (20260109020346)
   - RecurringTransactions, RecurringTransactionExecutions

3. ✅ **AddInvitations** (20260109022655)
   - Invitations

4. ✅ **AddAlertsAndNotifications** (20260109024953)
   - Alerts, Notifications

5. ✅ **AddBankingIntegration** (20260109032803)
   - BankConnections, BankAccounts, BankTransactions

6. ✅ **AddBudgets** (20260109162720)
   - Budgets

7. ✅ **AddGoals** (20260109163746)
   - Goals

---

## 🔧 Opção 1: Gerar Script SQL e Aplicar no Supabase (RECOMENDADO)

### Passo 1: Gerar Script SQL Completo

```bash
cd backend/FinancialControl.Api

# Gerar script SQL de todas as migrações
dotnet ef migrations script --output ../../supabase-migration.sql --idempotent
```

O parâmetro `--idempotent` garante que o script pode ser executado múltiplas vezes sem erros.

### Passo 2: Revisar o Script SQL

Abra o arquivo `supabase-migration.sql` e revise:
- ✅ Todas as tabelas estão sendo criadas
- ✅ Todos os índices estão corretos
- ✅ Foreign keys estão configuradas
- ✅ Tipos de dados estão compatíveis com PostgreSQL

### Passo 3: Conectar ao Supabase

1. Acesse o [Supabase Dashboard](https://app.supabase.com)
2. Selecione seu projeto
3. Vá em **SQL Editor** no menu lateral
4. Clique em **New Query**

### Passo 4: Executar o Script

1. Copie todo o conteúdo de `supabase-migration.sql`
2. Cole no SQL Editor do Supabase
3. Clique em **Run** ou pressione `Ctrl+Enter`
4. Aguarde a execução (pode levar alguns segundos)
5. Verifique se não há erros

### Passo 5: Verificar Tabelas Criadas

Execute no SQL Editor:

```sql
-- Listar todas as tabelas
SELECT table_name 
FROM information_schema.tables 
WHERE table_schema = 'public' 
ORDER BY table_name;

-- Verificar tabela __EFMigrationsHistory
SELECT * FROM "__EFMigrationsHistory" ORDER BY "MigrationId";
```

Você deve ver 15 tabelas:
- Users
- Accounts
- AccountMembers
- Categories
- Transactions
- RecurringTransactions
- RecurringTransactionExecutions
- Invitations
- Alerts
- Notifications
- BankConnections
- BankAccounts
- BankTransactions
- Budgets
- Goals

---

## 🔧 Opção 2: Aplicar Migrações Diretamente (Alternativa)

### Passo 1: Configurar Connection String

Obtenha a connection string do Supabase:

1. Acesse **Settings** → **Database**
2. Copie a **Connection String** (modo Pooler ou Direct)
3. Substitua `[YOUR-PASSWORD]` pela senha do banco

Exemplo:
```
postgresql://postgres.[PROJECT-REF]:[PASSWORD]@aws-0-us-east-1.pooler.supabase.com:6543/postgres
```

### Passo 2: Atualizar appsettings.Production.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=aws-0-us-east-1.pooler.supabase.com;Port=6543;Database=postgres;Username=postgres.[PROJECT-REF];Password=[YOUR-PASSWORD];SSL Mode=Require"
  }
}
```

### Passo 3: Aplicar Migrações

```bash
cd backend/FinancialControl.Api

# Aplicar todas as migrações
dotnet ef database update --connection "Host=aws-0-us-east-1.pooler.supabase.com;Port=6543;Database=postgres;Username=postgres.[PROJECT-REF];Password=[YOUR-PASSWORD];SSL Mode=Require"
```

---

## 🔒 Opção 3: Usar Supabase CLI (Mais Seguro)

### Passo 1: Instalar Supabase CLI

```bash
# macOS
brew install supabase/tap/supabase

# Windows
scoop bucket add supabase https://github.com/supabase/scoop-bucket.git
scoop install supabase

# Linux
brew install supabase/tap/supabase
```

### Passo 2: Login no Supabase

```bash
supabase login
```

### Passo 3: Link com Projeto

```bash
supabase link --project-ref [YOUR-PROJECT-REF]
```

### Passo 4: Executar Migration

```bash
supabase db push --db-url "postgresql://postgres.[PROJECT-REF]:[PASSWORD]@aws-0-us-east-1.pooler.supabase.com:6543/postgres"
```

---

## 📊 Estrutura Final do Banco

Após aplicar todas as migrações, você terá:

### Tabelas Principais (5)
- **Users**: Usuários do sistema
- **Accounts**: Contas financeiras
- **AccountMembers**: Membros de contas compartilhadas
- **Categories**: Categorias de transações
- **Transactions**: Transações financeiras

### Tabelas de Funcionalidades (10)
- **RecurringTransactions**: Transações recorrentes
- **RecurringTransactionExecutions**: Execuções de recorrências
- **Invitations**: Convites para contas compartilhadas
- **Alerts**: Alertas financeiros
- **Notifications**: Notificações do sistema
- **BankConnections**: Conexões bancárias (Pluggy)
- **BankAccounts**: Contas bancárias sincronizadas
- **BankTransactions**: Transações bancárias importadas
- **Budgets**: Orçamentos
- **Goals**: Metas financeiras

### Tabela de Controle (1)
- **__EFMigrationsHistory**: Histórico de migrações aplicadas

---

## ✅ Verificação Pós-Deploy

Execute estas queries no SQL Editor do Supabase:

### 1. Verificar Todas as Tabelas

```sql
SELECT 
    schemaname,
    tablename,
    tableowner
FROM pg_tables
WHERE schemaname = 'public'
ORDER BY tablename;
```

### 2. Verificar Migrações Aplicadas

```sql
SELECT 
    "MigrationId",
    "ProductVersion"
FROM "__EFMigrationsHistory"
ORDER BY "MigrationId";
```

Deve retornar 7 registros (uma para cada migração).

### 3. Verificar Foreign Keys

```sql
SELECT
    tc.table_name,
    kcu.column_name,
    ccu.table_name AS foreign_table_name,
    ccu.column_name AS foreign_column_name
FROM information_schema.table_constraints AS tc
JOIN information_schema.key_column_usage AS kcu
    ON tc.constraint_name = kcu.constraint_name
JOIN information_schema.constraint_column_usage AS ccu
    ON ccu.constraint_name = tc.constraint_name
WHERE tc.constraint_type = 'FOREIGN KEY'
ORDER BY tc.table_name;
```

### 4. Verificar Índices

```sql
SELECT
    tablename,
    indexname,
    indexdef
FROM pg_indexes
WHERE schemaname = 'public'
ORDER BY tablename, indexname;
```

### 5. Contar Registros (Deve estar vazio inicialmente)

```sql
SELECT 
    'Users' as table_name, COUNT(*) as count FROM "Users"
UNION ALL
SELECT 'Accounts', COUNT(*) FROM "Accounts"
UNION ALL
SELECT 'Transactions', COUNT(*) FROM "Transactions"
UNION ALL
SELECT 'Budgets', COUNT(*) FROM "Budgets"
UNION ALL
SELECT 'Goals', COUNT(*) FROM "Goals";
```

---

## 🔐 Segurança

### Habilitar Row Level Security (RLS)

Após aplicar as migrações, habilite RLS nas tabelas:

```sql
-- Habilitar RLS em todas as tabelas
ALTER TABLE "Users" ENABLE ROW LEVEL SECURITY;
ALTER TABLE "Accounts" ENABLE ROW LEVEL SECURITY;
ALTER TABLE "AccountMembers" ENABLE ROW LEVEL SECURITY;
ALTER TABLE "Categories" ENABLE ROW LEVEL SECURITY;
ALTER TABLE "Transactions" ENABLE ROW LEVEL SECURITY;
ALTER TABLE "RecurringTransactions" ENABLE ROW LEVEL SECURITY;
ALTER TABLE "Invitations" ENABLE ROW LEVEL SECURITY;
ALTER TABLE "Alerts" ENABLE ROW LEVEL SECURITY;
ALTER TABLE "Notifications" ENABLE ROW LEVEL SECURITY;
ALTER TABLE "BankConnections" ENABLE ROW LEVEL SECURITY;
ALTER TABLE "BankAccounts" ENABLE ROW LEVEL SECURITY;
ALTER TABLE "BankTransactions" ENABLE ROW LEVEL SECURITY;
ALTER TABLE "Budgets" ENABLE ROW LEVEL SECURITY;
ALTER TABLE "Goals" ENABLE ROW LEVEL SECURITY;

-- Criar políticas básicas (exemplo para Users)
CREATE POLICY "Users can view own data"
ON "Users"
FOR SELECT
USING (auth.uid()::text = "Id"::text);

CREATE POLICY "Users can update own data"
ON "Users"
FOR UPDATE
USING (auth.uid()::text = "Id"::text);
```

**⚠️ IMPORTANTE**: Ajuste as políticas RLS conforme suas necessidades de segurança.

---

## 🔄 Atualizar Connection String no Backend

### appsettings.Production.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Host=aws-0-us-east-1.pooler.supabase.com;Port=6543;Database=postgres;Username=postgres.[PROJECT-REF];Password=[YOUR-PASSWORD];SSL Mode=Require;Trust Server Certificate=true"
  },
  "Jwt": {
    "Key": "[YOUR-JWT-SECRET-KEY]",
    "Issuer": "FinancialControl",
    "Audience": "FinancialControlUsers",
    "ExpirationInMinutes": 1440
  },
  "AllowedHosts": "*"
}
```

### Variáveis de Ambiente (Recomendado)

Em vez de hardcoded, use variáveis de ambiente:

```bash
# .env ou Railway/Vercel Environment Variables
DATABASE_URL=postgresql://postgres.[PROJECT-REF]:[PASSWORD]@aws-0-us-east-1.pooler.supabase.com:6543/postgres
JWT_KEY=[YOUR-JWT-SECRET-KEY]
```

---

## 📝 Checklist de Deploy

- [ ] Gerar script SQL das migrações
- [ ] Revisar script SQL
- [ ] Fazer backup do banco (se existir dados)
- [ ] Aplicar migrações no Supabase
- [ ] Verificar todas as tabelas criadas
- [ ] Verificar __EFMigrationsHistory
- [ ] Verificar foreign keys
- [ ] Verificar índices
- [ ] Habilitar RLS (Row Level Security)
- [ ] Criar políticas de segurança
- [ ] Atualizar connection string no backend
- [ ] Testar conexão do backend com Supabase
- [ ] Testar CRUD básico
- [ ] Monitorar logs de erro

---

## 🐛 Troubleshooting

### Erro: "relation already exists"

Se você já executou parte das migrações:

```sql
-- Verificar quais migrações foram aplicadas
SELECT * FROM "__EFMigrationsHistory";

-- Aplicar apenas migrações faltantes
dotnet ef migrations script [LAST-APPLIED-MIGRATION] --output remaining.sql
```

### Erro: "SSL connection required"

Adicione `SSL Mode=Require` na connection string:

```
Host=...;SSL Mode=Require;Trust Server Certificate=true
```

### Erro: "password authentication failed"

1. Verifique a senha no Supabase Dashboard
2. Resete a senha se necessário: **Settings** → **Database** → **Reset database password**

### Erro: "too many connections"

Use o **Pooler** do Supabase em vez da conexão direta:
- Pooler: `Port=6543`
- Direct: `Port=5432`

---

## 📚 Recursos

- [Supabase Documentation](https://supabase.com/docs)
- [Entity Framework Core Migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)
- [Supabase CLI](https://supabase.com/docs/guides/cli)

---

## 🎯 Próximos Passos

Após aplicar as migrações:

1. **Deploy do Backend** (Railway/Heroku/Azure)
2. **Deploy do Frontend** (Vercel/Netlify)
3. **Configurar CORS** no backend
4. **Configurar variáveis de ambiente**
5. **Testar integração completa**
6. **Configurar CI/CD** para deploys automáticos

---

**Última Atualização**: 09/01/2026 - 15:55
