import { useState, useEffect } from 'react'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from '@/components/ui/dialog'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Textarea } from '@/components/ui/textarea'
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select'
import { goalService } from '@/services/goalService'
import { GoalSummary, CreateGoal, CreateContribution } from '@/types/goal'
import { PlusIcon, TrashIcon, TrendingUpIcon, TargetIcon, DollarSignIcon } from 'lucide-react'
import { Progress } from '@/components/ui/progress'

export default function GoalsPage() {
  const [summary, setSummary] = useState<GoalSummary | null>(null)
  const [loading, setLoading] = useState(true)
  const [isDialogOpen, setIsDialogOpen] = useState(false)
  const [isContributionDialogOpen, setIsContributionDialogOpen] = useState(false)
  const [selectedGoalId, setSelectedGoalId] = useState<string | null>(null)
  
  const [formData, setFormData] = useState<CreateGoal>({
    name: '',
    description: '',
    targetAmount: 0,
    targetDate: '',
    priority: 'Medium'
  })

  const [contributionData, setContributionData] = useState<CreateContribution>({
    amount: 0,
    note: ''
  })

  useEffect(() => {
    loadData()
  }, [])

  const loadData = async () => {
    try {
      setLoading(true)
      const summaryData = await goalService.getSummary()
      setSummary(summaryData)
    } catch (error) {
      console.error('Erro ao carregar dados:', error)
    } finally {
      setLoading(false)
    }
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    try {
      await goalService.create(formData)
      setIsDialogOpen(false)
      resetForm()
      loadData()
    } catch (error) {
      console.error('Erro ao criar meta:', error)
    }
  }

  const handleDelete = async (id: string) => {
    if (confirm('Deseja realmente excluir esta meta?')) {
      try {
        await goalService.delete(id)
        loadData()
      } catch (error) {
        console.error('Erro ao excluir meta:', error)
      }
    }
  }

  const handleAddContribution = async (e: React.FormEvent) => {
    e.preventDefault()
    if (!selectedGoalId) return

    try {
      await goalService.addContribution(selectedGoalId, contributionData)
      setIsContributionDialogOpen(false)
      setSelectedGoalId(null)
      setContributionData({ amount: 0, note: '' })
      loadData()
    } catch (error) {
      console.error('Erro ao adicionar contribuição:', error)
    }
  }

  const openContributionDialog = (goalId: string) => {
    setSelectedGoalId(goalId)
    setIsContributionDialogOpen(true)
  }

  const resetForm = () => {
    setFormData({
      name: '',
      description: '',
      targetAmount: 0,
      targetDate: '',
      priority: 'Medium'
    })
  }

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL'
    }).format(value)
  }

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('pt-BR')
  }

  const getProgressColor = (percentage: number) => {
    if (percentage >= 100) return 'bg-green-500'
    if (percentage >= 75) return 'bg-blue-500'
    if (percentage >= 50) return 'bg-yellow-500'
    return 'bg-orange-500'
  }

  const getPriorityColor = (priority: string) => {
    switch (priority) {
      case 'Critical': return 'text-red-600'
      case 'High': return 'text-orange-600'
      case 'Medium': return 'text-yellow-600'
      case 'Low': return 'text-green-600'
      default: return 'text-gray-600'
    }
  }

  if (loading) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="text-gray-500">Carregando metas...</div>
      </div>
    )
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="flex justify-between items-center mb-8">
        <div>
          <h1 className="text-3xl font-bold">🎯 Metas Financeiras</h1>
          <p className="text-gray-600 mt-2">Economize para seus objetivos</p>
        </div>
        <Dialog open={isDialogOpen} onOpenChange={setIsDialogOpen}>
          <DialogTrigger asChild>
            <Button onClick={resetForm}>
              <PlusIcon className="w-4 h-4 mr-2" />
              Nova Meta
            </Button>
          </DialogTrigger>
          <DialogContent className="max-w-md">
            <DialogHeader>
              <DialogTitle>Nova Meta</DialogTitle>
              <DialogDescription>
                Defina um objetivo financeiro para economizar
              </DialogDescription>
            </DialogHeader>
            <form onSubmit={handleSubmit}>
              <div className="space-y-4 py-4">
                <div className="space-y-2">
                  <Label htmlFor="name">Nome da Meta</Label>
                  <Input
                    id="name"
                    value={formData.name}
                    onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                    placeholder="Ex: Viagem para Europa"
                    required
                  />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="description">Descrição</Label>
                  <Textarea
                    id="description"
                    value={formData.description}
                    onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                    placeholder="Descreva sua meta..."
                  />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="targetAmount">Valor Alvo</Label>
                  <Input
                    id="targetAmount"
                    type="number"
                    step="0.01"
                    value={formData.targetAmount}
                    onChange={(e) => setFormData({ ...formData, targetAmount: parseFloat(e.target.value) })}
                    required
                  />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="targetDate">Data Alvo</Label>
                  <Input
                    id="targetDate"
                    type="date"
                    value={formData.targetDate}
                    onChange={(e) => setFormData({ ...formData, targetDate: e.target.value })}
                    required
                  />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="priority">Prioridade</Label>
                  <Select
                    value={formData.priority}
                    onValueChange={(value) => setFormData({ ...formData, priority: value })}
                  >
                    <SelectTrigger>
                      <SelectValue />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="Low">Baixa</SelectItem>
                      <SelectItem value="Medium">Média</SelectItem>
                      <SelectItem value="High">Alta</SelectItem>
                      <SelectItem value="Critical">Crítica</SelectItem>
                    </SelectContent>
                  </Select>
                </div>
              </div>
              <DialogFooter>
                <Button type="button" variant="outline" onClick={() => setIsDialogOpen(false)}>
                  Cancelar
                </Button>
                <Button type="submit">Criar Meta</Button>
              </DialogFooter>
            </form>
          </DialogContent>
        </Dialog>
      </div>

      <div className="grid gap-6 md:grid-cols-4 mb-8">
        <Card>
          <CardHeader className="pb-2">
            <CardDescription>Total de Metas</CardDescription>
            <CardTitle className="text-3xl text-blue-600">
              {summary?.totalGoals || 0}
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="flex items-center text-sm text-gray-600">
              <TargetIcon className="w-4 h-4 mr-1" />
              {summary?.activeGoals || 0} ativas
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader className="pb-2">
            <CardDescription>Valor Total Alvo</CardDescription>
            <CardTitle className="text-3xl text-purple-600">
              {formatCurrency(summary?.totalTargetAmount || 0)}
            </CardTitle>
          </CardHeader>
        </Card>

        <Card>
          <CardHeader className="pb-2">
            <CardDescription>Valor Economizado</CardDescription>
            <CardTitle className="text-3xl text-green-600">
              {formatCurrency(summary?.totalCurrentAmount || 0)}
            </CardTitle>
          </CardHeader>
        </Card>

        <Card>
          <CardHeader className="pb-2">
            <CardDescription>Progresso Geral</CardDescription>
            <CardTitle className="text-3xl text-orange-600">
              {summary?.overallProgress.toFixed(1)}%
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="flex items-center text-sm text-gray-600">
              <TrendingUpIcon className="w-4 h-4 mr-1" />
              {summary?.completedGoals || 0} concluídas
            </div>
          </CardContent>
        </Card>
      </div>

      <div className="space-y-4">
        {summary?.goals.map((goal) => (
          <Card key={goal.id} className={goal.status === 'Completed' ? 'border-green-500' : ''}>
            <CardContent className="pt-6">
              <div className="flex items-start justify-between mb-4">
                <div className="flex-1">
                  <div className="flex items-center gap-2 mb-2">
                    <h3 className="font-semibold text-lg">{goal.name}</h3>
                    <span className={`text-sm font-medium ${getPriorityColor(goal.priority)}`}>
                      {goal.priority === 'Critical' ? 'Crítica' : goal.priority === 'High' ? 'Alta' : goal.priority === 'Medium' ? 'Média' : 'Baixa'}
                    </span>
                    {goal.status === 'Completed' && (
                      <span className="text-sm bg-green-100 text-green-800 px-2 py-1 rounded">
                        ✓ Concluída
                      </span>
                    )}
                  </div>
                  <p className="text-sm text-gray-600 mb-3">{goal.description}</p>
                  <div className="grid grid-cols-2 md:grid-cols-4 gap-4 text-sm">
                    <div>
                      <div className="text-gray-500">Progresso</div>
                      <div className="font-semibold">{goal.percentageComplete.toFixed(0)}%</div>
                    </div>
                    <div>
                      <div className="text-gray-500">Economizado</div>
                      <div className="font-semibold">{formatCurrency(goal.currentAmount)}</div>
                    </div>
                    <div>
                      <div className="text-gray-500">Faltam</div>
                      <div className="font-semibold">{formatCurrency(goal.remainingAmount)}</div>
                    </div>
                    <div>
                      <div className="text-gray-500">Prazo</div>
                      <div className="font-semibold">{goal.daysRemaining} dias</div>
                    </div>
                  </div>
                </div>
                <div className="flex gap-2 ml-4">
                  {goal.status === 'Active' && (
                    <Button 
                      variant="outline" 
                      size="sm"
                      onClick={() => openContributionDialog(goal.id)}
                    >
                      <DollarSignIcon className="w-4 h-4" />
                    </Button>
                  )}
                  <Button variant="ghost" size="sm" onClick={() => handleDelete(goal.id)}>
                    <TrashIcon className="w-4 h-4" />
                  </Button>
                </div>
              </div>
              <Progress value={Math.min(goal.percentageComplete, 100)} className={getProgressColor(goal.percentageComplete)} />
              <div className="flex justify-between mt-2 text-sm text-gray-600">
                <span>Meta: {formatCurrency(goal.targetAmount)}</span>
                <span>Data: {formatDate(goal.targetDate)}</span>
              </div>
              {goal.status === 'Active' && goal.requiredMonthlyContribution > 0 && (
                <div className="mt-3 p-3 bg-blue-50 rounded text-sm">
                  <span className="font-medium">💡 Sugestão:</span> Economize {formatCurrency(goal.requiredMonthlyContribution)}/mês para atingir sua meta
                </div>
              )}
            </CardContent>
          </Card>
        ))}

        {(!summary?.goals || summary.goals.length === 0) && (
          <Card>
            <CardContent className="py-12 text-center">
              <p className="text-gray-500">Nenhuma meta cadastrada</p>
              <Button className="mt-4" onClick={() => setIsDialogOpen(true)}>
                <PlusIcon className="w-4 h-4 mr-2" />
                Criar Primeira Meta
              </Button>
            </CardContent>
          </Card>
        )}
      </div>

      <Dialog open={isContributionDialogOpen} onOpenChange={setIsContributionDialogOpen}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Adicionar Contribuição</DialogTitle>
            <DialogDescription>
              Registre um valor economizado para esta meta
            </DialogDescription>
          </DialogHeader>
          <form onSubmit={handleAddContribution}>
            <div className="space-y-4 py-4">
              <div className="space-y-2">
                <Label htmlFor="amount">Valor</Label>
                <Input
                  id="amount"
                  type="number"
                  step="0.01"
                  value={contributionData.amount}
                  onChange={(e) => setContributionData({ ...contributionData, amount: parseFloat(e.target.value) })}
                  required
                />
              </div>
              <div className="space-y-2">
                <Label htmlFor="note">Observação (opcional)</Label>
                <Textarea
                  id="note"
                  value={contributionData.note}
                  onChange={(e) => setContributionData({ ...contributionData, note: e.target.value })}
                  placeholder="Ex: Economia do mês de janeiro"
                />
              </div>
            </div>
            <DialogFooter>
              <Button type="button" variant="outline" onClick={() => setIsContributionDialogOpen(false)}>
                Cancelar
              </Button>
              <Button type="submit">Adicionar</Button>
            </DialogFooter>
          </form>
        </DialogContent>
      </Dialog>
    </div>
  )
}
