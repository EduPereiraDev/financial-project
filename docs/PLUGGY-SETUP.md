# 🏦 Configuração da Integração Pluggy (Open Banking)

## 📋 Pré-requisitos

1. Criar conta no Pluggy: https://dashboard.pluggy.ai/signup
2. Obter credenciais (Client ID e Client Secret)

## 🔧 Configuração Backend

### 1. Adicionar Credenciais

Edite o arquivo `appsettings.json` ou configure via variáveis de ambiente:

```json
{
  "Pluggy": {
    "ClientId": "SEU_CLIENT_ID_AQUI",
    "ClientSecret": "SEU_CLIENT_SECRET_AQUI"
  }
}
```

**⚠️ IMPORTANTE**: Nunca commite suas credenciais reais! Use variáveis de ambiente em produção:

```bash
export Pluggy__ClientId="seu_client_id"
export Pluggy__ClientSecret="seu_client_secret"
```

### 2. Pacotes Instalados

- ✅ Pluggy.SDK v0.32.7

## 🚀 Como Funciona

### Fluxo de Conexão

1. **Frontend** solicita um Connect Token via `POST /api/banking/connect-token`
2. **Backend** cria token usando Pluggy SDK
3. **Frontend** abre Pluggy Connect Widget com o token
4. **Usuário** autentica no banco através do widget
5. **Pluggy** retorna `itemId` após conexão bem-sucedida
6. **Frontend** salva conexão via `POST /api/banking/connections`
7. **Backend** sincroniza transações automaticamente

### Endpoints Disponíveis

```
POST   /api/banking/connect-token          - Criar token para Pluggy Connect
POST   /api/banking/connections            - Salvar conexão bancária
GET    /api/banking/connections            - Listar conexões
GET    /api/banking/connections/{id}       - Detalhes da conexão
PUT    /api/banking/connections/{id}       - Atualizar conexão
DELETE /api/banking/connections/{id}       - Excluir conexão
POST   /api/banking/connections/{id}/sync  - Sincronizar transações
GET    /api/banking/transactions/pending   - Transações pendentes
POST   /api/banking/transactions/import    - Importar transação
POST   /api/banking/transactions/{id}/ignore - Ignorar transação
```

## 🔐 Segurança

- ✅ Todas as rotas protegidas com JWT
- ✅ Credenciais nunca expostas ao frontend
- ✅ Connect Token tem validade curta
- ✅ ItemId vinculado ao usuário

## 📊 Dados Sincronizados

- **Transações**: Últimos 3 meses por padrão
- **Contas**: Todas as contas do banco conectado
- **Saldo**: Saldo atual de cada conta
- **Categorias**: Categorização automática do Pluggy

## 🧪 Modo de Teste

O Pluggy oferece um ambiente sandbox para testes:

1. Use credenciais de sandbox
2. Conecte bancos fictícios
3. Teste fluxo completo sem dados reais

## 📚 Documentação Oficial

- Pluggy Docs: https://docs.pluggy.ai/
- Pluggy Connect: https://docs.pluggy.ai/docs/connect-quickstart
- API Reference: https://docs.pluggy.ai/reference/

## 🐛 Troubleshooting

### Erro: "Pluggy ClientId não configurado"
- Verifique se as credenciais estão no appsettings.json
- Ou configure via variáveis de ambiente

### Erro ao sincronizar transações
- Verifique se o ItemId é válido
- Confirme que a conexão está ativa no Pluggy Dashboard
- Veja logs do backend para detalhes

### Widget não abre
- Verifique se o Connect Token foi criado corretamente
- Confirme que o frontend está usando o token correto
- Veja console do navegador para erros
