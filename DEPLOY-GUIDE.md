esta# 🚀 Guia de Deploy - Financial Control

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

### 2️⃣ Deploy do Backend - Render

1. Acesse [Render](https://render.com)
2. **New** → **Web Service**
3. Conecte seu repositório GitHub
4. Configure:
   - **Name**: `financial-control-api`
   - **Region**: US East (Ohio) ou mais próximo
   - **Branch**: `main`
   - **Root Directory**: `backend/FinancialControl.Api`
   - **Runtime**: .NET
   - **Build Command**: `dotnet publish -c Release -o out`
   - **Start Command**: `dotnet out/FinancialControl.Api.dll`
   - **Instance Type**: Free (ou Starter)

5. **Environment Variables** (adicione todas):
   ```
   ASPNETCORE_ENVIRONMENT=Production
   ASPNETCORE_URLS=http://0.0.0.0:$PORT
   ConnectionStrings__DefaultConnection=<sua-connection-string-supabase>
   JwtSettings__Secret=<seu-secret-32-chars>
   JwtSettings__Issuer=FinancialControlAPI
   JwtSettings__Audience=FinancialControlApp
   JwtSettings__ExpirationInDays=7
   Cors__AllowedOrigins__0=https://seu-app.vercel.app
   Cors__AllowedOrigins__1=http://localhost:5173
   ```

6. **Create Web Service** → Deploy automático! 🚀

---

### 3️⃣ Deploy do Frontend - Vercel

1. Acesse [Vercel](https://vercel.com)
2. **Add New** → **Project**
3. **Import Git Repository** → Selecione seu repositório
4. Configure:
   - **Framework Preset**: Vite
   - **Root Directory**: `frontend`
   - **Build Command**: `npm run build`
   - **Output Directory**: `dist`
   - **Install Command**: `npm install`

5. **Environment Variables**:
   ```
   VITE_API_URL=https://seu-backend.onrender.com
   ```
   ⚠️ **IMPORTANTE**: Substitua pela URL real do seu backend no Render!

6. **Deploy** → Deploy automático! 🚀

7. **Após o deploy**:
   - Copie a URL do Vercel (ex: `https://financial-project-indol.vercel.app`)
   - Volte no Render e adicione essa URL no `Cors__AllowedOrigins__0`
   - Redeploy do backend no Render

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
Backend: https://_____.onrender.com
Banco: https://_____.supabase.co
```

---

## 🚀 Deploy Rápido (TL;DR)

```bash
# 1. Deploy Backend (Render)
# - New Web Service → Conecte GitHub
# - Root: backend/FinancialControl.Api
# - Build: dotnet publish -c Release -o out
# - Start: dotnet out/FinancialControl.Api.dll
# - Adicione variáveis de ambiente (Connection String, JWT, CORS)

# 2. Deploy Frontend (Vercel)
# - Import Project → Conecte GitHub
# - Root: frontend
# - Configure VITE_API_URL com URL do Render
# - Deploy automático

# 3. Configurar CORS
# - Copie URL do Vercel
# - Adicione no Cors__AllowedOrigins__0 do Render
# - Redeploy backend

# 4. Teste!
```

---

**Última Atualização**: 09/01/2026 - 16:12
