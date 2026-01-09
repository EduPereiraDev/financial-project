-- ============================================
-- DESABILITAR ROW LEVEL SECURITY (RLS)
-- Para uso com autenticação JWT do backend .NET
-- ============================================

-- A segurança será gerenciada pelo backend via JWT
-- O backend controla todas as permissões de acesso

ALTER TABLE "Users" DISABLE ROW LEVEL SECURITY;
ALTER TABLE "Accounts" DISABLE ROW LEVEL SECURITY;
ALTER TABLE "AccountMembers" DISABLE ROW LEVEL SECURITY;
ALTER TABLE "Categories" DISABLE ROW LEVEL SECURITY;
ALTER TABLE "Transactions" DISABLE ROW LEVEL SECURITY;
ALTER TABLE "RecurringTransactions" DISABLE ROW LEVEL SECURITY;
ALTER TABLE "RecurringTransactionExecutions" DISABLE ROW LEVEL SECURITY;
ALTER TABLE "Invitations" DISABLE ROW LEVEL SECURITY;
ALTER TABLE "Alerts" DISABLE ROW LEVEL SECURITY;
ALTER TABLE "Notifications" DISABLE ROW LEVEL SECURITY;
ALTER TABLE "BankConnections" DISABLE ROW LEVEL SECURITY;
ALTER TABLE "BankAccounts" DISABLE ROW LEVEL SECURITY;
ALTER TABLE "BankTransactions" DISABLE ROW LEVEL SECURITY;
ALTER TABLE "Budgets" DISABLE ROW LEVEL SECURITY;
ALTER TABLE "Goals" DISABLE ROW LEVEL SECURITY;

-- ============================================
-- VERIFICAÇÃO
-- ============================================
-- Execute para verificar que RLS foi desabilitado:
-- SELECT tablename, rowsecurity 
-- FROM pg_tables 
-- WHERE schemaname = 'public' 
-- ORDER BY tablename;
-- 
-- rowsecurity deve estar FALSE para todas as tabelas
