import { useState } from 'react'
import { Link, useLocation, useNavigate } from 'react-router-dom'
import { useAuth } from '../hooks/useAuth'
import NotificationCenter from './NotificationCenter'
import { motion, AnimatePresence } from 'framer-motion'
import {
  LayoutDashboard,
  ArrowLeftRight,
  Repeat,
  Bell,
  Building2,
  Users,
  Wallet,
  Target,
  Menu,
  X,
  LogOut,
  ChevronLeft,
  ChevronRight
} from 'lucide-react'

interface LayoutProps {
  children: React.ReactNode
}

const menuItems = [
  { path: '/dashboard', icon: LayoutDashboard, label: 'Dashboard' },
  { path: '/transactions', icon: ArrowLeftRight, label: 'Transações' },
  { path: '/recurring', icon: Repeat, label: 'Recorrentes' },
  { path: '/budgets', icon: Wallet, label: 'Orçamentos' },
  { path: '/goals', icon: Target, label: 'Metas' },
  { path: '/alerts', icon: Bell, label: 'Alertas' },
  { path: '/banking', icon: Building2, label: 'Banking' },
  { path: '/members', icon: Users, label: 'Membros' }
]

export default function ModernLayout({ children }: LayoutProps) {
  const { logout } = useAuth()
  const navigate = useNavigate()
  const location = useLocation()
  const [sidebarOpen, setSidebarOpen] = useState(true)
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false)

  const handleLogout = () => {
    logout()
    navigate('/login')
  }

  const isActive = (path: string) => location.pathname === path

  return (
    <div className="min-h-screen bg-gradient-to-br from-gray-50 to-gray-100">
      {/* Mobile Menu Button */}
      <button
        onClick={() => setMobileMenuOpen(!mobileMenuOpen)}
        className="lg:hidden fixed top-4 left-4 z-50 p-2 bg-white rounded-lg shadow-lg"
      >
        {mobileMenuOpen ? <X className="w-6 h-6" /> : <Menu className="w-6 h-6" />}
      </button>

      {/* Sidebar Desktop */}
      <motion.aside
        initial={false}
        animate={{ width: sidebarOpen ? 256 : 80 }}
        className="hidden lg:block fixed left-0 top-0 h-screen bg-white shadow-xl z-40 border-r border-gray-200"
      >
        <div className="flex flex-col h-full">
          {/* Logo */}
          <div className="flex items-center justify-between p-4 border-b border-gray-200">
            <motion.div
              initial={false}
              animate={{ opacity: sidebarOpen ? 1 : 0 }}
              className="flex items-center space-x-2"
            >
              <div className="w-8 h-8 bg-gradient-to-br from-blue-500 to-purple-600 rounded-lg flex items-center justify-center text-white font-bold">
                FC
              </div>
              {sidebarOpen && (
                <span className="font-bold text-gray-800">Financial Control</span>
              )}
            </motion.div>
            <button
              onClick={() => setSidebarOpen(!sidebarOpen)}
              className="p-1 hover:bg-gray-100 rounded-lg transition-colors"
            >
              {sidebarOpen ? <ChevronLeft className="w-5 h-5" /> : <ChevronRight className="w-5 h-5" />}
            </button>
          </div>

          {/* Menu Items */}
          <nav className="flex-1 overflow-y-auto py-4">
            {menuItems.map((item) => {
              const Icon = item.icon
              const active = isActive(item.path)
              
              return (
                <Link
                  key={item.path}
                  to={item.path}
                  className={`
                    flex items-center px-4 py-3 mx-2 rounded-lg transition-all
                    ${active 
                      ? 'bg-gradient-to-r from-blue-500 to-purple-600 text-white shadow-lg' 
                      : 'text-gray-700 hover:bg-gray-100'
                    }
                  `}
                >
                  <Icon className={`w-5 h-5 ${!sidebarOpen && 'mx-auto'}`} />
                  {sidebarOpen && (
                    <motion.span
                      initial={{ opacity: 0 }}
                      animate={{ opacity: 1 }}
                      className="ml-3 font-medium"
                    >
                      {item.label}
                    </motion.span>
                  )}
                </Link>
              )
            })}
          </nav>

          {/* Logout Button */}
          <div className="p-4 border-t border-gray-200">
            <button
              onClick={handleLogout}
              className="flex items-center w-full px-4 py-3 text-red-600 hover:bg-red-50 rounded-lg transition-colors"
            >
              <LogOut className={`w-5 h-5 ${!sidebarOpen && 'mx-auto'}`} />
              {sidebarOpen && <span className="ml-3 font-medium">Sair</span>}
            </button>
          </div>
        </div>
      </motion.aside>

      {/* Mobile Sidebar */}
      <AnimatePresence>
        {mobileMenuOpen && (
          <>
            <motion.div
              initial={{ opacity: 0 }}
              animate={{ opacity: 1 }}
              exit={{ opacity: 0 }}
              onClick={() => setMobileMenuOpen(false)}
              className="lg:hidden fixed inset-0 bg-black bg-opacity-50 z-40"
            />
            <motion.aside
              initial={{ x: -300 }}
              animate={{ x: 0 }}
              exit={{ x: -300 }}
              className="lg:hidden fixed left-0 top-0 h-screen w-64 bg-white shadow-xl z-50"
            >
              <div className="flex flex-col h-full">
                <div className="flex items-center justify-between p-4 border-b border-gray-200">
                  <div className="flex items-center space-x-2">
                    <div className="w-8 h-8 bg-gradient-to-br from-blue-500 to-purple-600 rounded-lg flex items-center justify-center text-white font-bold">
                      FC
                    </div>
                    <span className="font-bold text-gray-800">Financial Control</span>
                  </div>
                </div>

                <nav className="flex-1 overflow-y-auto py-4">
                  {menuItems.map((item) => {
                    const Icon = item.icon
                    const active = isActive(item.path)
                    
                    return (
                      <Link
                        key={item.path}
                        to={item.path}
                        onClick={() => setMobileMenuOpen(false)}
                        className={`
                          flex items-center px-4 py-3 mx-2 rounded-lg transition-all
                          ${active 
                            ? 'bg-gradient-to-r from-blue-500 to-purple-600 text-white shadow-lg' 
                            : 'text-gray-700 hover:bg-gray-100'
                          }
                        `}
                      >
                        <Icon className="w-5 h-5" />
                        <span className="ml-3 font-medium">{item.label}</span>
                      </Link>
                    )
                  })}
                </nav>

                <div className="p-4 border-t border-gray-200">
                  <button
                    onClick={handleLogout}
                    className="flex items-center w-full px-4 py-3 text-red-600 hover:bg-red-50 rounded-lg transition-colors"
                  >
                    <LogOut className="w-5 h-5" />
                    <span className="ml-3 font-medium">Sair</span>
                  </button>
                </div>
              </div>
            </motion.aside>
          </>
        )}
      </AnimatePresence>

      {/* Main Content */}
      <motion.main
        initial={false}
        animate={{ marginLeft: sidebarOpen ? 256 : 80 }}
        className="hidden lg:block min-h-screen transition-all"
      >
        <div className="p-8">
          {/* Header */}
          <div className="flex items-center justify-between mb-8">
            <div>
              <h1 className="text-3xl font-bold text-gray-800">
                {menuItems.find(item => item.path === location.pathname)?.label || 'Dashboard'}
              </h1>
              <p className="text-gray-600 mt-1">Bem-vindo ao seu controle financeiro</p>
            </div>
            <NotificationCenter />
          </div>

          {/* Page Content */}
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.3 }}
          >
            {children}
          </motion.div>
        </div>
      </motion.main>

      {/* Mobile Main Content */}
      <main className="lg:hidden min-h-screen pt-20 px-4 pb-8">
        <div className="mb-6">
          <h1 className="text-2xl font-bold text-gray-800">
            {menuItems.find(item => item.path === location.pathname)?.label || 'Dashboard'}
          </h1>
          <p className="text-gray-600 mt-1">Bem-vindo ao seu controle financeiro</p>
        </div>
        
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.3 }}
        >
          {children}
        </motion.div>
      </main>
    </div>
  )
}
