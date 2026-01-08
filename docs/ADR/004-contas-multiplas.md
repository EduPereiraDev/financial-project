# ADR 004: Contas Múltiplas (Pessoal + Compartilhada)

**Status**: Aprovado  
**Data**: Janeiro 2026  
**Decisores**: Eduardo Pereira  
**Contexto**: Suporte a múltiplas contas (pessoal e compartilhada)

---

## Contexto e Problema

O usuário precisa gerenciar:
1. **Conta Pessoal**: Gastos individuais
2. **Conta Compartilhada**: Gastos do casal
3. **Visualizações Configuráveis**: Filtrar por tipo de conta

**Requisitos**:
- Cada usuário tem sua conta pessoal
- Usuários podem criar/participar de contas compartilhadas
- Filtros para visualizar: "Só minha", "Só compartilhada", "Todas"
- Relatórios separados por conta
- Categorias podem ser específicas ou compartilhadas

---

## Decisão: Modelo de Contas Múltiplas

### Estrutura de Dados

```csharp
public class Account
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public AccountType Type { get; set; } // Personal, Shared
    public Guid OwnerId { get; set; }
    public DateTime CreatedAt { get; set; }
}

public enum AccountType
{
    Personal,   // Conta pessoal (default)
    Shared      // Conta compartilhada
}

public class AccountMember
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public Guid UserId { get; set; }
    public AccountRole Role { get; set; } // Owner, Editor, Viewer
    public DateTime JoinedAt { get; set; }
}

public enum AccountRole
{
    Owner,   // Controle total
    Editor,  // Pode adicionar/editar transações
    Viewer   // Apenas visualizar
}

public class Transaction
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; } // ← NOVO: Pertence a uma conta
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }
}
```

---

## Fluxo de Uso

### 1. Criação de Usuário
```
Usuário se registra
  ↓
Sistema cria automaticamente:
  - Conta Pessoal "Minha Conta"
  - Categorias padrão para essa conta
```

### 2. Criar Conta Compartilhada
```
Usuário clica "Nova Conta Compartilhada"
  ↓
Preenche: Nome ("Conta do Casal")
  ↓
Sistema cria conta com usuário como Owner
  ↓
Usuário convida namorada por email
  ↓
Namorada aceita convite
  ↓
Ambos têm acesso à conta compartilhada
```

### 3. Adicionar Transação
```
Usuário seleciona conta:
  [Minha Conta ▼]  ou  [Conta do Casal ▼]
  ↓
Adiciona transação
  ↓
Transação fica vinculada à conta selecionada
```

### 4. Visualizar Transações
```
Filtro de Conta:
  [ ] Minha Conta (pessoal)
  [ ] Conta do Casal (compartilhada)
  [x] Todas as contas
  ↓
Dashboard mostra transações filtradas
```

---

## Interface do Usuário

### Seletor de Conta (Global)
```
┌─────────────────────────────────────────┐
│  📊 Financial Control                   │
├─────────────────────────────────────────┤
│  Conta: [Todas ▼]                       │
│         ├─ Minha Conta (pessoal)        │
│         ├─ Conta do Casal (compartilh.) │
│         └─ Todas as contas              │
└─────────────────────────────────────────┘
```

### Dashboard com Filtro
```
┌─────────────────────────────────────────┐
│  Dashboard                              │
├─────────────────────────────────────────┤
│  Visualizando: [Todas as contas ▼]     │
│                                         │
│  💰 Resumo                              │
│  Receitas:  R$ 8.500,00                │
│  Despesas:  R$ 6.200,00                │
│  Saldo:     R$ 2.300,00                │
│                                         │
│  📊 Por Conta:                          │
│  • Minha Conta:      R$ 1.200,00       │
│  • Conta do Casal:   R$ 1.100,00       │
└─────────────────────────────────────────┘
```

### Adicionar Transação
```
┌─────────────────────────────────────────┐
│  Nova Transação                         │
├─────────────────────────────────────────┤
│  Conta: [Conta do Casal ▼]             │
│         ├─ Minha Conta                  │
│         └─ Conta do Casal               │
│                                         │
│  Descrição: [Mercado]                   │
│  Valor: [R$ 350,00]                     │
│  Categoria: [Alimentação ▼]             │
│  Data: [08/01/2026]                     │
│                                         │
│  [Cancelar]  [Salvar]                   │
└─────────────────────────────────────────┘
```

### Página de Contas
```
┌─────────────────────────────────────────┐
│  Minhas Contas                          │
├─────────────────────────────────────────┤
│  👤 Minha Conta (Pessoal)               │
│  Apenas você                            │
│  Saldo: R$ 1.200,00                     │
│  [Ver Detalhes]                         │
│                                         │
│  👫 Conta do Casal (Compartilhada)      │
│  Você e Maria Silva                     │
│  Saldo: R$ 1.100,00                     │
│  [Ver Detalhes] [Gerenciar Membros]    │
│                                         │
│  [+ Nova Conta Compartilhada]           │
└─────────────────────────────────────────┘
```

---

## Regras de Negócio

### Conta Pessoal
- ✅ Criada automaticamente no registro
- ✅ Apenas o dono tem acesso
- ✅ Não pode ser deletada
- ✅ Não pode ser compartilhada

### Conta Compartilhada
- ✅ Criada manualmente pelo usuário
- ✅ Owner pode convidar outros usuários
- ✅ Owner pode remover membros
- ✅ Owner pode deletar a conta
- ✅ Membros podem sair da conta
- ✅ Todos os membros veem as mesmas transações

### Permissões
- **Owner**: Tudo (adicionar, editar, deletar, gerenciar membros)
- **Editor**: Adicionar e editar transações
- **Viewer**: Apenas visualizar

### Categorias
- Categorias são específicas de cada conta
- Ao criar conta compartilhada, copia categorias padrão
- Membros podem criar categorias na conta compartilhada

---

## Filtros e Visualizações

### Opções de Filtro
1. **Conta Específica**: Mostra apenas transações dessa conta
2. **Todas as Contas**: Mostra transações de todas as contas
3. **Múltiplas Contas**: Selecionar várias contas

### Relatórios
- Relatórios podem ser filtrados por conta
- Gráficos mostram breakdown por conta
- Exportação pode ser por conta ou consolidada

### Dashboard
- Cards de resumo mostram totais por conta
- Gráficos podem ser filtrados por conta
- Indicador visual de qual conta está ativa

---

## Implementação Backend

### Endpoints

```csharp
// Contas
GET    /api/accounts                    // Listar minhas contas
POST   /api/accounts                    // Criar conta compartilhada
GET    /api/accounts/{id}               // Detalhes da conta
PUT    /api/accounts/{id}               // Atualizar conta
DELETE /api/accounts/{id}               // Deletar conta

// Membros
GET    /api/accounts/{id}/members       // Listar membros
POST   /api/accounts/{id}/members       // Convidar membro
PUT    /api/accounts/{id}/members/{uid} // Atualizar permissão
DELETE /api/accounts/{id}/members/{uid} // Remover membro

// Transações (com filtro de conta)
GET    /api/transactions?accountId={id} // Filtrar por conta
GET    /api/transactions?accountIds=1,2 // Múltiplas contas
POST   /api/transactions                // Criar (requer accountId)
```

### Validações

```csharp
public class TransactionValidator : AbstractValidator<CreateTransactionDto>
{
    public TransactionValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("Conta é obrigatória");
            
        RuleFor(x => x.AccountId)
            .MustAsync(UserHasAccessToAccount)
            .WithMessage("Você não tem acesso a esta conta");
    }
}
```

---

## Implementação Frontend

### Context de Conta Ativa

```typescript
interface AccountContextType {
  accounts: Account[];
  activeAccount: Account | null;
  setActiveAccount: (account: Account | null) => void;
  filterMode: 'single' | 'all';
}

const AccountContext = createContext<AccountContextType>();

// Uso
const { activeAccount, setActiveAccount } = useAccount();
```

### Hook de Transações com Filtro

```typescript
const useTransactions = (accountFilter?: string) => {
  const { activeAccount } = useAccount();
  
  const accountId = accountFilter || activeAccount?.id;
  
  return useQuery(['transactions', accountId], () => 
    api.getTransactions({ accountId })
  );
};
```

### Componente de Seletor

```typescript
const AccountSelector = () => {
  const { accounts, activeAccount, setActiveAccount } = useAccount();
  
  return (
    <Select value={activeAccount?.id} onChange={setActiveAccount}>
      <option value="all">Todas as contas</option>
      {accounts.map(account => (
        <option key={account.id} value={account.id}>
          {account.name} ({account.type})
        </option>
      ))}
    </Select>
  );
};
```

---

## Migração de Dados

Para usuários existentes:

```sql
-- 1. Criar tabela de contas
CREATE TABLE accounts (
  id UUID PRIMARY KEY,
  name VARCHAR(255),
  type VARCHAR(50),
  owner_id UUID,
  created_at TIMESTAMP
);

-- 2. Criar conta pessoal para cada usuário
INSERT INTO accounts (id, name, type, owner_id, created_at)
SELECT 
  gen_random_uuid(),
  'Minha Conta',
  'Personal',
  id,
  created_at
FROM users;

-- 3. Associar transações existentes à conta pessoal
UPDATE transactions t
SET account_id = (
  SELECT a.id 
  FROM accounts a 
  WHERE a.owner_id = t.user_id 
  AND a.type = 'Personal'
);
```

---

## Consequências

### Positivas ✅
- Separação clara entre gastos pessoais e compartilhados
- Flexibilidade para criar múltiplas contas
- Relatórios mais precisos
- Privacidade mantida (conta pessoal)
- Colaboração facilitada (conta compartilhada)

### Negativas ⚠️
- Complexidade adicional no código
- Usuário precisa selecionar conta ao adicionar transação
- Mais dados para gerenciar

### Mitigações
- Conta padrão selecionada automaticamente
- UI intuitiva para trocar de conta
- Indicadores visuais claros de qual conta está ativa

---

## Roadmap de Implementação

### Fase 1: Estrutura Básica (Semana 7)
- [ ] Modelo de dados (Account, AccountMember)
- [ ] Migration para criar tabelas
- [ ] Criar conta pessoal automaticamente no registro
- [ ] Endpoints básicos de contas

### Fase 2: Contas Compartilhadas (Semana 8)
- [ ] Criar conta compartilhada
- [ ] Convidar membros
- [ ] Gerenciar permissões
- [ ] Aceitar/rejeitar convites

### Fase 3: Filtros e Visualizações (Semana 9)
- [ ] Seletor de conta no frontend
- [ ] Filtrar transações por conta
- [ ] Dashboard com breakdown por conta
- [ ] Relatórios por conta

### Fase 4: Melhorias (Semana 10)
- [ ] Indicadores visuais
- [ ] Conta padrão configurável
- [ ] Histórico de atividades por conta
- [ ] Notificações de convites

---

## Exemplos de Uso

### Caso 1: Gastos Pessoais
```
Eduardo adiciona:
  Conta: "Minha Conta"
  Descrição: "Presente para amigo"
  Valor: R$ 150,00
  
→ Apenas Eduardo vê essa transação
```

### Caso 2: Gastos do Casal
```
Eduardo adiciona:
  Conta: "Conta do Casal"
  Descrição: "Mercado"
  Valor: R$ 350,00
  
→ Eduardo e Maria veem essa transação
```

### Caso 3: Visualização Consolidada
```
Eduardo seleciona: "Todas as contas"
Dashboard mostra:
  - Transações da "Minha Conta"
  - Transações da "Conta do Casal"
  - Total consolidado
```

### Caso 4: Visualização Específica
```
Eduardo seleciona: "Minha Conta"
Dashboard mostra:
  - Apenas transações pessoais
  - Relatórios da conta pessoal
```

---

## Métricas de Sucesso

- ✅ 100% dos usuários têm conta pessoal
- ✅ 80% dos casais criam conta compartilhada
- ✅ Usuários conseguem trocar de conta em < 2 cliques
- ✅ Zero confusão sobre qual conta está ativa
- ✅ Relatórios corretos por conta

---

## Referências

- [Multi-tenancy Patterns](https://docs.microsoft.com/en-us/azure/architecture/patterns/multi-tenancy)
- [Shared Data Architecture](https://martinfowler.com/articles/patterns-of-distributed-systems/)

---

**Última atualização**: Janeiro 2026  
**Status**: Aprovado para implementação na Fase 4 (v0.4.0)
