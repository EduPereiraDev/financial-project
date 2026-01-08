# 🚀 Guia de Deploy - 100% Gratuito

> **Stack Gratuita Permanente**: Render.com + Supabase + Vercel

---

## 📋 Pré-requisitos

- [ ] Conta GitHub (gratuita)
- [ ] Conta Render.com (gratuita, sem cartão)
- [ ] Conta Supabase (gratuita, sem cartão)
- [ ] Conta Vercel (gratuita, sem cartão)

---

## 🗄️ Passo 1: Configurar Banco de Dados (Supabase)

### 1.1 Criar Projeto Supabase

1. Acesse [supabase.com](https://supabase.com)
2. Clique em "Start your project"
3. Crie uma conta (GitHub login recomendado)
4. Clique em "New Project"
5. Preencha:
   - **Name**: `financial-control`
   - **Database Password**: (anote essa senha!)
   - **Region**: `South America (São Paulo)` ou mais próximo
   - **Pricing Plan**: `Free` (gratuito para sempre)
6. Clique em "Create new project"
7. Aguarde ~2 minutos

### 1.2 Obter Connection String

1. No projeto criado, vá em **Settings** → **Database**
2. Role até **Connection string**
3. Selecione **URI** (não Pooler)
4. Copie a connection string (formato: `postgresql://postgres:[YOUR-PASSWORD]@...`)
5. **Substitua `[YOUR-PASSWORD]`** pela senha que você criou
6. **Anote essa connection string** - você vai usar no Render

**Exemplo:**
```
postgresql://postgres:SuaSenhaAqui@db.abcdefghijk.supabase.co:5432/postgres
```

### 1.3 Executar Migrations

**Opção A: Via Supabase SQL Editor (Recomendado)**

1. No Supabase, vá em **SQL Editor**
2. Clique em "New query"
3. Copie o conteúdo de `backend/Migrations/20260108171315_InitialCreate.cs`
4. Cole apenas o SQL (remova o código C#)
5. Execute

**Opção B: Via pgAdmin/DBeaver**

1. Use a connection string para conectar
2. Execute o script SQL da migration

---

## 🔧 Passo 2: Deploy Backend (Render.com)

### 2.1 Preparar Repositório GitHub

1. **Commit e push** de todos os arquivos:
```bash
cd /Users/edupereira/Projetos/Financial-Project
git add .
git commit -m "feat: adicionar configuração de deploy"
git push origin main
```

### 2.2 Criar Serviço no Render

1. Acesse [render.com](https://render.com)
2. Clique em "Get Started" ou "Sign Up"
3. Faça login com GitHub
4. Clique em "New +" → "Web Service"
5. Conecte seu repositório GitHub:
   - Clique em "Connect a repository"
   - Autorize o Render no GitHub
   - Selecione o repositório `Financial-Project`
6. Configure o serviço:
   - **Name**: `financial-control-api`
   - **Region**: `Oregon (US West)` (gratuito)
   - **Branch**: `main`
   - **Root Directory**: deixe vazio
   - **Environment**: `Docker`
   - **Plan**: `Free` ✅
7. Clique em "Advanced"
8. Adicione **Environment Variables**:
   - `ASPNETCORE_ENVIRONMENT` = `Production`
   - `ConnectionStrings__DefaultConnection` = `sua-connection-string-do-supabase`
   - `JwtSettings__Secret` = `gere-uma-chave-secreta-forte-aqui-min-32-caracteres`
   - `JwtSettings__Issuer` = `FinancialControlAPI`
   - `JwtSettings__Audience` = `FinancialControlApp`
   - `JwtSettings__ExpirationInDays` = `7`
9. Clique em "Create Web Service"
10. Aguarde o deploy (~5-10 minutos)

### 2.3 Verificar Deploy

1. Após o deploy, você verá a URL: `https://financial-control-api.onrender.com`
2. Teste a API:
   - Acesse: `https://financial-control-api.onrender.com/swagger`
   - Deve abrir a documentação Swagger
3. **Anote essa URL** - você vai usar no frontend

**⚠️ Importante:**
- O app gratuito "dorme" após 15min de inatividade
- Primeira requisição após dormir demora ~30s (cold start)
- Para manter ativo 24/7, use um serviço de ping (opcional)

---

## 🎨 Passo 3: Deploy Frontend (Vercel)

### 3.1 Configurar Variável de Ambiente

1. Edite `frontend/.env.production`:
```env
VITE_API_URL=https://financial-control-api.onrender.com/api
```

2. Commit e push:
```bash
git add frontend/.env.production
git commit -m "feat: configurar URL da API de produção"
git push origin main
```

### 3.2 Deploy no Vercel

1. Acesse [vercel.com](https://vercel.com)
2. Clique em "Sign Up" ou "Login"
3. Faça login com GitHub
4. Clique em "Add New..." → "Project"
5. Importe o repositório `Financial-Project`
6. Configure:
   - **Framework Preset**: `Vite`
   - **Root Directory**: `frontend`
   - **Build Command**: `npm run build`
   - **Output Directory**: `dist`
7. Adicione **Environment Variables**:
   - `VITE_API_URL` = `https://financial-control-api.onrender.com/api`
8. Clique em "Deploy"
9. Aguarde o deploy (~2 minutos)

### 3.3 Verificar Deploy

1. Após o deploy, você terá uma URL: `https://financial-control-frontend.vercel.app`
2. Acesse a URL
3. Teste login/registro

---

## 🔄 Passo 4: Configurar CORS no Backend

### 4.1 Atualizar CORS

1. Volte ao Render.com
2. Vá no serviço `financial-control-api`
3. Clique em "Environment"
4. Adicione/Atualize:
   - `CORS__AllowedOrigins` = `https://seu-app.vercel.app,http://localhost:3000`
5. Salve (vai fazer redeploy automático)

---

## ✅ Checklist Final

- [ ] ✅ Supabase PostgreSQL criado e rodando
- [ ] ✅ Migrations executadas no banco
- [ ] ✅ Backend deployado no Render.com
- [ ] ✅ Swagger acessível
- [ ] ✅ Frontend deployado no Vercel
- [ ] ✅ CORS configurado corretamente
- [ ] ✅ Login/Registro funcionando
- [ ] ✅ Transações CRUD funcionando

---

## 🎉 Aplicação no Ar!

**URLs Finais:**
- 🔧 **Backend API**: `https://financial-control-api.onrender.com`
- 🎨 **Frontend App**: `https://financial-control-frontend.vercel.app`
- 📚 **Swagger Docs**: `https://financial-control-api.onrender.com/swagger`

**Custo Total: R$ 0,00/mês** 🎊

---

## 🔧 Troubleshooting

### Backend não inicia
- Verifique logs no Render: Dashboard → Logs
- Confirme connection string do Supabase
- Verifique se migrations foram executadas

### Frontend não conecta no backend
- Verifique CORS no backend
- Confirme `VITE_API_URL` no Vercel
- Teste API diretamente no Swagger

### App demora para responder
- Normal no plano gratuito (cold start ~30s)
- Use serviço de ping para manter ativo (opcional)

---

## 📝 Próximos Passos

1. **Custom Domain** (opcional, gratuito):
   - Render: Settings → Custom Domain
   - Vercel: Settings → Domains

2. **CI/CD Automático**:
   - ✅ Já configurado!
   - Cada push no GitHub = deploy automático

3. **Monitoramento**:
   - Render: Dashboard → Metrics
   - Vercel: Analytics (gratuito)

---

**Última atualização**: 08/01/2026  
**Autor**: Cascade AI + Eduardo Pereira
