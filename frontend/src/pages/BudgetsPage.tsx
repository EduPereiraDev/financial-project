import { useState, useEffect } from 'react'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from '@/components/ui/dialog'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select'
import { budgetService } from '@/services/budgetService'
import { categoryService, Category } from '@/services/categoryService'
import { BudgetSummary, CreateBudget } from '@/types/budget'
import { PlusIcon, TrashIcon, PencilIcon, TrendingUpIcon, TrendingDownIcon, AlertTriangleIcon } from 'lucide-react'
import { Progress } from '@/components/ui/progress'

export default function BudgetsPage() {
  const [summary, setSummary] = useState<BudgetSummary | null>(null)
  const [categories, setCategories] = useState<Category[]>([])
  const [loading, setLoading] = useState(true)
  const [isDialogOpen, setIsDialogOpen] = useState(false)
  const [editingBudget, setEditingBudget] = useState<string | null>(null)
  const selectedMonth = new Date().getMonth() + 1
  const selectedYear = new Date().getFullYear()
  
  const [formData, setFormData] = useState<CreateBudget>({
    categoryId: '',
    amount: 0,
    period: 'Monthly',
    month: new Date().getMonth() + 1,
    year: new Date().getFullYear()
  })

  useEffect(() => {
    loadData()
  }, [selectedMonth, selectedYear])

  const loadData = async () => {
    try {
      setLoading(true)
      const summaryData = await budgetService.getSummary(selectedMonth, selectedYear)
      setSummary(summaryData)
      
      // Buscar categorias do primeiro account do usuário (simplificado)
      const accountId = localStorage.getItem('accountId') || ''
      if (accountId) {
        const categoriesData = await categoryService.getByAccount(accountId)
        setCategories(categoriesData.filter((c: Category) => c.type === 'Expense'))
      }
    } catch (error) {
      console.error('Erro ao carregar dados:', error)
    } finally {
      setLoading(false)
    }
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    try {
      if (editingBudget) {
        await budgetService.update(editingBudget, { amount: formData.amount })
      } else {
        await budgetService.create(formData)
      }
      setIsDialogOpen(false)
      setEditingBudget(null)
      resetForm()
      loadData()
    } catch (error) {
      console.error('Erro ao salvar orçamento:', error)
    }
  }

  const handleDelete = async (id: string) => {
    if (confirm('Deseja realmente excluir este orçamento?')) {
      try {
        await budgetService.delete(id)
        loadData()
      } catch (error) {
        console.error('Erro ao excluir orçamento:', error)
      }
    }
  }

  const handleEdit = (budget: any) => {
    setEditingBudget(budget.id)
    setFormData({
      categoryId: budget.categoryId,
      amount: budget.amount,
      period: budget.period,
      month: budget.month,
      year: budget.year
    })
    setIsDialogOpen(true)
  }

  const resetForm = () => {
    setFormData({
      categoryId: '',
      amount: 0,
      period: 'Monthly',
      month: selectedMonth,
      year: selectedYear
    })
  }

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL'
    }).format(value)
  }

  const getProgressColor = (percentage: number) => {
    if (percentage >= 100) return 'bg-red-500'
    if (percentage >= 80) return 'bg-yellow-500'
    return 'bg-green-500'
  }

  if (loading) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="text-gray-500">Carregando orçamentos...</div>
      </div>
    )
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="flex justify-between items-center mb-8">
        <div>
          <h1 className="text-3xl font-bold">💰 Orçamentos</h1>
          <p className="text-gray-600 mt-2">Controle seus gastos por categoria</p>
        </div>
        <Dialog open={isDialogOpen} onOpenChange={setIsDialogOpen}>
          <DialogTrigger asChild>
            <Button onClick={() => { resetForm(); setEditingBudget(null); }}>
              <PlusIcon className="w-4 h-4 mr-2" />
              Novo Orçamento
            </Button>
          </DialogTrigger>
          <DialogContent>
            <DialogHeader>
              <DialogTitle>{editingBudget ? 'Editar Orçamento' : 'Novo Orçamento'}</DialogTitle>
              <DialogDescription>
                {editingBudget ? 'Atualize o valor do orçamento' : 'Defina um limite de gastos para uma categoria'}
              </DialogDescription>
            </DialogHeader>
            <form onSubmit={handleSubmit}>
              <div className="space-y-4 py-4">
                {!editingBudget && (
                  <div className="space-y-2">
                    <Label htmlFor="category">Categoria</Label>
                    <Select
                      value={formData.categoryId}
                      onValueChange={(value) => setFormData({ ...formData, categoryId: value })}
                    >
                      <SelectTrigger>
                        <SelectValue placeholder="Selecione uma categoria" />
                      </SelectTrigger>
                      <SelectContent>
                        {categories.map((category) => (
                          <SelectItem key={category.id} value={category.id}>
                            {category.name}
                          </SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                  </div>
                )}
                <div className="space-y-2">
                  <Label htmlFor="amount">Valor do Orçamento</Label>
                  <Input
                    id="amount"
                    type="number"
                    step="0.01"
                    value={formData.amount}
                    onChange={(e) => setFormData({ ...formData, amount: parseFloat(e.target.value) })}
                    required
                  />
                </div>
              </div>
              <DialogFooter>
                <Button type="button" variant="outline" onClick={() => setIsDialogOpen(false)}>
                  Cancelar
                </Button>
                <Button type="submit">
                  {editingBudget ? 'Atualizar' : 'Criar'}
                </Button>
              </DialogFooter>
            </form>
          </DialogContent>
        </Dialog>
      </div>

      <div className="grid gap-6 md:grid-cols-3 mb-8">
        <Card>
          <CardHeader className="pb-2">
            <CardDescription>Orçamento Total</CardDescription>
            <CardTitle className="text-3xl text-blue-600">
              {formatCurrency(summary?.totalBudget || 0)}
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="flex items-center text-sm text-gray-600">
              <TrendingUpIcon className="w-4 h-4 mr-1" />
              {summary?.categoriesCount || 0} categorias
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="pb-2">
            <CardDescription>Total Gasto</CardDescription>
            <CardTitle className="text-3xl text-red-600">
              {formatCurrency(summary?.totalSpent || 0)}
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="flex items-center text-sm text-gray-600">
              <TrendingDownIcon className="w-4 h-4 mr-1" />
              {summary?.overallPercentage.toFixed(1)}% do orçamento
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="pb-2">
            <CardDescription>Saldo Restante</CardDescription>
            <CardTitle className={`text-3xl ${(summary?.totalRemaining || 0) >= 0 ? 'text-green-600' : 'text-red-600'}`}>
              {formatCurrency(summary?.totalRemaining || 0)}
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="flex items-center text-sm text-gray-600">
              <AlertTriangleIcon className="w-4 h-4 mr-1" />
              {summary?.overBudgetCount || 0} acima do limite
            </div>
          </CardContent>
        </Card>
      </div>

      <div className="space-y-4">
        {summary?.budgets.map((budget) => (
          <Card key={budget.id}>
            <CardContent className="pt-6">
              <div className="flex items-center justify-between mb-4">
                <div className="flex items-center gap-3">
                  <div
                    className="w-4 h-4 rounded-full"
                    style={{ backgroundColor: budget.categoryColor }}
                  />
                  <div>
                    <h3 className="font-semibold text-lg">{budget.categoryName}</h3>
                    <p className="text-sm text-gray-600">
                      {formatCurrency(budget.spent)} de {formatCurrency(budget.amount)}
                    </p>
                  </div>
                </div>
                <div className="flex items-center gap-2">
                  <span className={`text-lg font-bold ${budget.percentageUsed >= 100 ? 'text-red-600' : budget.percentageUsed >= 80 ? 'text-yellow-600' : 'text-green-600'}`}>
                    {budget.percentageUsed.toFixed(0)}%
                  </span>
                  <Button variant="ghost" size="sm" onClick={() => handleEdit(budget)}>
                    <PencilIcon className="w-4 h-4" />
                  </Button>
                  <Button variant="ghost" size="sm" onClick={() => handleDelete(budget.id)}>
                    <TrashIcon className="w-4 h-4" />
                  </Button>
                </div>
              </div>
              <Progress value={Math.min(budget.percentageUsed, 100)} className={getProgressColor(budget.percentageUsed)} />
              <div className="flex justify-between mt-2 text-sm text-gray-600">
                <span>Restante: {formatCurrency(budget.remaining)}</span>
                <span>{budget.percentageUsed >= 100 ? 'Limite excedido!' : `${(100 - budget.percentageUsed).toFixed(0)}% disponível`}</span>
              </div>
            </CardContent>
          </Card>
        ))}

        {(!summary?.budgets || summary.budgets.length === 0) && (
          <Card>
            <CardContent className="py-12 text-center">
              <p className="text-gray-500">Nenhum orçamento cadastrado para este período</p>
              <Button className="mt-4" onClick={() => setIsDialogOpen(true)}>
                <PlusIcon className="w-4 h-4 mr-2" />
                Criar Primeiro Orçamento
              </Button>
            </CardContent>
          </Card>
        )}
      </div>
    </div>
  )
}
