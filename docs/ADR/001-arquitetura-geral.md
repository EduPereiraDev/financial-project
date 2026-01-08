# ADR 001: Arquitetura Geral do Sistema

**Status**: Aprovado  
**Data**: Janeiro 2026  
**Decisores**: Eduardo Pereira  
**Contexto**: Definição da arquitetura para aplicativo de controle financeiro pessoal

---

## Contexto e Problema

Precisamos desenvolver um aplicativo de controle financeiro pessoal com as seguintes características:
- Suporte a múltiplos usuários (você e sua namorada)
- Controle de receitas e despesas
- Relatórios por múltiplos períodos (diário, semanal, mensal, trimestral, semestral, anual)
- Hospedagem 100% gratuita
- Interface moderna e responsiva

## Decisão

Adotaremos uma arquitetura **Client-Server** com as seguintes tecnologias:

### Backend
- **.NET 8 Web API**: Framework robusto, moderno e gratuito
- **Entity Framework Core**: ORM maduro com migrations
- **PostgreSQL**: Banco de dados relacional confiável
- **JWT**: Autenticação stateless e escalável

### Frontend
- **React 18 + Vite**: Build rápido e desenvolvimento ágil
- **TypeScript**: Segurança de tipos
- **TailwindCSS + shadcn/ui**: UI moderna e acessível
- **Recharts**: Visualizações de dados

### Hospedagem (Gratuita)
- **Vercel**: Frontend (100 GB bandwidth/mês)
- **Railway**: Backend + PostgreSQL ($5 crédito/mês)
- **Supabase**: PostgreSQL alternativo (500 MB storage)

## Consequências

### Positivas ✅
- Stack moderna e bem documentada
- Hospedagem totalmente gratuita para uso pessoal
- Escalabilidade futura sem mudanças arquiteturais
- Comunidade ativa e suporte
- TypeScript garante qualidade de código
- Deploy automático via Git

### Negativas ⚠️
- Limites de free tier (mitigado com otimizações)
- Railway pode ter cold starts (aceitável para uso pessoal)
- Necessidade de gerenciar dois deploys (frontend + backend)

### Riscos e Mitigações
1. **Risco**: Ultrapassar limites gratuitos
   - **Mitigação**: Monitoramento de uso, otimização de queries, cache

2. **Risco**: Downtime de provedores gratuitos
   - **Mitigação**: Backups regulares, documentação de migração

3. **Risco**: Performance com muitos dados
   - **Mitigação**: Paginação, índices de banco, lazy loading

## Alternativas Consideradas

### 1. Monolito com Server-Side Rendering (Next.js)
- **Prós**: Deploy único, SSR para SEO
- **Contras**: Menos flexibilidade, acoplamento frontend/backend
- **Motivo da rejeição**: Separação de concerns é melhor para manutenção

### 2. Serverless (AWS Lambda + DynamoDB)
- **Prós**: Auto-scaling, pay-per-use
- **Contras**: Complexidade, cold starts, custo potencial
- **Motivo da rejeição**: Overhead desnecessário para uso pessoal

### 3. Firebase (BaaS)
- **Prós**: Backend pronto, real-time
- **Contras**: Vendor lock-in, menos controle, NoSQL
- **Motivo da rejeição**: Preferência por SQL e controle total

## Padrões Arquiteturais

### Backend
- **Clean Architecture**: Separação de camadas (API, Domain, Infrastructure)
- **Repository Pattern**: Abstração de acesso a dados
- **Dependency Injection**: Nativo do .NET
- **CQRS Light**: Separação de comandos/queries quando necessário

### Frontend
- **Component-Based**: Componentes reutilizáveis
- **Context API**: Gerenciamento de estado global (auth)
- **Custom Hooks**: Lógica reutilizável
- **Atomic Design**: Organização de componentes

## Decisões Técnicas Específicas

### Autenticação
- **JWT com Refresh Tokens**: Stateless, escalável
- **BCrypt**: Hash de senhas (salt rounds: 12)
- **HTTPS Only**: Segurança em trânsito

### Banco de Dados
- **PostgreSQL 15+**: Recursos avançados, JSON support
- **Migrations**: Entity Framework Core Migrations
- **Índices**: Em UserId, Date, CategoryId para performance

### API Design
- **RESTful**: Padrão HTTP semântico
- **Versionamento**: Via URL (/api/v1/)
- **Paginação**: Cursor-based para grandes datasets
- **Validação**: FluentValidation no backend, Zod no frontend

## Estrutura de Pastas

```
Financial-Project/
├── backend/
│   └── FinancialControl.Api/
│       ├── Controllers/
│       ├── Models/
│       ├── DTOs/
│       ├── Data/
│       ├── Services/
│       ├── Repositories/
│       └── Middleware/
├── frontend/
│   └── src/
│       ├── components/
│       ├── pages/
│       ├── services/
│       ├── hooks/
│       └── contexts/
└── docs/
    ├── ROADMAP.md
    ├── TASKLIST.md
    └── ADR/
```

## Métricas de Sucesso

1. **Performance**: Load time < 2s
2. **Disponibilidade**: Uptime > 99%
3. **Custo**: $0/mês
4. **Manutenibilidade**: Código limpo, testado, documentado

## Referências

- [.NET 8 Documentation](https://learn.microsoft.com/dotnet/)
- [React Documentation](https://react.dev/)
- [Vercel Free Tier](https://vercel.com/pricing)
- [Railway Free Tier](https://railway.app/pricing)
- [Supabase Free Tier](https://supabase.com/pricing)

---

**Próxima Revisão**: Após MVP (v0.1.0)
