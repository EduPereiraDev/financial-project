export async function registerServiceWorker() {
  if ('serviceWorker' in navigator) {
    try {
      const registration = await navigator.serviceWorker.register('/sw.js', {
        scope: '/'
      })
      
      console.log('Service Worker registrado com sucesso:', registration.scope)
      
      // Check for updates
      registration.addEventListener('updatefound', () => {
        const newWorker = registration.installing
        if (newWorker) {
          newWorker.addEventListener('statechange', () => {
            if (newWorker.state === 'installed' && navigator.serviceWorker.controller) {
              // New service worker available
              console.log('Nova versão disponível! Recarregue a página.')
              if (confirm('Nova versão disponível! Deseja atualizar agora?')) {
                window.location.reload()
              }
            }
          })
        }
      })
      
      return registration
    } catch (error) {
      console.error('Erro ao registrar Service Worker:', error)
    }
  } else {
    console.log('Service Worker não suportado neste navegador')
  }
}

export async function unregisterServiceWorker() {
  if ('serviceWorker' in navigator) {
    const registrations = await navigator.serviceWorker.getRegistrations()
    for (const registration of registrations) {
      await registration.unregister()
    }
    console.log('Service Workers desregistrados')
  }
}

export async function checkForUpdates() {
  if ('serviceWorker' in navigator) {
    const registration = await navigator.serviceWorker.getRegistration()
    if (registration) {
      await registration.update()
      console.log('Verificação de atualizações concluída')
    }
  }
}

// Request notification permission
export async function requestNotificationPermission() {
  if ('Notification' in window) {
    const permission = await Notification.requestPermission()
    console.log('Permissão de notificação:', permission)
    return permission === 'granted'
  }
  return false
}

// Subscribe to push notifications
export async function subscribeToPushNotifications() {
  if ('serviceWorker' in navigator && 'PushManager' in window) {
    try {
      const registration = await navigator.serviceWorker.ready
      const subscription = await registration.pushManager.subscribe({
        userVisibleOnly: true,
        applicationServerKey: urlBase64ToUint8Array(
          // Add your VAPID public key here
          'YOUR_VAPID_PUBLIC_KEY'
        )
      })
      
      console.log('Inscrito em push notifications:', subscription)
      return subscription
    } catch (error) {
      console.error('Erro ao inscrever em push notifications:', error)
    }
  }
  return null
}

function urlBase64ToUint8Array(base64String: string) {
  const padding = '='.repeat((4 - base64String.length % 4) % 4)
  const base64 = (base64String + padding)
    .replace(/\-/g, '+')
    .replace(/_/g, '/')
  
  const rawData = window.atob(base64)
  const outputArray = new Uint8Array(rawData.length)
  
  for (let i = 0; i < rawData.length; ++i) {
    outputArray[i] = rawData.charCodeAt(i)
  }
  return outputArray
}

// Install prompt
let deferredPrompt: any = null

window.addEventListener('beforeinstallprompt', (e) => {
  e.preventDefault()
  deferredPrompt = e
  console.log('PWA pode ser instalado')
})

export async function showInstallPrompt() {
  if (deferredPrompt) {
    deferredPrompt.prompt()
    const { outcome } = await deferredPrompt.userChoice
    console.log(`Usuário ${outcome === 'accepted' ? 'aceitou' : 'recusou'} a instalação`)
    deferredPrompt = null
    return outcome === 'accepted'
  }
  return false
}

export function canInstallPWA() {
  return deferredPrompt !== null
}
