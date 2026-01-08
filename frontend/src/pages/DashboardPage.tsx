import { useState, useEffect } from 'react'
import { useAuth } from '@/hooks/useAuth'
import { Button } from '@/components/ui/button'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { authService } from '@/services/api'
import { useNavigate, Link } from 'react-router-dom'
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer, PieChart, Pie, Cell } from 'recharts'
import api from '@/services/api'

interface DashboardStats {
  totalIncome: number
  totalExpense: number
  balance: number
  transactionCount: number
}

interface MonthlyData {
  month: string
  income: number
  expense: number
}

interface CategoryData {
  name: string
  value: number
}

const COLORS = ['#0088FE', '#00C49F', '#FFBB28', '#FF8042', '#8884D8', '#82CA9D']

export default function DashboardPage() {
  const { user, logout } = useAuth()
  const navigate = useNavigate()
  const [stats, setStats] = useState<DashboardStats>({
    totalIncome: 0,
    totalExpense: 0,
    balance: 0,
    transactionCount: 0
  })
  const [monthlyData, setMonthlyData] = useState<MonthlyData[]>([])
  const [categoryData, setCategoryData] = useState<CategoryData[]>([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    loadDashboardData()
  }, [])

  const loadDashboardData = async () => {
    try {
      setLoading(true)
      
      const [transactionsRes] = await Promise.all([
        api.get('/transactions', { params: { page: 1, pageSize: 100 } })
      ])

      const transactions = transactionsRes.data.items || []

      const income = transactions
        .filter((t: any) => t.type === 'Income')
        .reduce((sum: number, t: any) => sum + t.amount, 0)

      const expense = transactions
        .filter((t: any) => t.type === 'Expense')
        .reduce((sum: number, t: any) => sum + t.amount, 0)

      setStats({
        totalIncome: income,
        totalExpense: expense,
        balance: income - expense,
        transactionCount: transactions.length
      })

      const monthlyMap = new Map<string, { income: number; expense: number }>()
      transactions.forEach((t: any) => {
        const month = new Date(t.date).toLocaleDateString('pt-BR', { month: 'short', year: 'numeric' })
        const current = monthlyMap.get(month) || { income: 0, expense: 0 }
        if (t.type === 'Income') {
          current.income += t.amount
        } else {
          current.expense += t.amount
        }
        monthlyMap.set(month, current)
      })

      const monthly = Array.from(monthlyMap.entries())
        .map(([month, data]) => ({ month, ...data }))
        .slice(-6)

      setMonthlyData(monthly)

      const categoryMap = new Map<string, number>()
      transactions
        .filter((t: any) => t.type === 'Expense')
        .forEach((t: any) => {
          const current = categoryMap.get(t.categoryName) || 0
          categoryMap.set(t.categoryName, current + t.amount)
        })

      const categories = Array.from(categoryMap.entries())
        .map(([name, value]) => ({ name, value }))
        .sort((a, b) => b.value - a.value)
        .slice(0, 6)

      setCategoryData(categories)
    } catch (err) {
      console.error('Erro ao carregar dados do dashboard:', err)
    } finally {
      setLoading(false)
    }
  }

  const handleLogout = () => {
    authService.logout()
    logout()
    navigate('/login')
  }

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL'
    }).format(value)
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <header className="bg-white border-b">
        <div className="container mx-auto px-4 py-4 flex items-center justify-between">
          <h1 className="text-2xl font-bold">Dashboard</h1>
          <div className="flex items-center gap-4">
            <span className="text-sm text-gray-600">Olá, {user?.name}</span>
            <Link to="/transactions">
              <Button variant="outline">Transações</Button>
            </Link>
            <Button variant="outline" onClick={handleLogout}>
              Sair
            </Button>
          </div>
        </div>
      </header>

      <main className="container mx-auto px-4 py-8">
        {loading ? (
          <div className="text-center py-12">
            <p className="text-gray-600">Carregando dados...</p>
          </div>
        ) : (
          <div className="space-y-6">
            <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-4">
              <Card>
                <CardHeader className="pb-2">
                  <CardDescription>Receitas</CardDescription>
                  <CardTitle className="text-3xl text-green-600">
                    {formatCurrency(stats.totalIncome)}
                  </CardTitle>
                </CardHeader>
              </Card>

              <Card>
                <CardHeader className="pb-2">
                  <CardDescription>Despesas</CardDescription>
                  <CardTitle className="text-3xl text-red-600">
                    {formatCurrency(stats.totalExpense)}
                  </CardTitle>
                </CardHeader>
              </Card>

              <Card>
                <CardHeader className="pb-2">
                  <CardDescription>Saldo</CardDescription>
                  <CardTitle className={`text-3xl ${stats.balance >= 0 ? 'text-blue-600' : 'text-red-600'}`}>
                    {formatCurrency(stats.balance)}
                  </CardTitle>
                </CardHeader>
              </Card>

              <Card>
                <CardHeader className="pb-2">
                  <CardDescription>Transações</CardDescription>
                  <CardTitle className="text-3xl">
                    {stats.transactionCount}
                  </CardTitle>
                </CardHeader>
              </Card>
            </div>

            <div className="grid gap-6 md:grid-cols-2">
              <Card>
                <CardHeader>
                  <CardTitle>Evolução Mensal</CardTitle>
                  <CardDescription>Receitas vs Despesas (últimos 6 meses)</CardDescription>
                </CardHeader>
                <CardContent>
                  {monthlyData.length > 0 ? (
                    <ResponsiveContainer width="100%" height={300}>
                      <BarChart data={monthlyData}>
                        <CartesianGrid strokeDasharray="3 3" />
                        <XAxis dataKey="month" />
                        <YAxis />
                        <Tooltip formatter={(value) => formatCurrency(Number(value))} />
                        <Legend />
                        <Bar dataKey="income" fill="#10b981" name="Receitas" />
                        <Bar dataKey="expense" fill="#ef4444" name="Despesas" />
                      </BarChart>
                    </ResponsiveContainer>
                  ) : (
                    <p className="text-center text-gray-500 py-12">Nenhum dado disponível</p>
                  )}
                </CardContent>
              </Card>

              <Card>
                <CardHeader>
                  <CardTitle>Despesas por Categoria</CardTitle>
                  <CardDescription>Top 6 categorias</CardDescription>
                </CardHeader>
                <CardContent>
                  {categoryData.length > 0 ? (
                    <ResponsiveContainer width="100%" height={300}>
                      <PieChart>
                        <Pie
                          data={categoryData}
                          cx="50%"
                          cy="50%"
                          labelLine={false}
                          label={({ name, percent }) => `${name} (${(percent * 100).toFixed(0)}%)`}
                          outerRadius={80}
                          fill="#8884d8"
                          dataKey="value"
                        >
                          {categoryData.map((_, index) => (
                            <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                          ))}
                        </Pie>
                        <Tooltip formatter={(value) => formatCurrency(Number(value))} />
                      </PieChart>
                    </ResponsiveContainer>
                  ) : (
                    <p className="text-center text-gray-500 py-12">Nenhuma despesa registrada</p>
                  )}
                </CardContent>
              </Card>
            </div>
          </div>
        )}
      </main>
    </div>
  )
}
