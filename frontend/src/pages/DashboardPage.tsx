import { useState, useEffect } from 'react'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Skeleton } from '@/components/ui/skeleton'
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer, PieChart, Pie, Cell, LineChart, Line } from 'recharts'
import { dashboardService } from '@/services/dashboardService'
import { DashboardStats } from '@/types/dashboard'
import { ArrowUpIcon, ArrowDownIcon, TrendingUpIcon } from 'lucide-react'
import { motion } from 'framer-motion'
import { toast } from 'sonner'

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
      toast.success('Dashboard atualizado!')
    } catch (err) {
      console.error('Erro ao carregar dados do dashboard:', err)
      setError('Erro ao carregar dados do dashboard')
      toast.error('Erro ao carregar dashboard')
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
      <div className="space-y-6">
        <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
          {[1, 2, 3].map((i) => (
            <Card key={i}>
              <CardHeader>
                <Skeleton className="h-4 w-24" />
                <Skeleton className="h-8 w-32 mt-2" />
              </CardHeader>
              <CardContent>
                <Skeleton className="h-4 w-20" />
              </CardContent>
            </Card>
          ))}
        </div>
        <div className="grid gap-6 md:grid-cols-2">
          <Card>
            <CardHeader>
              <Skeleton className="h-6 w-48" />
            </CardHeader>
            <CardContent>
              <Skeleton className="h-64 w-full" />
            </CardContent>
          </Card>
          <Card>
            <CardHeader>
              <Skeleton className="h-6 w-48" />
            </CardHeader>
            <CardContent>
              <Skeleton className="h-64 w-full" />
            </CardContent>
          </Card>
        </div>
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
    <motion.div 
      initial={{ opacity: 0 }}
      animate={{ opacity: 1 }}
      transition={{ duration: 0.5 }}
      className="space-y-6"
    >
      <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0 }}
          whileHover={{ scale: 1.02, y: -5 }}
        >
          <Card className="overflow-hidden border-0 shadow-lg hover:shadow-xl transition-shadow duration-300">
            <div className="h-2 bg-gradient-to-r from-green-500 to-green-600" />
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
        </motion.div>

        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.1 }}
          whileHover={{ scale: 1.02, y: -5 }}
        >
          <Card className="overflow-hidden border-0 shadow-lg hover:shadow-xl transition-shadow duration-300">
            <div className="h-2 bg-gradient-to-r from-red-500 to-red-600" />
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
        </motion.div>

        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.2 }}
          whileHover={{ scale: 1.02, y: -5 }}
        >
          <Card className="overflow-hidden border-0 shadow-lg hover:shadow-xl transition-shadow duration-300">
            <div className={`h-2 bg-gradient-to-r ${stats.balance >= 0 ? 'from-blue-500 to-blue-600' : 'from-red-500 to-red-600'}`} />
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
        </motion.div>
      </div>

      <div className="grid gap-6 lg:grid-cols-2">
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.3 }}
        >
          <Card className="shadow-lg hover:shadow-xl transition-shadow duration-300">
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
        </motion.div>

        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.4 }}
        >
          <Card className="shadow-lg hover:shadow-xl transition-shadow duration-300">
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
        </motion.div>
      </div>

      <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ delay: 0.5 }}
      >
        <Card className="shadow-lg hover:shadow-xl transition-shadow duration-300">
        <CardHeader>
          <CardTitle>Evolução do Saldo</CardTitle>
          <CardDescription>Últimos 30 dias</CardDescription>
        </CardHeader>
        <CardContent>
          {stats.dailyBalance.length > 0 ? (
            <ResponsiveContainer width="100%" height={300}>
              <LineChart data={stats.dailyBalance}>
                <CartesianGrid strokeDasharray="3 3" stroke="#e5e7eb" />
                <XAxis dataKey="date" stroke="#6b7280" />
                <YAxis tickFormatter={(value) => `R$ ${value}`} stroke="#6b7280" />
                <Tooltip 
                  formatter={(value) => formatCurrency(Number(value))}
                  contentStyle={{ 
                    backgroundColor: 'white', 
                    border: '1px solid #e5e7eb',
                    borderRadius: '8px',
                    boxShadow: '0 4px 6px -1px rgb(0 0 0 / 0.1)'
                  }}
                />
                <Legend />
                <Line 
                  type="monotone" 
                  dataKey="balance" 
                  stroke="#3b82f6" 
                  strokeWidth={3}
                  name="Saldo"
                  dot={{ fill: '#3b82f6', r: 4 }}
                  activeDot={{ r: 6 }}
                  animationDuration={1000}
                />
              </LineChart>
            </ResponsiveContainer>
          ) : (
            <p className="text-center text-gray-500 py-12">Nenhum dado disponível</p>
          )}
        </CardContent>
      </Card>
      </motion.div>
    </motion.div>
  )
}
