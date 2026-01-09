# 🗺️ Roadmap Completo - Financial Control App

> **Status Atual**: v0.3.0 - Compartilhamento de Conta ✅  
> **Última Atualização**: 09/01/2026 00:34 UTC-3

---

## ✅ O Que Já Está Funcionando

### v0.1.0 - MVP Funcional ✅

#### Backend (.NET 9)
- ✅ Autenticação JWT completa
- ✅ CRUD de transações
- ✅ CRUD de categorias
- ✅ CRUD de contas
- ✅ Sistema de membros (AccountMembers)
- ✅ Deploy no Render.com
- ✅ Banco de dados Supabase PostgreSQL
- ✅ CORS configurado para Vercel
- ✅ Swagger habilitado em produção

#### Frontend (React 18 + TypeScript)
- ✅ Autenticação (login/registro)
- ✅ Dashboard com estatísticas
- ✅ Página de transações com paginação
- ✅ Gráficos (Recharts)
- ✅ Formulários de transações
- ✅ Deploy no Vercel
- ✅ Design responsivo
- ✅ Integração completa com backend

### v0.2.0 - Receitas/Despesas Recorrentes ✅ **COMPLETO**

#### Backend Implementado (08/01/2026) ✅
- ✅ Modelo `RecurringTransaction` com 6 frequências
- ✅ Migration aplicada no Supabase (4 índices)
- ✅ Service com lógica de cálculo de próxima execução (233 linhas)
- ✅ Endpoints REST completos (6 endpoints CRUD + processamento)
- ✅ Método `ProcessDueRecurringTransactionsAsync` para job
- ✅ Hangfire instalado e configurado com PostgreSQL
- ✅ Job diário agendado (00:01 UTC / 21:01 Brasília)
- ✅ Dashboard Hangfire em `/hangfire`
- ✅ Build bem-sucedido (0.9s)
- ✅ Commit e push para produção

#### Frontend Implementado (08/01/2026) ✅
- ✅ Tipos TypeScript completos (70 linhas)
- ✅ `recurringTransactionService` com 6 métodos
- ✅ `categoryService` para buscar categorias
- ✅ `RecurringTransactionsPage` com cards visuais (220 linhas)
- ✅ `RecurringTransactionModal` completo (250 linhas)
- ✅ Rota `/recurring` configurada
- ✅ Cards coloridos (verde=receita, vermelho=despesa)
- ✅ Badge de status (Ativa/Inativa) clicável
- ✅ Ícones por frequência (📅📆🗓️📋📊🎯)
- ✅ Toggle ativo/inativo funcional
- ✅ Formatação de moeda e datas
- ✅ Build bem-sucedido (994 módulos, 1.8s)
- ✅ Commit e push para produção

#### Job Agendado Implementado (08/01/2026) ✅
- ✅ Hangfire configurado com PostgreSQL
- ✅ Job recorrente: Diariamente às 00:01 UTC
- ✅ Processa automaticamente transações vencidas
- ✅ Cria transações com sufixo "(Recorrente)"
- ✅ Atualiza próxima execução automaticamente
- ✅ Dashboard de monitoramento disponível
- ✅ Retry automático em caso de falha

#### Estatísticas da Implementação
- **Arquivos criados**: 10
- **Arquivos modificados**: 4
- **Linhas de código**: ~1.200
- **Commits**: 3
- **Tempo de desenvolvimento**: ~4 horas
- **Build status**: ✅ 100% sucesso
- **Deploy status**: ⏳ Em andamento

**Documentação Completa**:
- `docs/RECURRING-TRANSACTIONS-TESTS.md` - Testes e exemplos técnicos (414 linhas)
- `docs/IMPLEMENTATION-SUMMARY-v0.2.0.md` - Resumo executivo completo (390 linhas)

### v0.3.0 - Compartilhamento de Conta ✅ **COMPLETO**

#### Backend Implementado (09/01/2026) ✅
- ✅ Modelo `Invitation` com 5 status (Pending, Accepted, Rejected, Expired, Cancelled)
- ✅ Migration aplicada no Supabase (6 índices)
- ✅ `InvitationService` com 273 linhas e 6 métodos principais
- ✅ `InvitationsController` com 5 endpoints REST
- ✅ Geração de token seguro (32 bytes, Base64 URL-safe)
- ✅ Validação de permissões (apenas Owner pode convidar)
- ✅ Verificação de email duplicado
- ✅ Expiração automática de convites (7 dias)
- ✅ Método de limpeza de convites expirados
- ✅ 3 níveis de permissão (Owner, Editor, Viewer)
- ✅ Build bem-sucedido (1.0s)
- ✅ Commit e push para produção

#### Frontend Implementado (09/01/2026) ✅
- ✅ Tipos TypeScript completos (57 linhas)
- ✅ `invitationService` com 5 métodos
- ✅ `AccountMembersPage` com lista de membros e convites (185 linhas)
- ✅ `InviteMemberModal` com seleção de roles (114 linhas)
- ✅ Rota `/members` configurada
- ✅ Cards visuais para membros ativos
- ✅ Lista de convites pendentes com status
- ✅ Badges coloridos por status (Pending, Accepted, Expired, etc)
- ✅ Seleção visual de níveis de acesso com descrições
- ✅ Botão de cancelar convite (apenas pendentes)
- ✅ Formatação de datas em português
- ✅ Build bem-sucedido (998 módulos, 1.8s)
- ✅ Commit e push para produção

#### Endpoints REST (09/01/2026) ✅
1. ✅ `POST /api/invitations` - Criar convite (JWT)
2. ✅ `GET /api/invitations/account/{id}` - Listar convites (JWT)
3. ✅ `GET /api/invitations/token/{token}` - Buscar por token (público)
4. ✅ `POST /api/invitations/accept` - Aceitar convite (JWT)
5. ✅ `DELETE /api/invitations/{id}` - Cancelar convite (JWT)

#### Estatísticas da Implementação
- **Arquivos criados**: 11
- **Arquivos modificados**: 3
- **Linhas de código**: ~1.180
- **Commits**: 2
- **Tempo de desenvolvimento**: ~2 horas
- **Build status**: ✅ 100% sucesso
- **Deploy status**: ⏳ Em andamento

#### Funcionalidades Pendentes
- ⏳ Página de aceitar convite (frontend)
- ⏳ Envio de email automático com link do convite
- ⏳ Notificação quando convite é aceito
- ⏳ Remover membro da conta

**Documentação Completa**:
- `docs/IMPLEMENTATION-SUMMARY-v0.3.0.md` - Resumo executivo completo (380 linhas)

---

## 🚀 Features Prioritárias (v0.2.0 - v0.5.0)

### 📌 **FASE 1: Essenciais para Uso Diário** (v0.2.0 - 2 semanas)

#### 1. **💰 Receitas Recorrentes** ⭐⭐⭐⭐⭐
**Problema**: Você tem que cadastrar seu salário todo mês manualmente  
**Solução**: Cadastrar uma vez e o sistema cria automaticamente

**Features**:
- Cadastrar receita recorrente (ex: Salário todo dia 5)
- Frequências: Mensal, Semanal, Quinzenal, Anual
- Editar valor quando mudar
- Pausar/reativar recorrência
- Histórico de recebimentos
- Previsão de receitas futuras (próximos 3 meses)

**Backend**:
- Nova tabela `RecurringTransactions`
- Job agendado (Hangfire ou similar) para criar transações automaticamente
- Endpoint para CRUD de recorrências

**Frontend**:
- Página "Receitas Recorrentes"
- Modal de cadastro
- Lista com toggle ativo/inativo
- Badge "Recorrente" nas transações geradas

**Esforço**: 3-4 dias  
**Valor**: ⭐⭐⭐⭐⭐

---

#### 2. **🔄 Despesas Recorrentes (Assinaturas)** ⭐⭐⭐⭐⭐
**Problema**: Você esquece de cadastrar Netflix, Spotify, etc todo mês  
**Solução**: Sistema cadastra automaticamente

**Exemplos**:
- Netflix (R$ 55,90/mês - dia 15)
- Spotify (R$ 21,90/mês - dia 10)
- Academia (R$ 89,00/mês - dia 1)
- Aluguel (R$ 1.200/mês - dia 5)
- Condomínio (R$ 350/mês - dia 10)

**Features**:
- Mesmo sistema de receitas recorrentes
- Dashboard mostra "Total de assinaturas: R$ 716,80/mês"
- Alerta antes do vencimento (3 dias antes)
- Sugestão de cancelamento (se não usar)
- Categoria automática "Assinaturas"

**Esforço**: 2-3 dias (aproveita código de receitas)  
**Valor**: ⭐⭐⭐⭐⭐

---

#### 3. **👫 Compartilhamento de Conta (Sincronização Casal)** ⭐⭐⭐⭐⭐
**Problema**: Você e sua namorada querem gerenciar as finanças juntos  
**Solução**: Uma conta compartilhada entre vocês dois

**Como Funciona**:
- Você convida sua namorada por email
- Ela aceita o convite e tem acesso à mesma conta
- Ambos veem as mesmas transações
- Ambos podem adicionar/editar/deletar
- Cada um tem suas categorias pessoais (opcional)
- Relatórios consolidados

**Permissões**:
- **Owner** (você): Controle total + gerenciar membros
- **Editor** (namorada): Adicionar/editar transações
- **Viewer** (futuro): Apenas visualizar

**Features**:
- Sistema de convites por email
- Gerenciamento de membros
- Histórico de quem fez o quê (auditoria)
- Notificações quando alguém adiciona transação
- Avatar/nome de quem criou cada transação

**Backend**:
- Tabela `AccountMembers` já existe! ✅
- Adicionar sistema de convites (tabela `Invitations`)
- Endpoint de convite por email
- Middleware de permissões

**Frontend**:
- Página "Membros da Conta"
- Modal de convite
- Lista de membros com permissões
- Badge "Adicionado por [nome]" nas transações

**Esforço**: 1-2 semanas  
**Valor**: ⭐⭐⭐⭐⭐ (ESSENCIAL para casais!)

---

### 📌 **FASE 2: Inteligência e Automação** (v0.3.0 - 1 semana)

#### 4. **🔔 Alertas e Notificações Inteligentes** ⭐⭐⭐⭐⭐
**Problema**: Você só vê os gastos quando entra no app  
**Solução**: Sistema te avisa proativamente

**Tipos de Alertas**:
- 🚨 Orçamento excedido (ex: "Você gastou 120% do orçamento de alimentação")
- 💰 Receita recebida (ex: "Salário de R$ 5.000 creditado")
- 📊 Gastos incomuns (ex: "Você gastou 50% a mais este mês")
- 🎯 Meta próxima de ser atingida (ex: "Faltam apenas R$ 200 para sua meta!")
- ⚠️ Despesas grandes (ex: "Compra de R$ 1.500 detectada")
- 📅 Assinatura vencendo (ex: "Netflix vence em 3 dias")
- 🔄 Recorrência criada (ex: "Salário de R$ 5.000 adicionado automaticamente")

**Canais**:
- In-app (badge + lista de notificações)
- Email (opcional, configurável)
- Push notification (PWA - futuro)

**Backend**:
- Tabela `Notifications`
- Sistema de regras de alerta
- Job para processar alertas diariamente
- Endpoint para marcar como lido

**Frontend**:
- Ícone de sino com badge
- Dropdown de notificações
- Página de histórico
- Configurações de preferências

**Esforço**: 5-7 dias  
**Valor**: ⭐⭐⭐⭐⭐

---

#### 5. **📈 Análise de Tendências e Insights** ⭐⭐⭐⭐
**Problema**: Você não sabe se está gastando mais ou menos que antes  
**Solução**: Sistema analisa e mostra insights automáticos

**Exemplos de Insights**:
- 📊 "Você gastou 30% a mais em alimentação este mês"
- 💡 "Seus gastos com transporte diminuíram 15%"
- ⚠️ "Atenção: gastos com lazer aumentaram 50%"
- 🎯 "No ritmo atual, você economizará R$ 800 este mês"
- 📅 "Seus maiores gastos são às sextas-feiras"
- 💰 "Você economizou R$ 1.200 nos últimos 3 meses"

**Visualizações**:
- Gráfico de tendência (últimos 6 meses)
- Comparação mês a mês
- Previsão de gastos futuros
- Ranking de categorias
- Heatmap de gastos por dia da semana

**Esforço**: 5-7 dias  
**Valor**: ⭐⭐⭐⭐

---

#### 6. **🎯 Metas Financeiras** ⭐⭐⭐⭐⭐
**Problema**: Você não tem motivação para economizar  
**Solução**: Definir objetivos com acompanhamento visual

**Exemplos**:
- "Economizar R$ 5.000 para viagem em 6 meses"
- "Juntar R$ 20.000 para entrada do carro em 1 ano"
- "Reserva de emergência de R$ 10.000"
- "Comprar notebook de R$ 4.000 em 4 meses"

**Features**:
- Criar meta com valor alvo e prazo
- Barra de progresso visual
- Sugestão de quanto economizar por mês
- Notificação quando atingir meta
- Histórico de metas alcançadas
- Comemoração visual ao completar 🎉

**Backend**:
- Tabela `Goals`
- Cálculo automático de progresso
- Endpoint para CRUD de metas

**Frontend**:
- Página "Metas"
- Card de meta com barra de progresso
- Modal de criação
- Animação ao completar meta

**Esforço**: 5-7 dias  
**Valor**: ⭐⭐⭐⭐⭐

---

### 📌 **FASE 3: UX e Polimento** (v0.4.0 - 1 semana)

#### 7. **🌙 Modo Escuro (Dark Mode)** ⭐⭐⭐⭐
**Benefícios**:
- Menos cansaço visual à noite
- Economia de bateria (OLED)
- Preferência pessoal

**Implementação**:
- Toggle no perfil/header
- Salva preferência no localStorage
- Aplica em todo o app
- Transição suave

**Esforço**: 2 dias  
**Valor**: ⭐⭐⭐⭐

---

#### 8. **🔍 Busca e Filtros Avançados** ⭐⭐⭐⭐
**Problema**: Difícil encontrar transações específicas  
**Solução**: Sistema de busca poderoso

**Filtros**:
- Por descrição (busca texto)
- Por categoria (múltiplas)
- Por valor (range: R$ 20 - R$ 50)
- Por data (range: 01/01 - 31/01)
- Por tipo (receita/despesa)
- Por membro (quem criou)
- Combinação de filtros
- Salvar filtros favoritos

**Esforço**: 3 dias  
**Valor**: ⭐⭐⭐⭐

---

#### 9. **📱 PWA (App Instalável)** ⭐⭐⭐⭐⭐
**Benefícios**:
- Ícone na tela inicial do celular
- Funciona offline (básico)
- Notificações push
- Experiência de app nativo
- Splash screen

**Esforço**: 2 dias  
**Valor**: ⭐⭐⭐⭐⭐

---

#### 10. **📄 Exportação de Relatórios** ⭐⭐⭐⭐
**Formatos**:
- PDF (relatório visual bonito)
- Excel/CSV (para análise)
- JSON (para backup)

**Tipos de Relatório**:
- Extrato completo
- Relatório por categoria
- Relatório por período
- Resumo mensal/anual

**Esforço**: 3 dias  
**Valor**: ⭐⭐⭐⭐

---

## 🏦 FASE 4: Integração Bancária (v1.0.0 - 2 semanas)

### **🔗 Integração com Bancos (Pluggy/Belvo)** ⭐⭐⭐⭐⭐
**Problema**: Você tem que cadastrar cada transação manualmente  
**Solução**: Conectar com seu banco e importar automaticamente

**Como Funciona**:
1. Você conecta sua conta bancária (Nubank, Inter, etc)
2. Sistema importa transações automaticamente
3. IA categoriza automaticamente
4. Você só revisa e confirma

**Serviços de Open Banking**:
- **Pluggy** (recomendado - brasileiro, fácil integração)
- **Belvo** (alternativa)
- **Plaid** (internacional)

**Features**:
- Conectar múltiplas contas bancárias
- Sincronização automática diária
- Categorização inteligente
- Detecção de duplicatas
- Reconciliação manual

**Backend**:
- Integração com API Pluggy
- Webhook para sincronização
- Job de sincronização agendado
- Tabela `BankConnections`

**Frontend**:
- Página "Contas Bancárias"
- Modal de conexão (iframe Pluggy)
- Status de sincronização
- Lista de transações importadas

**Esforço**: 2 semanas  
**Valor**: ⭐⭐⭐⭐⭐ (GAME CHANGER!)

**Custo**: Pluggy tem plano gratuito para desenvolvimento

---

## 🎨 Melhorias de UX/UI (Contínuo)

### **Micro-interações**:
- Animações suaves ao adicionar transação
- Feedback visual ao salvar
- Loading states elegantes
- Toasts informativos
- Confetti ao completar meta 🎉

### **Acessibilidade**:
- Suporte a screen readers
- Navegação por teclado
- Contraste adequado (WCAG 2.1)
- Textos alternativos

### **Performance**:
- Lazy loading de gráficos
- Virtual scrolling em listas grandes
- Cache inteligente
- Otimização de imagens

---

## 🔮 Features Futuras (v1.1.0+)

### **Avançadas**:
- 🤖 IA para categorização automática (Machine Learning)
- 📸 OCR de notas fiscais (tirar foto e extrair dados)
- 📈 Investimentos tracking (ações, fundos, cripto)
- 💳 Integração com cartões de crédito
- 🌍 Múltiplas moedas (USD, EUR)
- 🎮 Gamificação (badges, desafios, streaks)
- 💬 Assistente virtual (chatbot)
- 🏷️ Tags customizadas
- 📎 Anexos (notas fiscais)
- 🧮 Calculadora de divisão de contas
- 🔒 2FA (autenticação em dois fatores)
- 📊 Dashboard executivo
- 🔌 API pública

---

## 📅 Cronograma Sugerido

### **v0.2.0 - Recorrências** (Semanas 1-2)
- ✅ Receitas recorrentes (3-4 dias)
- ✅ Despesas recorrentes (2-3 dias)
- ✅ Testes e ajustes (2 dias)

### **v0.3.0 - Inteligência** (Semanas 3-4)
- ✅ Alertas e notificações (5-7 dias)
- ✅ Análise de tendências (5-7 dias)

### **v0.4.0 - Compartilhamento** (Semanas 5-6)
- ✅ Sistema de convites (3 dias)
- ✅ Permissões de usuário (3 dias)
- ✅ Histórico de alterações (3 dias)
- ✅ Testes e ajustes (2 dias)

### **v0.5.0 - UX** (Semana 7)
- ✅ Modo escuro (2 dias)
- ✅ Busca avançada (3 dias)
- ✅ PWA (2 dias)

### **v0.6.0 - Metas** (Semana 8)
- ✅ Metas financeiras (5-7 dias)

### **v0.7.0 - Relatórios** (Semana 9)
- ✅ Exportação PDF/Excel (3 dias)
- ✅ Relatórios customizados (4 dias)

### **v1.0.0 - Banking** (Semanas 10-12)
- ✅ Integração Pluggy (2 semanas)
- ✅ Testes e ajustes (1 semana)

**Total**: ~12 semanas (~3 meses) para app completo! 🚀

---

## 💰 Priorização por ROI

### **🔥 CRÍTICAS** (Implementar AGORA):
1. ⭐⭐⭐⭐⭐ Receitas/Despesas recorrentes (economiza MUITO tempo)
2. ⭐⭐⭐⭐⭐ Compartilhamento de conta (essencial para casais)
3. ⭐⭐⭐⭐⭐ Alertas e notificações (engajamento)

### **🚀 IMPORTANTES** (Implementar em seguida):
4. ⭐⭐⭐⭐⭐ Metas financeiras (motivação)
5. ⭐⭐⭐⭐⭐ PWA instalável (experiência mobile)
6. ⭐⭐⭐⭐ Modo escuro (preferência)
7. ⭐⭐⭐⭐ Busca avançada (usabilidade)

### **💎 DIFERENCIAIS** (Implementar depois):
8. ⭐⭐⭐⭐⭐ Integração bancária (GAME CHANGER)
9. ⭐⭐⭐⭐ Análise de tendências (insights)
10. ⭐⭐⭐⭐ Exportação de relatórios (profissional)

---

## 🎯 Minha Recomendação TOP 3 para Começar

### **1. Receitas/Despesas Recorrentes** (1 semana)
**Por quê**: Você vai economizar MUITO tempo todo mês. É a feature que mais vai impactar seu uso diário.

**Ordem de implementação**:
1. Backend: Tabela `RecurringTransactions` + endpoints
2. Backend: Job agendado (pode usar um cron job simples)
3. Frontend: Página de gerenciamento
4. Frontend: Badge "Recorrente" nas transações

---

### **2. Compartilhamento de Conta** (2 semanas)
**Por quê**: Essencial para você e sua namorada gerenciarem as finanças juntos. Diferencial competitivo.

**Ordem de implementação**:
1. Backend: Sistema de convites
2. Backend: Middleware de permissões
3. Frontend: Página de membros
4. Frontend: Indicador de quem criou cada transação

---

### **3. Alertas e Notificações** (1 semana)
**Por quê**: Mantém você engajado e informado sem precisar entrar no app toda hora.

**Ordem de implementação**:
1. Backend: Tabela `Notifications` + regras
2. Backend: Job de processamento
3. Frontend: Ícone de sino + dropdown
4. Frontend: Página de histórico

---

## 📝 Próximos Passos Imediatos

### **Opção A: Começar com Recorrências** (Recomendado)
```bash
# 1. Criar branch
git checkout -b feature/recurring-transactions

# 2. Backend primeiro
cd backend
# - Criar migration para RecurringTransactions
# - Criar DTOs e Services
# - Criar endpoints

# 3. Frontend depois
cd frontend
# - Criar página de gerenciamento
# - Criar modals de cadastro
# - Integrar com backend

# 4. Testar e fazer deploy
```

### **Opção B: Começar com Compartilhamento** (Se priorizar casal)
```bash
# 1. Criar branch
git checkout -b feature/account-sharing

# 2. Backend primeiro
cd backend
# - Criar sistema de convites
# - Adicionar permissões
# - Criar endpoints

# 3. Frontend depois
cd frontend
# - Criar página de membros
# - Criar modal de convite
# - Adicionar indicadores

# 4. Testar e fazer deploy
```

### **Opção C: Começar com Integração Bancária** (Se quiser o GAME CHANGER)
```bash
# 1. Criar conta no Pluggy
# https://pluggy.ai

# 2. Estudar documentação
# https://docs.pluggy.ai

# 3. Criar branch
git checkout -b feature/bank-integration

# 4. Implementar (2 semanas)
```

---

## 🎓 Recursos para Implementação

### **Integração Bancária**:
- [Pluggy Docs](https://docs.pluggy.ai)
- [Pluggy GitHub](https://github.com/pluggyai)
- [Open Banking Brasil](https://openbankingbrasil.org.br)

### **Notificações Push (PWA)**:
- [Web Push API](https://developer.mozilla.org/en-US/docs/Web/API/Push_API)
- [Firebase Cloud Messaging](https://firebase.google.com/docs/cloud-messaging)

### **Jobs Agendados (.NET)**:
- [Hangfire](https://www.hangfire.io)
- [Quartz.NET](https://www.quartz-scheduler.net)

### **Exportação PDF**:
- [QuestPDF](https://www.questpdf.com) (recomendado)
- [iTextSharp](https://github.com/itext/itextsharp)

---

## 💡 Dica Final

**Não tente implementar tudo de uma vez!** 

Minha sugestão:
1. ✅ Use o app por 1 semana como está
2. ✅ Identifique o que mais te incomoda
3. ✅ Implemente as TOP 3 features acima
4. ✅ Use por mais 1 mês
5. ✅ Avalie necessidade das demais

**Lembre-se**: Um app simples que você usa é melhor que um app complexo que você não usa! 🎯

---

## 🤝 Quer Ajuda para Implementar?

Posso te ajudar a implementar qualquer uma dessas features. Só me dizer qual você quer começar! 🚀

**Sugestão**: Começar com **Receitas/Despesas Recorrentes** porque:
- ✅ Rápido (1 semana)
- ✅ Alto impacto no uso diário
- ✅ Relativamente simples
- ✅ Base para outras features

Bora começar? 💪
