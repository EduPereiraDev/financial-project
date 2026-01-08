# 🗺️ ROADMAP - Financial Control App

> **Planejamento estratégico de desenvolvimento e releases**

---

## 📊 Visão Geral

Este roadmap define as fases de desenvolvimento do aplicativo de controle financeiro, priorizando entregas incrementais de valor com foco em MVP rápido e iterações baseadas em feedback.

---

## 🎯 Objetivos Estratégicos

1. **MVP Funcional**: Entregar valor básico em 2 semanas
2. **Hospedagem Gratuita**: Zero custos operacionais
3. **UX Moderna**: Interface intuitiva e responsiva
4. **Escalabilidade**: Arquitetura preparada para crescimento
5. **Qualidade**: Código limpo, testado e documentado

---

## 📅 Timeline de Releases

```
┌─────────────────────────────────────────────────────────────┐
│  Semana 1-2  │  Semana 3-4  │  Semana 5-6  │  Semana 7-9   │
├──────────────┼──────────────┼──────────────┼───────────────┤
│   v0.1.0     │   v0.2.0     │   v0.3.0     │    v1.0.0     │
│     MVP      │  Core Feat.  │  Analytics   │   Advanced    │
└─────────────────────────────────────────────────────────────┘
```

---

## 🚀 Fase 1: MVP (v0.1.0) - Semanas 1-2

**Objetivo**: Aplicativo funcional com features essenciais

### Backend (Semana 1)
- [x] Estrutura do projeto .NET 8
- [x] Configuração do Entity Framework Core
- [x] Modelos de dados básicos (User, Transaction, Category)
- [x] Autenticação JWT
- [x] Endpoints de autenticação (Login, Register)
- [x] CRUD de transações
- [x] Migrations iniciais
- [x] Configuração de CORS
- [x] Logging com Serilog
- [x] Swagger/OpenAPI

### Frontend (Semana 2)
- [ ] Setup React + Vite + TypeScript
- [ ] Configuração TailwindCSS + shadcn/ui
- [ ] Página de Login
- [ ] Página de Registro
- [ ] Página de Dashboard (básico)
- [ ] Listagem de transações
- [ ] Formulário de adicionar transação
- [ ] Autenticação com JWT (context)
- [ ] Roteamento protegido

### DevOps (Semana 2)
- [ ] Dockerfile backend
- [ ] Docker Compose (dev)
- [ ] Deploy backend no Railway
- [ ] Deploy frontend no Vercel
- [ ] Configuração Supabase PostgreSQL
- [ ] CI/CD básico (GitHub Actions)

### Entregáveis v0.1.0
✅ Usuários podem se registrar e fazer login  
✅ Usuários podem adicionar/editar/excluir transações  
✅ Dashboard mostra lista de transações  
✅ Aplicação deployada e acessível online  

---

## 🎨 Fase 2: Core Features (v0.2.0) - Semanas 3-4

**Objetivo**: Funcionalidades principais de controle financeiro

### Backend (Semana 3)
- [ ] Modelo de Categorias customizáveis
- [ ] Modelo de Receitas Fixas (RecurringIncome)
- [ ] Endpoints de categorias (CRUD)
- [ ] Endpoints de receitas fixas (CRUD)
- [ ] Filtros de transações por período
- [ ] Endpoint de resumo financeiro (summary)
- [ ] Validações com FluentValidation
- [ ] Testes unitários (controllers principais)

### Frontend (Semana 4)
- [ ] Página de Categorias
- [ ] Página de Receitas Fixas
- [ ] Filtros de período no dashboard
  - [ ] Diário
  - [ ] Semanal
  - [ ] Mensal
  - [ ] Trimestral
  - [ ] Semestral
  - [ ] Anual
- [ ] Cards de resumo (receitas, despesas, saldo)
- [ ] Gráfico de pizza (distribuição por categoria)
- [ ] Melhorias de UX (loading states, toasts)
- [ ] Responsividade mobile

### Entregáveis v0.2.0
✅ Categorias personalizadas  
✅ Receitas fixas cadastradas  
✅ Filtros por período funcionando  
✅ Visualização básica de distribuição  
✅ Interface responsiva  

---

## 📊 Fase 3: Analytics (v0.3.0) - Semanas 5-6

**Objetivo**: Análises avançadas e visualizações

### Backend (Semana 5)
- [ ] Modelo de Orçamentos (Budget)
- [ ] Endpoints de orçamentos
- [ ] Endpoint de analytics por período
- [ ] Endpoint de comparação de períodos
- [ ] Cálculo de tendências
- [ ] Alertas de orçamento excedido
- [ ] Exportação de dados (CSV/Excel)
- [ ] Testes de integração

### Frontend (Semana 6)
- [ ] Página de Orçamentos
- [ ] Gráfico de linha (evolução temporal)
- [ ] Gráfico de barra (comparação)
- [ ] Página de Relatórios
- [ ] Exportação de relatórios
- [ ] Filtros avançados (múltiplas categorias, ranges)
- [ ] Indicadores de tendência (↑↓)
- [ ] Comparação período anterior
- [ ] Dashboard analytics completo

### Entregáveis v0.3.0
✅ Orçamentos por categoria  
✅ Gráficos avançados (linha, barra, pizza)  
✅ Relatórios exportáveis  
✅ Análise de tendências  
✅ Comparação de períodos  

---

## 🔮 Fase 4: Advanced Features (v1.0.0) - Semanas 7-9

**Objetivo**: Features avançadas e polimento para v1.0

### Backend (Semana 7-8)
- [ ] Compartilhamento de contas (shared accounts)
- [ ] Permissões de usuário (owner, viewer)
- [ ] Modelo de Metas Financeiras (Goals)
- [ ] Sistema de notificações
- [ ] Webhook para alertas
- [ ] Cache com Redis (opcional)
- [ ] Rate limiting
- [ ] Auditoria de ações
- [ ] Backup automático
- [ ] Testes E2E

### Frontend (Semana 8-9)
- [ ] Página de Metas Financeiras
- [ ] Sistema de notificações in-app
- [ ] Compartilhamento de conta
- [ ] Configurações de perfil
- [ ] Tema dark/light
- [ ] PWA (Progressive Web App)
- [ ] Offline support básico
- [ ] Animações e transições
- [ ] Acessibilidade (WCAG 2.1)
- [ ] Testes E2E (Playwright)

### DevOps (Semana 9)
- [ ] Monitoramento (Sentry/LogRocket)
- [ ] Analytics (Google Analytics/Plausible)
- [ ] Health checks
- [ ] Auto-scaling (se necessário)
- [ ] Documentação completa
- [ ] Vídeo tutorial

### Entregáveis v1.0.0
✅ Compartilhamento entre usuários  
✅ Metas financeiras  
✅ Notificações  
✅ PWA instalável  
✅ Tema dark/light  
✅ Monitoramento completo  
✅ Documentação completa  

---

## 🔮 Fase 5: Melhorias Avançadas (v1.1.0) - Semanas 13-14

**Objetivo**: Features de alto valor agregado

### Backend (Semana 13)
- [ ] Receitas recorrentes (salário fixo)
- [ ] Despesas recorrentes (assinaturas)
- [ ] Sistema de tags customizadas
- [ ] Anexos (upload de notas fiscais)
- [ ] Histórico de alterações (auditoria)

### Frontend (Semana 14)
- [ ] Metas financeiras
- [ ] Análise de tendências e insights
- [ ] Comparação com mês anterior
- [ ] Busca e filtros avançados
- [ ] Exportação de relatórios (PDF)
- [ ] Widget de resumo na home

### Entregáveis v1.1.0
✅ Receitas/despesas recorrentes  
✅ Metas financeiras  
✅ Insights automáticos  
✅ Busca avançada  
✅ Exportação PDF  

---

## 🚀 Fase 6: Experiência Premium (v1.2.0) - Semanas 15-16

**Objetivo**: UX de alto nível

### Features (Semana 15-16)
- [ ] PWA completo (instalável)
- [ ] Modo escuro
- [ ] Notificações push
- [ ] Offline support básico
- [ ] Atalhos de teclado
- [ ] Onboarding interativo
- [ ] Backup automático
- [ ] Calculadora de divisão de contas
- [ ] Gráfico de fluxo de caixa
- [ ] Acessibilidade completa (WCAG 2.1)

### Entregáveis v1.2.0
✅ PWA instalável  
✅ Modo escuro  
✅ Notificações  
✅ Backup automático  
✅ Acessibilidade  

---

## 🔮 Backlog Futuro (v2.0+)

### Features Avançadas
- [ ] Integração com cartões de crédito
- [ ] Reconhecimento de recibos (OCR)
- [ ] IA para categorização automática
- [ ] Previsão de gastos (ML)
- [ ] Investimentos tracking
- [ ] Múltiplas moedas
- [ ] Planejamento financeiro
- [ ] Assistente virtual (chatbot)
- [ ] Gamificação (badges, desafios)
- [ ] Planejamento de viagens
- [ ] API pública
- [ ] Marketplace de integrações

### Melhorias Técnicas
- [ ] GraphQL API
- [ ] WebSockets para real-time
- [ ] Cache com Redis
- [ ] CDN para assets
- [ ] Otimização de performance
- [ ] Testes de carga
- [ ] Migração para microserviços (se escalar)

---

## 📈 Métricas de Sucesso

### MVP (v0.1.0)
- ✅ Aplicação deployada
- ✅ 2 usuários ativos (você + namorada)
- ✅ 10+ transações cadastradas
- ✅ Zero bugs críticos

### v0.2.0
- ✅ 5+ categorias customizadas
- ✅ Uso diário por 1 semana
- ✅ Relatórios gerados
- ✅ Feedback positivo dos usuários

### v0.3.0
- ✅ Orçamentos configurados
- ✅ Exportação de dados funcionando
- ✅ Análises sendo utilizadas
- ✅ Performance < 2s load time

### v1.0.0
- ✅ 100% features implementadas
- ✅ Cobertura de testes > 70%
- ✅ Documentação completa
- ✅ Zero custos de hospedagem
- ✅ Satisfação dos usuários

---

## 🎯 Priorização de Features

### Critérios de Priorização
1. **Valor para o Usuário**: Impacto direto na experiência
2. **Complexidade**: Esforço de desenvolvimento
3. **Dependências**: Bloqueadores técnicos
4. **ROI**: Retorno sobre investimento de tempo

### Matriz de Priorização

```
Alto Valor │ ✅ MVP          │ 🎨 Core Features
           │ (Fazer Agora)   │ (Fazer Logo)
           ├─────────────────┼──────────────────
Baixo Valor│ 🔮 Advanced     │ 📋 Backlog
           │ (Fazer Depois)  │ (Avaliar)
           └─────────────────┴──────────────────
             Baixa Complexidade  Alta Complexidade
```

---

## 🚧 Riscos e Mitigações

### Riscos Identificados

1. **Limite de Free Tier**
   - **Risco**: Ultrapassar limites gratuitos
   - **Mitigação**: Monitorar uso, otimizar queries, cache

2. **Performance com Dados Crescentes**
   - **Risco**: Lentidão com muitas transações
   - **Mitigação**: Paginação, índices, lazy loading

3. **Segurança de Dados**
   - **Risco**: Vazamento de dados financeiros
   - **Mitigação**: HTTPS, JWT, validações, auditoria

4. **Disponibilidade**
   - **Risco**: Downtime de provedores gratuitos
   - **Mitigação**: Health checks, fallbacks, backups

---

## 📝 Notas de Versão

### v0.1.0 (Planejado)
- Primeira versão funcional
- Autenticação e transações básicas
- Deploy inicial

### v0.2.0 (Planejado)
- Categorias e receitas fixas
- Filtros por período
- Gráficos básicos

### v0.3.0 (Planejado)
- Orçamentos
- Analytics avançado
- Exportação de dados

### v1.0.0 (Planejado)
- Compartilhamento
- Metas financeiras
- PWA completo

---

## 🔄 Processo de Atualização

1. **Planning**: Definir features da sprint
2. **Development**: Implementar features
3. **Testing**: Testes automatizados + manuais
4. **Review**: Code review + QA
5. **Deploy**: CI/CD automático
6. **Monitoring**: Acompanhar métricas
7. **Feedback**: Coletar feedback dos usuários
8. **Iterate**: Ajustar roadmap baseado em aprendizados

---

## 📞 Contato e Feedback

Para sugestões de features ou reportar bugs:
- Criar issue no GitHub
- Feedback direto dos usuários principais

---

**Última atualização**: Janeiro 2026  
**Próxima revisão**: Após v0.1.0 (2 semanas)
