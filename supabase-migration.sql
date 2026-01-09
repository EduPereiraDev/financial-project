CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260108171315_InitialCreate') THEN
    CREATE TABLE "Users" (
        "Id" uuid NOT NULL,
        "Email" character varying(255) NOT NULL,
        "PasswordHash" text NOT NULL,
        "Name" character varying(255) NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260108171315_InitialCreate') THEN
    CREATE TABLE "Accounts" (
        "Id" uuid NOT NULL,
        "Name" character varying(255) NOT NULL,
        "Type" text NOT NULL,
        "OwnerId" uuid NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_Accounts" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Accounts_Users_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES "Users" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260108171315_InitialCreate') THEN
    CREATE TABLE "AccountMembers" (
        "Id" uuid NOT NULL,
        "AccountId" uuid NOT NULL,
        "UserId" uuid NOT NULL,
        "Role" text NOT NULL,
        "JoinedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_AccountMembers" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_AccountMembers_Accounts_AccountId" FOREIGN KEY ("AccountId") REFERENCES "Accounts" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_AccountMembers_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260108171315_InitialCreate') THEN
    CREATE TABLE "Categories" (
        "Id" uuid NOT NULL,
        "AccountId" uuid NOT NULL,
        "Name" character varying(100) NOT NULL,
        "Color" character varying(7) NOT NULL,
        "Icon" character varying(50) NOT NULL,
        "Type" text NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_Categories" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Categories_Accounts_AccountId" FOREIGN KEY ("AccountId") REFERENCES "Accounts" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260108171315_InitialCreate') THEN
    CREATE TABLE "Transactions" (
        "Id" uuid NOT NULL,
        "AccountId" uuid NOT NULL,
        "UserId" uuid NOT NULL,
        "CategoryId" uuid NOT NULL,
        "Amount" numeric(18,2) NOT NULL,
        "Description" character varying(500) NOT NULL,
        "Date" timestamp with time zone NOT NULL,
        "Type" text NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_Transactions" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Transactions_Accounts_AccountId" FOREIGN KEY ("AccountId") REFERENCES "Accounts" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_Transactions_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_Transactions_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260108171315_InitialCreate') THEN
    CREATE UNIQUE INDEX "IX_AccountMembers_AccountId_UserId" ON "AccountMembers" ("AccountId", "UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260108171315_InitialCreate') THEN
    CREATE INDEX "IX_AccountMembers_UserId" ON "AccountMembers" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260108171315_InitialCreate') THEN
    CREATE INDEX "IX_Accounts_OwnerId" ON "Accounts" ("OwnerId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260108171315_InitialCreate') THEN
    CREATE INDEX "IX_Categories_AccountId" ON "Categories" ("AccountId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260108171315_InitialCreate') THEN
    CREATE INDEX "IX_Transactions_AccountId" ON "Transactions" ("AccountId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260108171315_InitialCreate') THEN
    CREATE INDEX "IX_Transactions_CategoryId" ON "Transactions" ("CategoryId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260108171315_InitialCreate') THEN
    CREATE INDEX "IX_Transactions_Date" ON "Transactions" ("Date");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260108171315_InitialCreate') THEN
    CREATE INDEX "IX_Transactions_UserId" ON "Transactions" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260108171315_InitialCreate') THEN
    CREATE UNIQUE INDEX "IX_Users_Email" ON "Users" ("Email");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260108171315_InitialCreate') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260108171315_InitialCreate', '9.0.0');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109020346_AddRecurringTransactions') THEN
    CREATE TABLE "RecurringTransactions" (
        "Id" uuid NOT NULL,
        "AccountId" uuid NOT NULL,
        "CategoryId" uuid NOT NULL,
        "Description" character varying(500) NOT NULL,
        "Amount" numeric(18,2) NOT NULL,
        "Type" text NOT NULL,
        "Frequency" text NOT NULL,
        "DayOfMonth" integer NOT NULL,
        "StartDate" timestamp with time zone NOT NULL,
        "EndDate" timestamp with time zone,
        "IsActive" boolean NOT NULL,
        "LastExecutionDate" timestamp with time zone,
        "NextExecutionDate" timestamp with time zone,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_RecurringTransactions" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_RecurringTransactions_Accounts_AccountId" FOREIGN KEY ("AccountId") REFERENCES "Accounts" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_RecurringTransactions_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109020346_AddRecurringTransactions') THEN
    CREATE INDEX "IX_RecurringTransactions_AccountId" ON "RecurringTransactions" ("AccountId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109020346_AddRecurringTransactions') THEN
    CREATE INDEX "IX_RecurringTransactions_CategoryId" ON "RecurringTransactions" ("CategoryId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109020346_AddRecurringTransactions') THEN
    CREATE INDEX "IX_RecurringTransactions_IsActive" ON "RecurringTransactions" ("IsActive");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109020346_AddRecurringTransactions') THEN
    CREATE INDEX "IX_RecurringTransactions_NextExecutionDate" ON "RecurringTransactions" ("NextExecutionDate");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109020346_AddRecurringTransactions') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260109020346_AddRecurringTransactions', '9.0.0');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109022655_AddInvitations') THEN
    CREATE TABLE "Invitations" (
        "Id" uuid NOT NULL,
        "AccountId" uuid NOT NULL,
        "InvitedByUserId" uuid NOT NULL,
        "InvitedEmail" character varying(255) NOT NULL,
        "Role" integer NOT NULL,
        "Status" integer NOT NULL,
        "Token" character varying(100) NOT NULL,
        "ExpiresAt" timestamp with time zone NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "AcceptedAt" timestamp with time zone,
        CONSTRAINT "PK_Invitations" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Invitations_Accounts_AccountId" FOREIGN KEY ("AccountId") REFERENCES "Accounts" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_Invitations_Users_InvitedByUserId" FOREIGN KEY ("InvitedByUserId") REFERENCES "Users" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109022655_AddInvitations') THEN
    CREATE INDEX "IX_Invitations_AccountId" ON "Invitations" ("AccountId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109022655_AddInvitations') THEN
    CREATE INDEX "IX_Invitations_ExpiresAt" ON "Invitations" ("ExpiresAt");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109022655_AddInvitations') THEN
    CREATE INDEX "IX_Invitations_InvitedByUserId" ON "Invitations" ("InvitedByUserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109022655_AddInvitations') THEN
    CREATE INDEX "IX_Invitations_InvitedEmail" ON "Invitations" ("InvitedEmail");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109022655_AddInvitations') THEN
    CREATE INDEX "IX_Invitations_Status" ON "Invitations" ("Status");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109022655_AddInvitations') THEN
    CREATE UNIQUE INDEX "IX_Invitations_Token" ON "Invitations" ("Token");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109022655_AddInvitations') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260109022655_AddInvitations', '9.0.0');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109024953_AddAlertsAndNotifications') THEN
    CREATE TABLE "Alerts" (
        "Id" uuid NOT NULL,
        "AccountId" uuid NOT NULL,
        "UserId" uuid NOT NULL,
        "Type" integer NOT NULL,
        "Name" character varying(255) NOT NULL,
        "Description" character varying(500) NOT NULL,
        "IsActive" boolean NOT NULL,
        "ThresholdAmount" numeric,
        "ThresholdDays" integer,
        "CategoryId" uuid,
        "CreatedAt" timestamp with time zone NOT NULL,
        "LastTriggeredAt" timestamp with time zone,
        CONSTRAINT "PK_Alerts" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Alerts_Accounts_AccountId" FOREIGN KEY ("AccountId") REFERENCES "Accounts" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_Alerts_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id") ON DELETE SET NULL,
        CONSTRAINT "FK_Alerts_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109024953_AddAlertsAndNotifications') THEN
    CREATE TABLE "Notifications" (
        "Id" uuid NOT NULL,
        "UserId" uuid NOT NULL,
        "AlertId" uuid,
        "Type" integer NOT NULL,
        "Title" character varying(255) NOT NULL,
        "Message" character varying(1000) NOT NULL,
        "IsRead" boolean NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "ReadAt" timestamp with time zone,
        "ActionUrl" text,
        "Metadata" text,
        CONSTRAINT "PK_Notifications" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Notifications_Alerts_AlertId" FOREIGN KEY ("AlertId") REFERENCES "Alerts" ("Id") ON DELETE SET NULL,
        CONSTRAINT "FK_Notifications_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109024953_AddAlertsAndNotifications') THEN
    CREATE INDEX "IX_Alerts_AccountId" ON "Alerts" ("AccountId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109024953_AddAlertsAndNotifications') THEN
    CREATE INDEX "IX_Alerts_CategoryId" ON "Alerts" ("CategoryId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109024953_AddAlertsAndNotifications') THEN
    CREATE INDEX "IX_Alerts_IsActive" ON "Alerts" ("IsActive");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109024953_AddAlertsAndNotifications') THEN
    CREATE INDEX "IX_Alerts_Type" ON "Alerts" ("Type");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109024953_AddAlertsAndNotifications') THEN
    CREATE INDEX "IX_Alerts_UserId" ON "Alerts" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109024953_AddAlertsAndNotifications') THEN
    CREATE INDEX "IX_Notifications_AlertId" ON "Notifications" ("AlertId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109024953_AddAlertsAndNotifications') THEN
    CREATE INDEX "IX_Notifications_CreatedAt" ON "Notifications" ("CreatedAt");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109024953_AddAlertsAndNotifications') THEN
    CREATE INDEX "IX_Notifications_IsRead" ON "Notifications" ("IsRead");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109024953_AddAlertsAndNotifications') THEN
    CREATE INDEX "IX_Notifications_UserId" ON "Notifications" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109024953_AddAlertsAndNotifications') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260109024953_AddAlertsAndNotifications', '9.0.0');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109032803_AddBankingIntegration') THEN
    CREATE TABLE "BankConnections" (
        "Id" uuid NOT NULL,
        "AccountId" uuid NOT NULL,
        "UserId" uuid NOT NULL,
        "BankName" character varying(255) NOT NULL,
        "BankCode" character varying(50) NOT NULL,
        "InstitutionId" character varying(255) NOT NULL,
        "ItemId" character varying(255) NOT NULL,
        "Status" integer NOT NULL,
        "ConnectedAt" timestamp with time zone NOT NULL,
        "LastSyncAt" timestamp with time zone,
        "ErrorMessage" text,
        "AutoSync" boolean NOT NULL,
        "Metadata" text,
        CONSTRAINT "PK_BankConnections" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_BankConnections_Accounts_AccountId" FOREIGN KEY ("AccountId") REFERENCES "Accounts" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_BankConnections_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109032803_AddBankingIntegration') THEN
    CREATE TABLE "BankTransactions" (
        "Id" uuid NOT NULL,
        "BankConnectionId" uuid NOT NULL,
        "ExternalId" character varying(255) NOT NULL,
        "Description" character varying(500) NOT NULL,
        "Amount" numeric(18,2) NOT NULL,
        "Date" timestamp with time zone NOT NULL,
        "Type" integer NOT NULL,
        "Category" text,
        "Status" integer NOT NULL,
        "TransactionId" uuid,
        "CreatedAt" timestamp with time zone NOT NULL,
        "Metadata" text,
        CONSTRAINT "PK_BankTransactions" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_BankTransactions_BankConnections_BankConnectionId" FOREIGN KEY ("BankConnectionId") REFERENCES "BankConnections" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_BankTransactions_Transactions_TransactionId" FOREIGN KEY ("TransactionId") REFERENCES "Transactions" ("Id") ON DELETE SET NULL
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109032803_AddBankingIntegration') THEN
    CREATE INDEX "IX_BankConnections_AccountId" ON "BankConnections" ("AccountId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109032803_AddBankingIntegration') THEN
    CREATE INDEX "IX_BankConnections_ItemId" ON "BankConnections" ("ItemId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109032803_AddBankingIntegration') THEN
    CREATE INDEX "IX_BankConnections_Status" ON "BankConnections" ("Status");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109032803_AddBankingIntegration') THEN
    CREATE INDEX "IX_BankConnections_UserId" ON "BankConnections" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109032803_AddBankingIntegration') THEN
    CREATE INDEX "IX_BankTransactions_BankConnectionId" ON "BankTransactions" ("BankConnectionId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109032803_AddBankingIntegration') THEN
    CREATE INDEX "IX_BankTransactions_Date" ON "BankTransactions" ("Date");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109032803_AddBankingIntegration') THEN
    CREATE INDEX "IX_BankTransactions_ExternalId" ON "BankTransactions" ("ExternalId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109032803_AddBankingIntegration') THEN
    CREATE INDEX "IX_BankTransactions_Status" ON "BankTransactions" ("Status");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109032803_AddBankingIntegration') THEN
    CREATE INDEX "IX_BankTransactions_TransactionId" ON "BankTransactions" ("TransactionId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109032803_AddBankingIntegration') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260109032803_AddBankingIntegration', '9.0.0');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109162720_AddBudgets') THEN
    CREATE TABLE "Budgets" (
        "Id" uuid NOT NULL,
        "UserId" uuid NOT NULL,
        "CategoryId" uuid NOT NULL,
        "Amount" numeric(18,2) NOT NULL,
        "Period" text NOT NULL,
        "Month" integer NOT NULL,
        "Year" integer NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_Budgets" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Budgets_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_Budgets_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109162720_AddBudgets') THEN
    CREATE INDEX "IX_Budgets_CategoryId" ON "Budgets" ("CategoryId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109162720_AddBudgets') THEN
    CREATE INDEX "IX_Budgets_UserId" ON "Budgets" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109162720_AddBudgets') THEN
    CREATE UNIQUE INDEX "IX_Budgets_UserId_CategoryId_Month_Year" ON "Budgets" ("UserId", "CategoryId", "Month", "Year");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109162720_AddBudgets') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260109162720_AddBudgets', '9.0.0');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109163746_AddGoals') THEN
    CREATE TABLE "Goals" (
        "Id" uuid NOT NULL,
        "UserId" uuid NOT NULL,
        "Name" character varying(255) NOT NULL,
        "Description" character varying(1000) NOT NULL,
        "TargetAmount" numeric(18,2) NOT NULL,
        "CurrentAmount" numeric(18,2) NOT NULL,
        "TargetDate" timestamp with time zone NOT NULL,
        "Status" text NOT NULL,
        "Priority" text NOT NULL,
        "ImageUrl" character varying(500),
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_Goals" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Goals_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109163746_AddGoals') THEN
    CREATE TABLE "GoalContributions" (
        "Id" uuid NOT NULL,
        "GoalId" uuid NOT NULL,
        "Amount" numeric(18,2) NOT NULL,
        "Note" character varying(500),
        "ContributedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_GoalContributions" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_GoalContributions_Goals_GoalId" FOREIGN KEY ("GoalId") REFERENCES "Goals" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109163746_AddGoals') THEN
    CREATE INDEX "IX_GoalContributions_ContributedAt" ON "GoalContributions" ("ContributedAt");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109163746_AddGoals') THEN
    CREATE INDEX "IX_GoalContributions_GoalId" ON "GoalContributions" ("GoalId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109163746_AddGoals') THEN
    CREATE INDEX "IX_Goals_Status" ON "Goals" ("Status");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109163746_AddGoals') THEN
    CREATE INDEX "IX_Goals_TargetDate" ON "Goals" ("TargetDate");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109163746_AddGoals') THEN
    CREATE INDEX "IX_Goals_UserId" ON "Goals" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260109163746_AddGoals') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260109163746_AddGoals', '9.0.0');
    END IF;
END $EF$;
COMMIT;

