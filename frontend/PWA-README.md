# 📱 PWA - Progressive Web App

## 🎯 Funcionalidades Implementadas

### ✅ Instalação
- Prompt de instalação automático
- Suporte para iOS (Apple Touch Icons)
- Suporte para Android (Web App Manifest)
- Atalhos de aplicativo (Shortcuts)
- Ícones adaptativos (Maskable Icons)

### ✅ Offline First
- Service Worker com estratégia Network First
- Cache automático de recursos
- Funcionalidade offline básica
- Sincronização em background (Background Sync)

### ✅ Notificações Push
- Suporte para Push Notifications
- Permissão de notificações
- Badge e vibração
- Ações em notificações

### ✅ Responsividade
- Meta viewport otimizado
- Breakpoints: Mobile (< 768px), Tablet (768-1024px), Desktop (> 1024px)
- Hooks customizados: `useResponsive`, `useMediaQuery`, `useOrientation`
- Detecção de dispositivos touch

## 📦 Arquivos PWA

```
frontend/
├── public/
│   ├── manifest.json          # Web App Manifest
│   ├── sw.js                  # Service Worker
│   └── icons/                 # Ícones PWA (72x72 até 512x512)
├── src/
│   ├── utils/
│   │   └── registerSW.ts     # Registro do Service Worker
│   ├── hooks/
│   │   └── useResponsive.ts  # Hooks de responsividade
│   └── components/
│       └── InstallPWA.tsx    # Componente de instalação
└── index.html                 # Meta tags PWA
```

## 🚀 Como Usar

### Instalação do PWA

1. **Desktop (Chrome/Edge)**:
   - Clique no ícone de instalação na barra de endereços
   - Ou use o prompt automático que aparece

2. **Mobile (Android)**:
   - Abra o menu do navegador
   - Selecione "Adicionar à tela inicial"
   - Ou use o prompt automático

3. **Mobile (iOS)**:
   - Abra no Safari
   - Toque no ícone de compartilhar
   - Selecione "Adicionar à Tela de Início"

### Hooks de Responsividade

```typescript
import { useResponsive } from '@/hooks/useResponsive'

function MyComponent() {
  const { isMobile, isTablet, isDesktop, width } = useResponsive()
  
  return (
    <div>
      {isMobile && <MobileView />}
      {isTablet && <TabletView />}
      {isDesktop && <DesktopView />}
    </div>
  )
}
```

### Media Query Hook

```typescript
import { useMediaQuery } from '@/hooks/useResponsive'

function MyComponent() {
  const isDarkMode = useMediaQuery('(prefers-color-scheme: dark)')
  const isLandscape = useMediaQuery('(orientation: landscape)')
  
  return <div>...</div>
}
```

### Componente de Instalação

```typescript
import { InstallPWA } from '@/components/InstallPWA'

function App() {
  return (
    <>
      <YourApp />
      <InstallPWA />
    </>
  )
}
```

## 🔧 Configuração

### Manifest.json

O arquivo `manifest.json` contém:
- Nome e descrição do app
- Ícones em múltiplos tamanhos
- Tema e cores
- Atalhos (Shortcuts)
- Screenshots
- Orientação preferida

### Service Worker

Estratégias implementadas:
- **Network First**: Tenta buscar da rede primeiro, fallback para cache
- **Cache on Success**: Cacheia respostas bem-sucedidas automaticamente
- **Offline Fallback**: Retorna página inicial quando offline

### Notificações

Para habilitar notificações:

```typescript
import { requestNotificationPermission } from '@/utils/registerSW'

const granted = await requestNotificationPermission()
if (granted) {
  // Notificações habilitadas
}
```

## 📊 Breakpoints

```css
/* Mobile */
< 768px

/* Tablet */
768px - 1024px

/* Desktop */
1024px - 1536px

/* Large Desktop */
> 1536px
```

## 🎨 Tema

- **Primary Color**: #3b82f6 (Blue 500)
- **Background**: #ffffff (White)
- **Display Mode**: Standalone
- **Orientation**: Portrait Primary

## 🔒 Segurança

- HTTPS obrigatório para Service Workers
- Permissões explícitas para notificações
- Cache controlado e versionado
- Validação de origem

## 📈 Performance

- Cache de recursos estáticos
- Lazy loading de componentes
- Code splitting automático
- Compressão gzip

## 🐛 Troubleshooting

### Service Worker não registra
- Verifique se está em HTTPS ou localhost
- Limpe o cache do navegador
- Verifique o console para erros

### PWA não instala
- Verifique se o manifest.json está acessível
- Confirme que todos os ícones existem
- Verifique os requisitos do navegador

### Notificações não funcionam
- Verifique permissões do navegador
- Confirme que o Service Worker está ativo
- Teste em HTTPS

## 📚 Recursos

- [Web.dev - PWA](https://web.dev/progressive-web-apps/)
- [MDN - Service Workers](https://developer.mozilla.org/en-US/docs/Web/API/Service_Worker_API)
- [Web App Manifest](https://developer.mozilla.org/en-US/docs/Web/Manifest)

## ✅ Checklist PWA

- ✅ Manifest.json configurado
- ✅ Service Worker registrado
- ✅ Ícones em múltiplos tamanhos
- ✅ Meta tags para iOS
- ✅ HTTPS (produção)
- ✅ Responsivo em todos os dispositivos
- ✅ Funcionalidade offline básica
- ✅ Prompt de instalação
- ✅ Tema e cores definidos
- ✅ Atalhos configurados

## 🎯 Próximos Passos

1. Gerar ícones PWA reais (atualmente placeholders)
2. Implementar sincronização offline completa
3. Adicionar mais estratégias de cache
4. Configurar Push Notifications backend
5. Adicionar Analytics PWA
6. Implementar Update Prompt
7. Adicionar Screenshots para stores
