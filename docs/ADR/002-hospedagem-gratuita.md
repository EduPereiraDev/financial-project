# ADR 002: Estratégia de Hospedagem Gratuita

**Status**: Aprovado  
**Data**: Janeiro 2026  
**Decisores**: Eduardo Pereira  
**Contexto**: Definir plataformas de hospedagem com custo zero

---

## Contexto e Problema

O requisito principal é **zero custo de hospedagem**. Precisamos hospedar:
1. Frontend (React SPA)
2. Backend (.NET Web API)
3. Banco de dados (PostgreSQL)

Com requisitos de:
- Disponibilidade razoável (não precisa ser 99.99%)
- Performance aceitável para 2 usuários
- HTTPS incluído
- Deploy automático via Git

## Decisão

### Frontend: Vercel ✅

**Plano**: Free Tier

**Recursos Incluídos**:
- 100 GB bandwidth/mês
- Builds ilimitados
- Deploy automático (Git push)
- HTTPS automático
- Domínio gratuito (.vercel.app)
- CDN global
- Preview deployments

**Limites**:
- 100 GB bandwidth (mais que suficiente)
- 100 GB-hours de execução

**Por que Vercel**:
- Otimizado para React/Vite
- Deploy em segundos
- Melhor DX (Developer Experience)
- Sem cold starts
- Analytics incluído

---

### Backend: Railway (Primário) ✅

**Plano**: Hobby Plan (Free)

**Recursos Incluídos**:
- $5 de crédito/mês (renova mensalmente)
- 512 MB RAM
- 1 GB storage
- PostgreSQL incluído
- Deploy via Git
- HTTPS automático
- Variáveis de ambiente

**Consumo Estimado**:
- Backend API: ~$3/mês
- PostgreSQL: ~$1/mês
- **Total**: ~$4/mês (dentro do crédito)

**Por que Railway**:
- PostgreSQL incluído (não precisa de serviço separado)
- Interface simples
- Logs em tempo real
- Métricas de uso
- Sem cold starts (sempre ativo)

---

### Backend: Render (Alternativo) ⚠️

**Plano**: Free Tier

**Recursos Incluídos**:
- 750 horas/mês (suficiente)
- 512 MB RAM
- Deploy via Git
- HTTPS automático

**Limitações**:
- **Cold starts**: Sleep após 15 min de inatividade
- Wake-up: ~30 segundos
- PostgreSQL free: Apenas 90 dias

**Quando usar**:
- Se Railway atingir limite
- Backup/redundância
- Testes

---

### Banco de Dados: Supabase ✅

**Plano**: Free Tier

**Recursos Incluídos**:
- 500 MB storage
- 2 GB bandwidth/mês
- PostgreSQL 15+
- Backups automáticos (7 dias)
- Dashboard completo
- API REST automática (opcional)
- Realtime subscriptions (opcional)

**Limites**:
- 500 MB storage (suficiente para milhares de transações)
- 2 GB bandwidth
- Pausa após 1 semana de inatividade (reativa automaticamente)

**Por que Supabase**:
- PostgreSQL gerenciado
- Dashboard visual
- Backups incluídos
- Pode usar API REST se quiser (futuro)
- Migração fácil se precisar

---

## Estratégia de Deploy

### Configuração Recomendada

```
┌─────────────────────────────────────────┐
│  Vercel (Frontend)                      │
│  - React Build                          │
│  - CDN Global                           │
│  - HTTPS                                │
└──────────────┬──────────────────────────┘
               │ API Calls
┌──────────────┴──────────────────────────┐
│  Railway (Backend)                      │
│  - .NET 8 API                           │
│  - JWT Auth                             │
│  - HTTPS                                │
└──────────────┬──────────────────────────┘
               │ PostgreSQL
┌──────────────┴──────────────────────────┐
│  Supabase (Database)                    │
│  - PostgreSQL 15                        │
│  - Backups                              │
└─────────────────────────────────────────┘
```

### Configuração Alternativa (Tudo no Railway)

```
┌─────────────────────────────────────────┐
│  Vercel (Frontend)                      │
└──────────────┬──────────────────────────┘
               │
┌──────────────┴──────────────────────────┐
│  Railway                                │
│  ├── Backend (.NET API)                 │
│  └── PostgreSQL                         │
└─────────────────────────────────────────┘
```

**Vantagem**: Menos serviços para gerenciar  
**Desvantagem**: Usa mais crédito Railway

---

## Consequências

### Positivas ✅

1. **Custo Zero**: Todos os serviços são gratuitos
2. **Deploy Automático**: Git push → Deploy
3. **HTTPS Incluído**: Segurança sem configuração
4. **Escalabilidade**: Fácil upgrade se necessário
5. **Backups**: Supabase faz backup automático
6. **Monitoramento**: Dashboards incluídos

### Negativas ⚠️

1. **Limites de Uso**: Precisa monitorar consumo
2. **Cold Starts**: Render tem (Railway não)
3. **Suporte**: Limitado em planos gratuitos
4. **Vendor Lock-in**: Moderado (mas mitigável)

---

## Monitoramento de Limites

### Vercel
- **Bandwidth**: 100 GB/mês
- **Monitorar**: Dashboard Vercel
- **Alerta**: Email automático em 80%

### Railway
- **Crédito**: $5/mês
- **Monitorar**: Dashboard Railway (uso em tempo real)
- **Alerta**: Email automático em $4

### Supabase
- **Storage**: 500 MB
- **Bandwidth**: 2 GB/mês
- **Monitorar**: Dashboard Supabase
- **Alerta**: Email automático em 80%

---

## Plano de Contingência

### Se ultrapassar limites:

1. **Vercel Bandwidth**:
   - Otimizar assets (compressão, lazy loading)
   - Usar CDN externo para imagens grandes

2. **Railway Crédito**:
   - Migrar para Render (com cold starts)
   - Otimizar queries (reduzir uso de CPU)
   - Adicionar cache (Redis gratuito: Upstash)

3. **Supabase Storage**:
   - Limpar dados antigos (após 2 anos)
   - Arquivar transações antigas
   - Migrar para Railway PostgreSQL

---

## Estimativa de Uso (2 Usuários)

### Transações por Mês
- 2 usuários × 30 transações/mês = 60 transações
- Tamanho médio: ~500 bytes/transação
- **Storage/mês**: 30 KB

### Requests por Mês
- 2 usuários × 10 acessos/dia × 30 dias = 600 acessos
- 600 acessos × 5 requests/acesso = 3.000 requests
- **Bandwidth**: ~50 MB/mês

### Conclusão
**Muito abaixo dos limites!** ✅

---

## Alternativas Consideradas

### 1. Netlify (Frontend)
- **Prós**: Similar ao Vercel
- **Contras**: Menos otimizado para Vite
- **Motivo da rejeição**: Vercel tem melhor DX

### 2. Heroku (Backend)
- **Prós**: Conhecido, maduro
- **Contras**: Free tier removido em 2022
- **Motivo da rejeição**: Não é mais gratuito

### 3. AWS Free Tier
- **Prós**: 12 meses grátis, robusto
- **Contras**: Complexo, pode gerar custos inesperados
- **Motivo da rejeição**: Overhead desnecessário

### 4. MongoDB Atlas (Database)
- **Prós**: 512 MB grátis
- **Contras**: NoSQL (preferimos SQL)
- **Motivo da rejeição**: PostgreSQL é melhor para finanças

---

## Migração Futura

Se precisar escalar ou pagar:

### Opção 1: Upgrade nos mesmos serviços
- Vercel Pro: $20/mês
- Railway Pro: $5/mês + uso
- Supabase Pro: $25/mês

### Opção 2: Migrar para VPS
- DigitalOcean Droplet: $6/mês
- Hospedar tudo em um servidor
- Mais controle, mais trabalho

### Opção 3: Cloud Provider
- AWS/GCP/Azure
- Mais caro, mais recursos
- Apenas se escalar muito

---

## Checklist de Deploy

### Vercel (Frontend)
- [ ] Criar conta Vercel
- [ ] Conectar repositório GitHub
- [ ] Configurar `VITE_API_URL`
- [ ] Deploy automático configurado
- [ ] Domínio customizado (opcional)

### Railway (Backend)
- [ ] Criar conta Railway
- [ ] Criar novo projeto
- [ ] Adicionar serviço: .NET API
- [ ] Adicionar serviço: PostgreSQL
- [ ] Configurar variáveis de ambiente
- [ ] Deploy automático configurado

### Supabase (Database)
- [ ] Criar conta Supabase
- [ ] Criar novo projeto
- [ ] Obter connection string
- [ ] Configurar no Railway
- [ ] Aplicar migrations
- [ ] Configurar backups

---

## Métricas de Sucesso

1. **Uptime**: > 99% (monitorar com UptimeRobot grátis)
2. **Custo**: $0/mês
3. **Performance**: Load time < 2s
4. **Deploy**: < 5 minutos

---

## Referências

- [Vercel Pricing](https://vercel.com/pricing)
- [Railway Pricing](https://railway.app/pricing)
- [Supabase Pricing](https://supabase.com/pricing)
- [Render Pricing](https://render.com/pricing)

---

**Próxima Revisão**: Após 1 mês de uso (avaliar consumo real)
