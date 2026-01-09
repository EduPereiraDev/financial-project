import { useState, useEffect } from 'react'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer, PieChart, Pie, Cell, LineChart, Line } from 'recharts'
import { dashboardService } from '@/services/dashboardService'
import { DashboardStats } from '@/types/dashboard'
import { ArrowUpIcon, ArrowDownIcon, TrendingUpIcon } from 'lucide-react'

export default function DashboardPage() {
  const [stats, setStats] = useState<DashboardStats | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    loadDashboardData()
  }, [])

  const loadDashboardData = async () => {
    try {
      setLoading(true)
      setError(null)
      const data = await dashboardService.getStats(6)
      setStats(data)
    } catch (err) {
      console.error('Erro ao carregar dados do dashboard:', err)
      setError('Erro ao carregar dados do dashboard')
    } finally {
      setLoading(false)
    }
  }

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL'
    }).format(value)
  }

  const calculatePercentageChange = (current: number, previous: number) => {
    if (previous === 0) return 0
    return ((current - previous) / previous) * 100
  }

  if (loading) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="text-gray-500">Carregando estatísticas...</div>
      </div>
    )
  }

  if (error || !stats) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="text-red-500">{error || 'Erro ao carregar dados'}</div>
      </div>
    )
  }

  const incomeChange = calculatePercentageChange(stats.totalIncome, stats.previousMonthIncome)
  const expensesChange = calculatePercentageChange(stats.totalExpenses, stats.previousMonthExpenses)

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="mb-8">
        <h1 className="text-3xl font-bold">📊 Dashboard</h1>
        <p className="text-gray-600 mt-2">Visão geral das suas finanças</p>
      </div>

      <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3 mb-8">
        <Card>
          <CardHeader className="pb-2">
            <CardDescription>Receitas do Mês</CardDescription>
            <CardTitle className="text-3xl text-green-600">
              {formatCurrency(stats.totalIncome)}
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="flex items-center text-sm">
              {incomeChange >= 0 ? (
                <>
                  <ArrowUpIcon className="w-4 h-4 text-green-600 mr-1" />
                  <span className="text-green-600">+{incomeChange.toFixed(1)}%</span>
                </>
              ) : (
                <>
                  <ArrowDownIcon className="w-4 h-4 text-red-600 mr-1" />
                  <span className="text-red-600">{incomeChange.toFixed(1)}%</span>
                </>
              )}
              <span className="text-gray-600 ml-2">vs mês anterior</span>
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="pb-2">
            <CardDescription>Despesas do Mês</CardDescription>
            <CardTitle className="text-3xl text-red-600">
              {formatCurrency(stats.totalExpenses)}
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="flex items-center text-sm">
              {expensesChange >= 0 ? (
                <>
                  <ArrowUpIcon className="w-4 h-4 text-red-600 mr-1" />
                  <span className="text-red-600">+{expensesChange.toFixed(1)}%</span>
                </>
              ) : (
                <>
                  <ArrowDownIcon className="w-4 h-4 text-green-600 mr-1" />
                  <span className="text-green-600">{expensesChange.toFixed(1)}%</span>
                </>
              )}
              <span className="text-gray-600 ml-2">vs mês anterior</span>
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="pb-2">
            <CardDescription>Saldo do Mês</CardDescription>
            <CardTitle className={`text-3xl ${stats.balance >= 0 ? 'text-blue-600' : 'text-red-600'}`}>
              {formatCurrency(stats.balance)}
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="flex items-center text-sm">
              <TrendingUpIcon className="w-4 h-4 text-blue-600 mr-1" />
              <span className="text-gray-600">
                {stats.balance >= 0 ? 'Positivo' : 'Negativo'}
              </span>
            </div>
          </CardContent>
        </Card>
      </div>

      <div className="grid gap-6 lg:grid-cols-2 mb-8">
        <Card>
          <CardHeader>
            <CardTitle>Receitas vs Despesas</CardTitle>
            <CardDescription>Últimos 6 meses</CardDescription>
          </CardHeader>
          <CardContent>
            {stats.monthlyData.length > 0 ? (
              <ResponsiveContainer width="100%" height={300}>
                <BarChart data={stats.monthlyData}>
                  <CartesianGrid strokeDasharray="3 3" />
                  <XAxis dataKey="month" />
                  <YAxis tickFormatter={(value) => `R$ ${value}`} />
                  <Tooltip formatter={(value) => formatCurrency(Number(value))} />
                  <Legend />
                  <Bar dataKey="income" fill="#10b981" name="Receitas" />
                  <Bar dataKey="expenses" fill="#ef4444" name="Despesas" />
                </BarChart>
              </ResponsiveContainer>
            ) : (
              <p className="text-center text-gray-500 py-12">Nenhum dado disponível</p>
            )}
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle>Gastos por Categoria</CardTitle>
            <CardDescription>Distribuição do mês atual</CardDescription>
          </CardHeader>
          <CardContent>
            {stats.categoryExpenses.length > 0 ? (
              <ResponsiveContainer width="100%" height={300}>
                <PieChart>
                  <Pie
                    data={stats.categoryExpenses}
                    cx="50%"
                    cy="50%"
                    labelLine={false}
                    label={({ category, percent }) => `${category} (${(percent * 100).toFixed(0)}%)`}
                    outerRadius={100}
                    fill="#8884d8"
                    dataKey="amount"
                  >
                    {stats.categoryExpenses.map((entry, index) => (
                      <Cell key={`cell-${index}`} fill={entry.color} />
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

      <Card>
        <CardHeader>
          <CardTitle>Evolução do Saldo</CardTitle>
          <CardDescription>Últimos 30 dias</CardDescription>
        </CardHeader>
        <CardContent>
          {stats.dailyBalance.length > 0 ? (
            <ResponsiveContainer width="100%" height={300}>
              <LineChart data={stats.dailyBalance}>
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="date" />
                <YAxis tickFormatter={(value) => `R$ ${value}`} />
                <Tooltip formatter={(value) => formatCurrency(Number(value))} />
                <Legend />
                <Line 
                  type="monotone" 
                  dataKey="balance" 
                  stroke="#3b82f6" 
                  strokeWidth={2}
                  name="Saldo"
                  dot={false}
                />
              </LineChart>
            </ResponsiveContainer>
          ) : (
            <p className="text-center text-gray-500 py-12">Nenhum dado disponível</p>
          )}
        </CardContent>
      </Card>
    </div>
  )
}
