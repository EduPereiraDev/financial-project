import { useState, useEffect } from 'react'
import { Download, X } from 'lucide-react'
import { Button } from './ui/button'
import { canInstallPWA, showInstallPrompt } from '@/utils/registerSW'
import { motion, AnimatePresence } from 'framer-motion'

export function InstallPWA() {
  const [showPrompt, setShowPrompt] = useState(false)
  const [isInstalled, setIsInstalled] = useState(false)

  useEffect(() => {
    // Check if already installed
    if (window.matchMedia('(display-mode: standalone)').matches) {
      setIsInstalled(true)
      return
    }

    // Check if can be installed
    const checkInstallable = setInterval(() => {
      if (canInstallPWA()) {
        setShowPrompt(true)
        clearInterval(checkInstallable)
      }
    }, 1000)

    return () => clearInterval(checkInstallable)
  }, [])

  const handleInstall = async () => {
    const accepted = await showInstallPrompt()
    if (accepted) {
      setIsInstalled(true)
      setShowPrompt(false)
    }
  }

  const handleDismiss = () => {
    setShowPrompt(false)
    localStorage.setItem('pwa-prompt-dismissed', 'true')
  }

  // Don't show if already installed or dismissed
  if (isInstalled || localStorage.getItem('pwa-prompt-dismissed')) {
    return null
  }

  return (
    <AnimatePresence>
      {showPrompt && (
        <motion.div
          initial={{ opacity: 0, y: 50 }}
          animate={{ opacity: 1, y: 0 }}
          exit={{ opacity: 0, y: 50 }}
          className="fixed bottom-4 right-4 z-50 max-w-sm"
        >
          <div className="bg-white rounded-lg shadow-2xl border border-gray-200 p-4">
            <div className="flex items-start gap-3">
              <div className="flex-shrink-0 w-12 h-12 bg-gradient-to-r from-blue-500 to-purple-600 rounded-lg flex items-center justify-center">
                <Download className="w-6 h-6 text-white" />
              </div>
              
              <div className="flex-1">
                <h3 className="font-semibold text-gray-900 mb-1">
                  Instalar Aplicativo
                </h3>
                <p className="text-sm text-gray-600 mb-3">
                  Instale o Financial Control para acesso rápido e funcionalidade offline
                </p>
                
                <div className="flex gap-2">
                  <Button
                    onClick={handleInstall}
                    size="sm"
                    className="bg-gradient-to-r from-blue-500 to-purple-600 hover:from-blue-600 hover:to-purple-700"
                  >
                    Instalar
                  </Button>
                  <Button
                    onClick={handleDismiss}
                    size="sm"
                    variant="outline"
                  >
                    Agora não
                  </Button>
                </div>
              </div>
              
              <button
                onClick={handleDismiss}
                className="flex-shrink-0 p-1 hover:bg-gray-100 rounded-full transition-colors"
              >
                <X className="w-4 h-4 text-gray-500" />
              </button>
            </div>
          </div>
        </motion.div>
      )}
    </AnimatePresence>
  )
}
