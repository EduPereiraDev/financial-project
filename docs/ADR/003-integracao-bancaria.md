# ADR 003: Integração Bancária Automática

**Status**: Planejado (Fase 2+)  
**Data**: Janeiro 2026  
**Decisores**: Eduardo Pereira  
**Contexto**: Integração com bancos para importação automática de transações

---

## Contexto e Problema

Cadastrar manualmente todas as transações pode ser trabalhoso. A integração bancária permitiria:
- Importação automática de entradas e saídas
- Sincronização em tempo real
- Redução de erros de digitação
- Economia de tempo

**Desafio**: Como fazer isso de forma gratuita e segura?

---

## Opções de Integração Bancária no Brasil

### 1. Open Banking (Open Finance Brasil) ✅ RECOMENDADO

**O que é**: Sistema regulado pelo Banco Central que permite compartilhamento seguro de dados bancários entre instituições autorizadas.

**Como funciona**:
1. Usuário autoriza acesso aos dados bancários
2. Aplicativo se conecta via APIs padronizadas
3. Transações são importadas automaticamente
4. Renovação de consentimento periódica (90 dias)

**Bancos Participantes** (principais):
- Nubank ✅
- Itaú ✅
- Bradesco ✅
- Banco do Brasil ✅
- Santander ✅
- Caixa ✅
- Inter ✅
- C6 Bank ✅
- Mais de 800+ instituições

**Vantagens**:
- ✅ Regulado pelo Banco Central (seguro)
- ✅ APIs padronizadas
- ✅ Gratuito para consumidores
- ✅ Cobertura ampla de bancos
- ✅ Dados em tempo real

**Desvantagens**:
- ⚠️ Requer certificação (custo para produção)
- ⚠️ Complexidade de implementação
- ⚠️ Renovação de consentimento a cada 90 dias
- ⚠️ Sandbox gratuito, produção tem custos

---

### 2. Pluggy (Agregador) ✅ MELHOR PARA MVP

**O que é**: Plataforma que simplifica integração com Open Banking e outros bancos.

**Plano Gratuito**:
- 100 conexões/mês (suficiente para uso pessoal)
- Acesso a 200+ instituições
- APIs simplificadas
- Sandbox ilimitado

**Como funciona**:
```
Seu App → Pluggy API → Open Banking → Bancos
```

**Vantagens**:
- ✅ **Gratuito até 100 conexões/mês**
- ✅ Implementação simplificada
- ✅ SDK em várias linguagens (.NET, JS)
- ✅ Suporte a Open Banking + scraping
- ✅ Documentação completa
- ✅ Não precisa de certificação própria

**Desvantagens**:
- ⚠️ Dependência de terceiro
- ⚠️ Limite de 100 conexões (ok para 2 usuários)

**Custo**:
- Free: 100 conexões/mês → **$0**
- Starter: 500 conexões/mês → $49/mês
- Growth: 2000 conexões/mês → $149/mês

**Para 2 usuários**: Free tier é suficiente! ✅

---

### 3. Belvo (Alternativa)

Similar ao Pluggy, mas:
- 50 conexões/mês no free tier
- Foco em América Latina
- Menos bancos brasileiros

**Veredicto**: Pluggy é melhor para Brasil

---

### 4. Scraping Direto (NÃO RECOMENDADO)

**O que é**: Automatizar login no site do banco e extrair dados.

**Por que NÃO**:
- ❌ Viola termos de uso dos bancos
- ❌ Inseguro (precisa armazenar senha)
- ❌ Quebra facilmente (mudanças no site)
- ❌ Ilegal em alguns casos
- ❌ Bloqueios frequentes

---

## Decisão: Estratégia em Fases

### Fase 1 (MVP): Cadastro Manual ✅
**Status**: Implementar primeiro  
**Prazo**: Semanas 1-2

- Cadastro manual de transações
- Foco em validar o produto
- Zero custos
- Simplicidade

**Justificativa**: Validar se o app atende a necessidade antes de investir em integrações complexas.

---

### Fase 2: Importação via CSV ✅
**Status**: Após MVP  
**Prazo**: Semana 5-6

**Implementação**:
- Upload de extrato CSV do banco
- Parser para formatos comuns (Nubank, Itaú, etc)
- Mapeamento automático de categorias
- Deduplicação de transações

**Vantagens**:
- ✅ Gratuito
- ✅ Simples de implementar
- ✅ Funciona com qualquer banco
- ✅ Usuário mantém controle

**Desvantagens**:
- ⚠️ Não é automático (precisa baixar CSV)
- ⚠️ Formatos variam por banco

**Esforço**: 1 semana de desenvolvimento

---

### Fase 3: Integração com Pluggy (Open Banking) 🚀
**Status**: Futuro (após validação)  
**Prazo**: v1.1+

**Implementação**:

#### Backend (.NET)
```csharp
// Instalar SDK
dotnet add package Pluggy.SDK

// Serviço de integração
public class BankIntegrationService
{
    private readonly PluggyClient _pluggy;
    
    public async Task<string> CreateConnectToken(Guid userId)
    {
        // Gera token para conectar banco
        return await _pluggy.CreateConnectToken(userId.ToString());
    }
    
    public async Task<List<Transaction>> SyncTransactions(Guid userId)
    {
        // Busca transações dos últimos 30 dias
        var accounts = await _pluggy.GetAccounts(userId.ToString());
        var transactions = new List<Transaction>();
        
        foreach (var account in accounts)
        {
            var bankTransactions = await _pluggy.GetTransactions(
                account.Id, 
                from: DateTime.Now.AddDays(-30)
            );
            
            transactions.AddRange(MapToInternalTransactions(bankTransactions));
        }
        
        return transactions;
    }
}
```

#### Frontend (React)
```typescript
// Componente de conexão bancária
const BankConnect = () => {
  const connectBank = async () => {
    // 1. Obter token do backend
    const { connectToken } = await api.post('/bank/connect-token');
    
    // 2. Abrir widget Pluggy
    const pluggy = new PluggyConnect({
      connectToken,
      onSuccess: (itemData) => {
        // Banco conectado com sucesso
        syncTransactions();
      }
    });
    
    pluggy.open();
  };
  
  const syncTransactions = async () => {
    // Sincronizar transações
    await api.post('/bank/sync');
  };
  
  return (
    <Button onClick={connectBank}>
      Conectar Banco
    </Button>
  );
};
```

**Fluxo do Usuário**:
1. Clica em "Conectar Banco"
2. Seleciona seu banco (Nubank, Itaú, etc)
3. Faz login no banco (via Open Banking)
4. Autoriza compartilhamento de dados
5. Transações são importadas automaticamente
6. Renovação de consentimento a cada 90 dias

**Custo**: $0/mês (dentro do free tier)

---

## Arquitetura Proposta (Fase 3)

```
┌─────────────────────────────────────────────────┐
│  Frontend (React)                               │
│  - Botão "Conectar Banco"                       │
│  - Widget Pluggy                                │
└──────────────────┬──────────────────────────────┘
                   │
┌──────────────────┴──────────────────────────────┐
│  Backend (.NET API)                             │
│  - BankIntegrationService                       │
│  - TransactionMappingService                    │
│  - DeduplicationService                         │
└──────────────────┬──────────────────────────────┘
                   │
┌──────────────────┴──────────────────────────────┐
│  Pluggy API                                     │
│  - Connect Widget                               │
│  - Accounts API                                 │
│  - Transactions API                             │
└──────────────────┬──────────────────────────────┘
                   │
┌──────────────────┴──────────────────────────────┐
│  Open Banking / Bancos                          │
│  - Nubank, Itaú, Bradesco, etc                  │
└─────────────────────────────────────────────────┘
```

---

## Funcionalidades da Integração

### 1. Conexão Inicial
- Usuário autoriza acesso ao banco
- Importação histórica (últimos 90 dias)
- Categorização automática (ML)

### 2. Sincronização Automática
- Webhook do Pluggy notifica novas transações
- Sincronização diária automática
- Deduplicação inteligente

### 3. Mapeamento de Categorias
```csharp
public class CategoryMapper
{
    public Category MapFromDescription(string description)
    {
        // Regras de mapeamento
        if (description.Contains("UBER") || description.Contains("99"))
            return Category.Transport;
            
        if (description.Contains("IFOOD") || description.Contains("RESTAURANTE"))
            return Category.Food;
            
        // ML para casos complexos
        return _mlService.PredictCategory(description);
    }
}
```

### 4. Deduplicação
- Evita importar transações já cadastradas manualmente
- Compara: data, valor, descrição
- Merge inteligente de dados

### 5. Renovação de Consentimento
- Notificação 7 dias antes de expirar
- Fluxo simplificado de renovação
- Histórico de consentimentos

---

## Segurança e Privacidade

### Dados Armazenados
- ❌ **NÃO armazenamos**: Senhas bancárias
- ❌ **NÃO armazenamos**: Tokens de acesso permanentes
- ✅ **Armazenamos**: ID da conexão Pluggy
- ✅ **Armazenamos**: Transações importadas (criptografadas)

### Conformidade
- ✅ LGPD compliant
- ✅ Open Banking regulado pelo Banco Central
- ✅ Criptografia em trânsito (HTTPS)
- ✅ Criptografia em repouso (PostgreSQL)

### Auditoria
- Log de todas as sincronizações
- Histórico de consentimentos
- Opção de desconectar banco a qualquer momento

---

## Estimativa de Custos

### Cenário: 2 Usuários

**Conexões/Mês**:
- 2 usuários × 1 banco cada = 2 conexões iniciais
- Sincronizações diárias: 2 × 30 = 60 sincronizações
- **Total**: ~62 conexões/mês

**Custo Pluggy**: $0 (dentro do free tier de 100)

### Cenário: 10 Usuários

**Conexões/Mês**:
- 10 usuários × 1 banco = 10 conexões
- Sincronizações: 10 × 30 = 300 sincronizações
- **Total**: ~310 conexões/mês

**Custo Pluggy**: $49/mês (plano Starter)

---

## Alternativas Consideradas

### 1. Implementar Open Banking Direto
- **Prós**: Sem dependência de terceiros
- **Contras**: Certificação cara (~R$ 5.000), complexo
- **Veredicto**: Não vale para uso pessoal

### 2. Usar Plaid (internacional)
- **Prós**: Líder global
- **Contras**: Poucos bancos brasileiros, caro
- **Veredicto**: Não adequado para Brasil

### 3. Não fazer integração
- **Prós**: Simples, gratuito
- **Contras**: Trabalho manual
- **Veredicto**: Ok para MVP, mas limitado

---

## Roadmap de Implementação

### Fase 1: MVP (Semanas 1-2) ✅
- [ ] Cadastro manual de transações
- [ ] CRUD completo
- [ ] Sem integração bancária

### Fase 2: Import CSV (Semanas 5-6)
- [ ] Upload de arquivo CSV
- [ ] Parser para formatos comuns
- [ ] Mapeamento de categorias
- [ ] Deduplicação

### Fase 3: Pluggy Integration (v1.1+)
- [ ] Criar conta Pluggy (free tier)
- [ ] Implementar backend integration
- [ ] Widget de conexão no frontend
- [ ] Sincronização automática
- [ ] Categorização com ML
- [ ] Deduplicação inteligente
- [ ] Renovação de consentimento

### Fase 4: Melhorias (v1.2+)
- [ ] Suporte a múltiplos bancos por usuário
- [ ] Regras customizadas de categorização
- [ ] Notificações de novas transações
- [ ] Análise de gastos por merchant

---

## Riscos e Mitigações

### Risco 1: Limite do Free Tier
- **Impacto**: Precisar pagar após crescer
- **Mitigação**: Monitorar uso, otimizar sincronizações
- **Plano B**: Migrar para CSV-only se necessário

### Risco 2: Mudanças na API Pluggy
- **Impacto**: Breaking changes
- **Mitigação**: Versionar API, testes automatizados
- **Plano B**: Manter import CSV como fallback

### Risco 3: Banco não suportado
- **Impacto**: Usuário não consegue conectar
- **Mitigação**: Oferecer import CSV
- **Plano B**: Cadastro manual sempre disponível

### Risco 4: Expiração de Consentimento
- **Impacto**: Sincronização para
- **Mitigação**: Notificações proativas, renovação fácil
- **Plano B**: Cadastro manual temporário

---

## Métricas de Sucesso

### Fase 2 (CSV)
- ✅ 80% das transações importadas corretamente
- ✅ < 5% de duplicatas
- ✅ Suporte a 5+ formatos de banco

### Fase 3 (Pluggy)
- ✅ Conexão bem-sucedida em < 2 minutos
- ✅ 95% de precisão na categorização
- ✅ < 1% de duplicatas
- ✅ Sincronização diária automática
- ✅ Taxa de renovação de consentimento > 80%

---

## Recursos e Referências

### Pluggy
- [Documentação](https://docs.pluggy.ai/)
- [Pricing](https://pluggy.ai/pricing)
- [SDK .NET](https://github.com/pluggyai/pluggy-dotnet)
- [Playground](https://dashboard.pluggy.ai/)

### Open Banking Brasil
- [Portal Oficial](https://openbankingbrasil.org.br/)
- [Bancos Participantes](https://openbanking.bcb.gov.br/)
- [Especificações Técnicas](https://openbanking-brasil.github.io/areadesenvolvedor/)

### Alternativas
- [Belvo](https://belvo.com/)
- [Yapily](https://www.yapily.com/)
- [Mono](https://mono.co/)

---

## Decisão Final

### Para MVP (v0.1-0.3): Cadastro Manual + CSV ✅
- Implementar primeiro
- Validar produto
- Zero custos
- Simples e rápido

### Para v1.1+: Adicionar Pluggy ✅
- Após validação do produto
- Quando tiver usuários ativos
- Dentro do free tier (2 usuários)
- Experiência premium

### Não fazer agora: Open Banking direto ❌
- Muito complexo
- Custo alto de certificação
- Overhead desnecessário para uso pessoal

---

## Próximos Passos

1. ✅ Implementar MVP com cadastro manual
2. ✅ Adicionar export/import CSV (Fase 2)
3. ⏳ Avaliar necessidade de integração automática
4. ⏳ Se sim, criar conta Pluggy e implementar (Fase 3)

---

**Última atualização**: Janeiro 2026  
**Próxima revisão**: Após MVP (v0.1.0)  
**Status**: Planejado para v1.1+
