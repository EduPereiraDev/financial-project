# 💰 Financial Control App

> **Aplicativo de Controle Financeiro Pessoal com Multi-Usuário**

Sistema completo para gerenciamento de finanças pessoais com suporte a múltiplos usuários, permitindo controle detalhado de receitas, despesas e análises por período (diário, semanal, mensal, trimestral, semestral e anual).

---

## 📋 Índice

- [Visão Geral](#-visão-geral)
- [Features](#-features)
- [Arquitetura](#-arquitetura)
- [Stack Tecnológica](#-stack-tecnológica)
- [Hospedagem Gratuita](#-hospedagem-gratuita)
- [Instalação](#-instalação)
- [Estrutura do Projeto](#-estrutura-do-projeto)
- [Documentação](#-documentação)
- [Roadmap](#-roadmap)

---

## 🎯 Visão Geral

Aplicativo web moderno para controle financeiro pessoal, desenvolvido com as melhores práticas de mercado e **hospedagem 100% gratuita**.

### Objetivos

- ✅ Controlar gastos e receitas de forma detalhada
- ✅ Categorizar transações financeiras
- ✅ Visualizar relatórios por múltiplos períodos
- ✅ Suportar múltiplos usuários com autenticação segura
- ✅ Interface moderna e responsiva
- ✅ Deploy e hospedagem totalmente gratuitos

---

## ✨ Features

### 🔐 Autenticação e Segurança
- Login/Registro com JWT
- Refresh tokens para sessões longas
- Hash de senhas com BCrypt
- Validação de dados em todas as camadas

### 💸 Gestão Financeira
- **Transações**: Adicionar, editar, excluir receitas e despesas
- **Categorias**: Personalizar categorias de gastos
- **Receitas Fixas**: Cadastrar salário e outras receitas recorrentes
- **Orçamentos**: Definir limites de gastos por categoria

### 📊 Relatórios e Análises
- **Visão Diária**: Gastos do dia atual
- **Visão Semanal**: Resumo da semana
- **Visão Mensal**: Análise mensal completa
- **Visão Trimestral**: Últimos 3 meses
- **Visão Semestral**: Últimos 6 meses
- **Visão Anual**: Análise do ano completo

### 📈 Visualizações
- Gráficos de pizza (distribuição por categoria)
- Gráficos de linha (evolução temporal)
- Gráficos de barra (comparação de períodos)
- Cards com métricas principais (total receitas, despesas, saldo)

### 👥 Multi-Usuário
- Cada usuário tem seus próprios dados
- Isolamento completo entre contas
- Possibilidade de compartilhamento futuro (roadmap)

---

## 🏗️ Arquitetura

```
┌─────────────────────────────────────────────────────────────┐
│                         FRONTEND                             │
│  React 18 + Vite + TailwindCSS + shadcn/ui + Recharts       │
│                    (Vercel - Grátis)                         │
└──────────────────────┬──────────────────────────────────────┘
                       │ HTTPS/REST API
┌──────────────────────┴──────────────────────────────────────┐
│                         BACKEND                              │
│        .NET 8 Web API + Entity Framework Core                │
│              (Railway/Render - Grátis)                       │
└──────────────────────┬──────────────────────────────────────┘
                       │ PostgreSQL
┌──────────────────────┴──────────────────────────────────────┐
│                        DATABASE                              │
│                  PostgreSQL 15+                              │
│                (Supabase/Railway - Grátis)                   │
└─────────────────────────────────────────────────────────────┘
```

### Padrões Arquiteturais

- **Clean Architecture**: Separação de camadas (API, Domain, Infrastructure)
- **Repository Pattern**: Abstração de acesso a dados
- **CQRS Light**: Separação de comandos e queries quando necessário
- **Dependency Injection**: Inversão de controle nativa do .NET

---

## 🛠️ Stack Tecnológica

### Backend
- **.NET 8**: Framework principal
- **ASP.NET Core Web API**: API RESTful
- **Entity Framework Core 8**: ORM
- **PostgreSQL**: Banco de dados
- **JWT Bearer**: Autenticação
- **BCrypt.Net**: Hash de senhas
- **FluentValidation**: Validação de dados
- **Serilog**: Logging estruturado
- **Swagger/OpenAPI**: Documentação da API

### Frontend
- **React 18**: Biblioteca UI
- **Vite**: Build tool moderna
- **TypeScript**: Tipagem estática
- **TailwindCSS**: Estilização utility-first
- **shadcn/ui**: Componentes acessíveis
- **Recharts**: Gráficos e visualizações
- **React Router**: Roteamento
- **Axios**: Cliente HTTP
- **React Hook Form**: Gerenciamento de formulários
- **Zod**: Validação de schemas
- **date-fns**: Manipulação de datas
- **Lucide React**: Ícones modernos

### DevOps & Infraestrutura
- **Docker**: Containerização
- **GitHub Actions**: CI/CD
- **Vercel**: Hospedagem frontend (grátis)
- **Railway/Render**: Hospedagem backend (grátis)
- **Supabase**: PostgreSQL gerenciado (grátis)

---

## 🆓 Hospedagem Gratuita

### Opções de Deploy (100% Grátis)

#### Frontend (Vercel)
- ✅ **Vercel Free Tier**
  - 100 GB bandwidth/mês
  - Deploy automático via Git
  - HTTPS incluído
  - Domínio gratuito (.vercel.app)

#### Backend (Escolha uma)

**Opção 1: Railway (Recomendado)**
- ✅ **Railway Free Tier**
  - $5 de crédito/mês (suficiente para uso pessoal)
  - 512 MB RAM
  - PostgreSQL incluído
  - Deploy via Git

**Opção 2: Render**
- ✅ **Render Free Tier**
  - 750 horas/mês (suficiente)
  - 512 MB RAM
  - Sleep após inatividade (wake-up automático)
  - PostgreSQL gratuito (90 dias, depois migrar para Supabase)

#### Database (Supabase)
- ✅ **Supabase Free Tier**
  - 500 MB storage
  - 2 GB bandwidth/mês
  - PostgreSQL 15+
  - Backups automáticos
  - Dashboard completo

### Estimativa de Custos
```
Frontend (Vercel):     $0/mês
Backend (Railway):     $0/mês (dentro do free tier)
Database (Supabase):   $0/mês
TOTAL:                 $0/mês 🎉
```

---

## 🚀 Instalação

### Pré-requisitos
- .NET 8 SDK
- Node.js 18+ e npm/yarn
- PostgreSQL 15+ (ou usar Supabase)
- Git

### 1. Clone o Repositório
```bash
git clone <repository-url>
cd Financial-Project
```

### 2. Backend Setup

```bash
cd backend/FinancialControl.Api

# Restaurar dependências
dotnet restore

# Configurar connection string
# Edite appsettings.Development.json com suas credenciais

# Aplicar migrations
dotnet ef database update

# Rodar aplicação
dotnet run
```

API disponível em: `https://localhost:7001`

### 3. Frontend Setup

```bash
cd frontend

# Instalar dependências
npm install

# Configurar variáveis de ambiente
cp .env.example .env.local
# Edite .env.local com a URL da API

# Rodar aplicação
npm run dev
```

App disponível em: `http://localhost:5173`

---

## 📁 Estrutura do Projeto

```
Financial-Project/
├── backend/
│   └── FinancialControl.Api/
│       ├── Controllers/          # Endpoints da API
│       ├── Models/               # Entidades do domínio
│       ├── DTOs/                 # Data Transfer Objects
│       ├── Data/                 # DbContext e Migrations
│       ├── Services/             # Lógica de negócio
│       ├── Repositories/         # Acesso a dados
│       ├── Middleware/           # Middleware customizado
│       ├── Validators/           # FluentValidation
│       └── Program.cs            # Configuração da aplicação
├── frontend/
│   ├── src/
│   │   ├── components/          # Componentes React
│   │   │   ├── ui/             # shadcn/ui components
│   │   │   ├── auth/           # Componentes de autenticação
│   │   │   ├── dashboard/      # Componentes do dashboard
│   │   │   └── transactions/   # Componentes de transações
│   │   ├── pages/              # Páginas da aplicação
│   │   ├── services/           # API clients
│   │   ├── hooks/              # Custom hooks
│   │   ├── contexts/           # React contexts
│   │   ├── utils/              # Funções utilitárias
│   │   └── types/              # TypeScript types
│   ├── public/                 # Assets estáticos
│   └── package.json
├── docs/
│   ├── ROADMAP.md              # Roadmap do projeto
│   ├── TASKLIST.md             # Lista de tarefas
│   ├── ADR/                    # Architecture Decision Records
│   └── API.md                  # Documentação da API
├── docker-compose.yml          # Configuração Docker
├── .github/
│   └── workflows/              # GitHub Actions CI/CD
└── README.md                   # Este arquivo
```

---

## 📚 Documentação

- **[ROADMAP.md](docs/ROADMAP.md)**: Planejamento de releases e features
- **[TASKLIST.md](docs/TASKLIST.md)**: Lista detalhada de tarefas
- **[API.md](docs/API.md)**: Documentação completa da API
- **[ADR/](docs/ADR/)**: Decisões arquiteturais importantes

---

## 🗺️ Roadmap

### ✅ Fase 1: MVP (v0.1.0) - 2 semanas
- Autenticação básica
- CRUD de transações
- Dashboard simples
- Deploy inicial

### 🚧 Fase 2: Core Features (v0.2.0) - 2 semanas
- Categorias customizáveis
- Receitas fixas
- Relatórios por período
- Gráficos básicos

### 📋 Fase 3: Analytics (v0.3.0) - 2 semanas
- Gráficos avançados
- Exportação de dados
- Filtros avançados
- Orçamentos

### 🔮 Fase 4: Advanced (v1.0.0) - 3 semanas
- Compartilhamento entre usuários
- Notificações
- Metas financeiras
- App mobile (PWA)

Veja detalhes completos em [ROADMAP.md](docs/ROADMAP.md)

---

## 🤝 Contribuindo

Este é um projeto pessoal, mas sugestões são bem-vindas!

---

## 📄 Licença

MIT License - Uso livre para fins pessoais e educacionais.

---

## 👨‍💻 Autor

**Eduardo Pereira**
- Projeto criado para controle financeiro pessoal
- Stack moderna com hospedagem gratuita

---

## 🙏 Agradecimentos

- Comunidade .NET
- Comunidade React
- Provedores de hospedagem gratuita (Vercel, Railway, Supabase)

---

**Desenvolvido com ❤️ e ☕**
