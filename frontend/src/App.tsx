import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import { useAuth } from '@/hooks/useAuth'
import LoginPage from '@/pages/LoginPage'
import RegisterPage from '@/pages/RegisterPage'
import DashboardPage from '@/pages/DashboardPage'
import TransactionsPage from '@/pages/TransactionsPage'
import RecurringTransactionsPage from '@/pages/RecurringTransactionsPage'

function PrivateRoute({ children }: { children: React.ReactNode }) {
  const { isAuthenticated, loading } = useAuth()

  if (loading) {
    return <div className="flex items-center justify-center min-h-screen">Carregando...</div>
  }

  return isAuthenticated ? <>{children}</> : <Navigate to="/login" replace />
}

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route
          path="/dashboard"
          element={
            <PrivateRoute>
              <DashboardPage />
            </PrivateRoute>
          }
        />
        <Route
          path="/transactions"
          element={
            <PrivateRoute>
              <TransactionsPage />
            </PrivateRoute>
          }
        />
        <Route
          path="/recurring"
          element={
            <PrivateRoute>
              <RecurringTransactionsPage />
            </PrivateRoute>
          }
        />
        <Route path="/" element={<Navigate to="/dashboard" replace />} />
      </Routes>
    </BrowserRouter>
  )
}

export default App
