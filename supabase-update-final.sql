-- ============================================
-- SCRIPT DE ATUALIZAÇÃO COMPLETO - SUPABASE
-- Adiciona as 10 tabelas que faltam
-- ============================================

-- Verificar tabela de controle de migrações
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

-- ============================================
-- 1. RECURRING TRANSACTIONS (Transações Recorrentes)
-- ============================================
CREATE TABLE IF NOT EXISTS "RecurringTransactions" (
    "Id" uuid NOT NULL,
    "AccountId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "CategoryId" uuid NOT NULL,
    "Amount" numeric(18,2) NOT NULL,
    "Description" character varying(500) NOT NULL,
    "Type" text NOT NULL,
    "Frequency" text NOT NULL,
    "StartDate" timestamp with time zone NOT NULL,
    "EndDate" timestamp with time zone,
    "IsActive" boolean NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_RecurringTransactions" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_RecurringTransactions_Accounts_AccountId" FOREIGN KEY ("AccountId") REFERENCES "Accounts" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_RecurringTransactions_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_RecurringTransactions_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE RESTRICT
);

CREATE INDEX IF NOT EXISTS "IX_RecurringTransactions_AccountId" ON "RecurringTransactions" ("AccountId");
CREATE INDEX IF NOT EXISTS "IX_RecurringTransactions_CategoryId" ON "RecurringTransactions" ("CategoryId");
CREATE INDEX IF NOT EXISTS "IX_RecurringTransactions_UserId" ON "RecurringTransactions" ("UserId");

-- ============================================
-- 2. RECURRING TRANSACTION EXECUTIONS (Execuções)
-- ============================================
CREATE TABLE IF NOT EXISTS "RecurringTransactionExecutions" (
    "Id" uuid NOT NULL,
    "RecurringTransactionId" uuid NOT NULL,
    "TransactionId" uuid,
    "ScheduledDate" timestamp with time zone NOT NULL,
    "ExecutedDate" timestamp with time zone,
    "Status" text NOT NULL,
    "ErrorMessage" text,
    "CreatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_RecurringTransactionExecutions" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_RecurringTransactionExecutions_RecurringTransactions_Recu~" FOREIGN KEY ("RecurringTransactionId") REFERENCES "RecurringTransactions" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_RecurringTransactionExecutions_Transactions_TransactionId" FOREIGN KEY ("TransactionId") REFERENCES "Transactions" ("Id")
);

CREATE INDEX IF NOT EXISTS "IX_RecurringTransactionExecutions_RecurringTransactionId" ON "RecurringTransactionExecutions" ("RecurringTransactionId");
CREATE INDEX IF NOT EXISTS "IX_RecurringTransactionExecutions_TransactionId" ON "RecurringTransactionExecutions" ("TransactionId");

-- ============================================
-- 3. INVITATIONS (Convites)
-- ============================================
CREATE TABLE IF NOT EXISTS "Invitations" (
    "Id" uuid NOT NULL,
    "AccountId" uuid NOT NULL,
    "InviterId" uuid NOT NULL,
    "Email" character varying(255) NOT NULL,
    "Role" text NOT NULL,
    "Token" character varying(500) NOT NULL,
    "Status" text NOT NULL,
    "ExpiresAt" timestamp with time zone NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "AcceptedAt" timestamp with time zone,
    CONSTRAINT "PK_Invitations" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Invitations_Accounts_AccountId" FOREIGN KEY ("AccountId") REFERENCES "Accounts" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Invitations_Users_InviterId" FOREIGN KEY ("InviterId") REFERENCES "Users" ("Id") ON DELETE RESTRICT
);

CREATE INDEX IF NOT EXISTS "IX_Invitations_AccountId" ON "Invitations" ("AccountId");
CREATE INDEX IF NOT EXISTS "IX_Invitations_InviterId" ON "Invitations" ("InviterId");
CREATE INDEX IF NOT EXISTS "IX_Invitations_Token" ON "Invitations" ("Token");

-- ============================================
-- 4. ALERTS (Alertas)
-- ============================================
CREATE TABLE IF NOT EXISTS "Alerts" (
    "Id" uuid NOT NULL,
    "AccountId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "Type" text NOT NULL,
    "Threshold" numeric(18,2) NOT NULL,
    "CategoryId" uuid,
    "IsActive" boolean NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Alerts" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Alerts_Accounts_AccountId" FOREIGN KEY ("AccountId") REFERENCES "Accounts" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Alerts_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id"),
    CONSTRAINT "FK_Alerts_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE RESTRICT
);

CREATE INDEX IF NOT EXISTS "IX_Alerts_AccountId" ON "Alerts" ("AccountId");
CREATE INDEX IF NOT EXISTS "IX_Alerts_CategoryId" ON "Alerts" ("CategoryId");
CREATE INDEX IF NOT EXISTS "IX_Alerts_UserId" ON "Alerts" ("UserId");

-- ============================================
-- 5. NOTIFICATIONS (Notificações)
-- ============================================
CREATE TABLE IF NOT EXISTS "Notifications" (
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "Type" text NOT NULL,
    "Title" character varying(255) NOT NULL,
    "Message" text NOT NULL,
    "IsRead" boolean NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "ReadAt" timestamp with time zone,
    CONSTRAINT "PK_Notifications" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Notifications_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "IX_Notifications_UserId" ON "Notifications" ("UserId");

-- ============================================
-- 6. BANK CONNECTIONS (Conexões Bancárias - Pluggy)
-- ============================================
CREATE TABLE IF NOT EXISTS "BankConnections" (
    "Id" uuid NOT NULL,
    "AccountId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "PluggyItemId" character varying(255) NOT NULL,
    "InstitutionName" character varying(255) NOT NULL,
    "Status" text NOT NULL,
    "LastSyncAt" timestamp with time zone,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_BankConnections" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_BankConnections_Accounts_AccountId" FOREIGN KEY ("AccountId") REFERENCES "Accounts" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_BankConnections_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE RESTRICT
);

CREATE INDEX IF NOT EXISTS "IX_BankConnections_AccountId" ON "BankConnections" ("AccountId");
CREATE INDEX IF NOT EXISTS "IX_BankConnections_UserId" ON "BankConnections" ("UserId");

-- ============================================
-- 7. BANK ACCOUNTS (Contas Bancárias)
-- ============================================
CREATE TABLE IF NOT EXISTS "BankAccounts" (
    "Id" uuid NOT NULL,
    "BankConnectionId" uuid NOT NULL,
    "PluggyAccountId" character varying(255) NOT NULL,
    "Name" character varying(255) NOT NULL,
    "Type" character varying(100) NOT NULL,
    "Balance" numeric(18,2) NOT NULL,
    "Currency" character varying(10) NOT NULL,
    "LastSyncAt" timestamp with time zone,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_BankAccounts" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_BankAccounts_BankConnections_BankConnectionId" FOREIGN KEY ("BankConnectionId") REFERENCES "BankConnections" ("Id") ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS "IX_BankAccounts_BankConnectionId" ON "BankAccounts" ("BankConnectionId");

-- ============================================
-- 8. BANK TRANSACTIONS (Transações Bancárias)
-- ============================================
CREATE TABLE IF NOT EXISTS "BankTransactions" (
    "Id" uuid NOT NULL,
    "BankAccountId" uuid NOT NULL,
    "PluggyTransactionId" character varying(255) NOT NULL,
    "Description" character varying(500) NOT NULL,
    "Amount" numeric(18,2) NOT NULL,
    "Date" timestamp with time zone NOT NULL,
    "Type" text NOT NULL,
    "Category" character varying(100),
    "IsImported" boolean NOT NULL,
    "TransactionId" uuid,
    "CreatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_BankTransactions" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_BankTransactions_BankAccounts_BankAccountId" FOREIGN KEY ("BankAccountId") REFERENCES "BankAccounts" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_BankTransactions_Transactions_TransactionId" FOREIGN KEY ("TransactionId") REFERENCES "Transactions" ("Id")
);

CREATE INDEX IF NOT EXISTS "IX_BankTransactions_BankAccountId" ON "BankTransactions" ("BankAccountId");
CREATE INDEX IF NOT EXISTS "IX_BankTransactions_TransactionId" ON "BankTransactions" ("TransactionId");

-- ============================================
-- 9. BUDGETS (Orçamentos)
-- ============================================
CREATE TABLE IF NOT EXISTS "Budgets" (
    "Id" uuid NOT NULL,
    "AccountId" uuid NOT NULL,
    "CategoryId" uuid NOT NULL,
    "Amount" numeric(18,2) NOT NULL,
    "Period" text NOT NULL,
    "StartDate" timestamp with time zone NOT NULL,
    "EndDate" timestamp with time zone NOT NULL,
    "IsActive" boolean NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Budgets" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Budgets_Accounts_AccountId" FOREIGN KEY ("AccountId") REFERENCES "Accounts" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Budgets_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id") ON DELETE RESTRICT
);

CREATE INDEX IF NOT EXISTS "IX_Budgets_AccountId" ON "Budgets" ("AccountId");
CREATE INDEX IF NOT EXISTS "IX_Budgets_CategoryId" ON "Budgets" ("CategoryId");

-- ============================================
-- 10. GOALS (Metas Financeiras)
-- ============================================
CREATE TABLE IF NOT EXISTS "Goals" (
    "Id" uuid NOT NULL,
    "AccountId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "Name" character varying(255) NOT NULL,
    "Description" text,
    "TargetAmount" numeric(18,2) NOT NULL,
    "CurrentAmount" numeric(18,2) NOT NULL,
    "TargetDate" timestamp with time zone NOT NULL,
    "Status" text NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Goals" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Goals_Accounts_AccountId" FOREIGN KEY ("AccountId") REFERENCES "Accounts" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Goals_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE RESTRICT
);

CREATE INDEX IF NOT EXISTS "IX_Goals_AccountId" ON "Goals" ("AccountId");
CREATE INDEX IF NOT EXISTS "IX_Goals_UserId" ON "Goals" ("UserId");

-- ============================================
-- REGISTRAR MIGRAÇÕES
-- ============================================
INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260108171315_InitialCreate', '9.0.0')
ON CONFLICT ("MigrationId") DO NOTHING;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260109020346_AddRecurringTransactions', '9.0.0')
ON CONFLICT ("MigrationId") DO NOTHING;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260109022655_AddInvitations', '9.0.0')
ON CONFLICT ("MigrationId") DO NOTHING;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260109024953_AddAlertsAndNotifications', '9.0.0')
ON CONFLICT ("MigrationId") DO NOTHING;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260109032803_AddBankingIntegration', '9.0.0')
ON CONFLICT ("MigrationId") DO NOTHING;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260109162720_AddBudgets', '9.0.0')
ON CONFLICT ("MigrationId") DO NOTHING;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260109163746_AddGoals', '9.0.0')
ON CONFLICT ("MigrationId") DO NOTHING;

-- ============================================
-- VERIFICAÇÃO FINAL
-- ============================================
-- Execute para verificar todas as tabelas:
-- SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' ORDER BY table_name;

-- Execute para verificar migrações:
-- SELECT * FROM "__EFMigrationsHistory" ORDER BY "MigrationId";
