# 🚀 Guia de Deploy - Financial Control

## ✅ Status Atual

- ✅ Banco Supabase: 16 tabelas criadas
- ✅ RLS: Desabilitado (segurança no backend)
- ✅ Frontend: PWA completo com ícones
- ✅ Backend: .NET 9 com JWT

---

## 📋 Passos Manuais Necessários

### 1️⃣ Configurar Connection String do Supabase

**Onde encontrar:**
1. Acesse [Supabase Dashboard](https://app.supabase.com)
2. Vá em **Settings** → **Database**
3. Copie a **Connection String** (Pooler)

**Formato:**
```
postgresql://postgres.[PROJECT-REF]:[PASSWORD]@aws-0-us-east-1.pooler.supabase.com:6543/postgres
```

**Atualizar em:**
- `backend/FinancialControl.Api/appsettings.Production.json`
- Substitua `YOUR_PROJECT_REF` e `YOUR_SUPABASE_PASSWORD`

**Ou use variáveis de ambiente (RECOMENDADO):**
```bash
DATABASE_URL=Host=aws-0-us-east-1.pooler.supabase.com;Port=6543;Database=postgres;Username=postgres.YOUR_PROJECT_REF;Password=YOUR_PASSWORD;SSL Mode=Require;Trust Server Certificate=true
```

---

### 2️⃣ Deploy do Backend

**Opções:**

#### Opção A: Railway (RECOMENDADO)
1. Acesse [Railway](https://railway.app)
2. **New Project** → **Deploy from GitHub**
3. Selecione o repositório
4. Configure:
   - **Root Directory**: `backend/FinancialControl.Api`
   - **Start Command**: `dotnet run`
5. Adicione variáveis de ambiente:
   ```
   DATABASE_URL=<connection-string-supabase>
   ASPNETCORE_ENVIRONMENT=Production
   ```
6. Deploy automático! 🚀

#### Opção B: Azure App Service
1. Crie um App Service (.NET 9)
2. Configure Connection String nas **Configuration Settings**
3. Deploy via GitHub Actions ou Visual Studio

#### Opção C: Heroku
1. Instale Heroku CLI
2. `heroku create financial-control-api`
3. Configure buildpack: `heroku buildpacks:set heroku/dotnet`
4. `git push heroku main`

---

### 3️⃣ Deploy do Frontend

**Opções:**

#### Opção A: Vercel (RECOMENDADO)
1. Acesse [Vercel](https://vercel.com)
2. **Import Project** → Selecione o repositório
3. Configure:
   - **Framework Preset**: Vite
   - **Root Directory**: `frontend`
   - **Build Command**: `npm run build`
   - **Output Directory**: `dist`
4. Adicione variável de ambiente:
   ```
   VITE_API_URL=https://seu-backend.railway.app
   ```
5. Deploy automático! 🚀

#### Opção B: Netlify
1. Acesse [Netlify](https://netlify.com)
2. **Add new site** → **Import from Git**
3. Configure:
   - **Base directory**: `frontend`
   - **Build command**: `npm run build`
   - **Publish directory**: `frontend/dist`
4. Deploy!

---

## 🔧 Configurações Finais

### Backend - CORS

Atualize o CORS no backend para incluir a URL do frontend em produção:

```json
"Cors": {
  "AllowedOrigins": [
    "https://seu-app.vercel.app",
    "http://localhost:5173"
  ]
}
```

### Frontend - API URL

Atualize a URL da API no frontend:

**Criar arquivo:** `frontend/.env.production`
```bash
VITE_API_URL=https://seu-backend.railway.app
```

---

## ✅ Checklist de Deploy

### Banco de Dados
- [x] Supabase configurado
- [x] 16 tabelas criadas
- [x] RLS desabilitado
- [x] Migrações registradas

### Backend
- [ ] Connection String do Supabase configurada
- [ ] Deploy realizado (Railway/Azure/Heroku)
- [ ] CORS configurado com URL do frontend
- [ ] Variáveis de ambiente configuradas
- [ ] Endpoint `/health` funcionando

### Frontend
- [ ] Deploy realizado (Vercel/Netlify)
- [ ] VITE_API_URL configurada
- [ ] PWA funcionando
- [ ] Ícones carregando
- [ ] Service Worker registrado

### Testes
- [ ] Login funcionando
- [ ] Criar transação
- [ ] Dashboard carregando
- [ ] Gráficos renderizando
- [ ] PWA instalável

---

## 🔍 Verificações

### Backend
```bash
# Testar endpoint
curl https://seu-backend.railway.app/health

# Deve retornar: {"status": "healthy"}
```

### Frontend
```bash
# Testar PWA
# Abra DevTools → Application → Manifest
# Deve mostrar o manifest.json

# Testar Service Worker
# Abra DevTools → Application → Service Workers
# Deve estar "activated and running"
```

---

## 🐛 Troubleshooting

### Erro: "Connection refused"
- Verifique se a connection string está correta
- Confirme que o Supabase está acessível
- Teste com `psql` ou DBeaver

### Erro: "CORS policy"
- Adicione a URL do frontend no CORS do backend
- Verifique se está usando HTTPS em produção

### Erro: "JWT invalid"
- Verifique se o Secret está configurado
- Confirme que o token não expirou

### PWA não instala
- Verifique se está em HTTPS
- Confirme que o manifest.json está acessível
- Verifique o Service Worker no DevTools

---

## 📊 Monitoramento

### Logs do Backend
- Railway: Dashboard → Logs
- Azure: Log Stream
- Heroku: `heroku logs --tail`

### Logs do Frontend
- Vercel: Dashboard → Deployments → Logs
- Netlify: Dashboard → Deploys → Deploy log

### Banco de Dados
- Supabase: Dashboard → Database → Logs

---

## 🎯 Após o Deploy

1. **Teste completo da aplicação**
2. **Configure backups automáticos** (Supabase faz isso)
3. **Configure alertas** de erro
4. **Monitore performance**
5. **Documente URLs** de produção

---

## 📝 URLs de Produção

Após o deploy, documente aqui:

```
Frontend: https://_____.vercel.app
Backend: https://_____.railway.app
Banco: https://_____.supabase.co
```

---

## 🚀 Deploy Rápido (TL;DR)

```bash
# 1. Configure Supabase connection string
# Edite: backend/FinancialControl.Api/appsettings.Production.json

# 2. Deploy Backend (Railway)
# - Conecte GitHub
# - Configure variáveis de ambiente
# - Deploy automático

# 3. Deploy Frontend (Vercel)
# - Conecte GitHub
# - Configure VITE_API_URL
# - Deploy automático

# 4. Teste!
```

---

**Última Atualização**: 09/01/2026 - 16:12
