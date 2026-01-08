import { useState, useEffect } from 'react'
import { Dialog, DialogContent, DialogDescription, DialogFooter, DialogHeader, DialogTitle } from '@/components/ui/dialog'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select'
import api from '@/services/api'
import type { Account, Category } from '@/types'

interface CreateTransactionDialogProps {
  open: boolean
  onOpenChange: (open: boolean) => void
  onSuccess: () => void
}

export default function CreateTransactionDialog({ open, onOpenChange, onSuccess }: CreateTransactionDialogProps) {
  const [loading, setLoading] = useState(false)
  const [accounts, setAccounts] = useState<Account[]>([])
  const [categories, setCategories] = useState<Category[]>([])
  const [error, setError] = useState('')

  const [formData, setFormData] = useState({
    description: '',
    amount: '',
    type: 'Expense' as 'Income' | 'Expense',
    date: new Date().toISOString().split('T')[0],
    accountId: '',
    categoryId: ''
  })

  useEffect(() => {
    if (open) {
      loadAccounts()
    }
  }, [open])

  useEffect(() => {
    if (formData.accountId) {
      loadCategories(formData.accountId)
    }
  }, [formData.accountId])

  const loadAccounts = async () => {
    try {
      const response = await api.get<Account[]>('/accounts')
      setAccounts(response.data)
      if (response.data.length > 0) {
        setFormData(prev => ({ ...prev, accountId: response.data[0].id.toString() }))
      }
    } catch (err) {
      console.error('Erro ao carregar contas:', err)
    }
  }

  const loadCategories = async (accountId: string) => {
    try {
      const response = await api.get<Category[]>(`/categories?accountId=${accountId}`)
      setCategories(response.data)
      if (response.data.length > 0) {
        setFormData(prev => ({ ...prev, categoryId: response.data[0].id.toString() }))
      }
    } catch (err) {
      console.error('Erro ao carregar categorias:', err)
    }
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setLoading(true)
    setError('')

    try {
      await api.post('/transactions', {
        description: formData.description,
        amount: parseFloat(formData.amount),
        type: formData.type,
        date: formData.date,
        accountId: parseInt(formData.accountId),
        categoryId: parseInt(formData.categoryId)
      })

      setFormData({
        description: '',
        amount: '',
        type: 'Expense',
        date: new Date().toISOString().split('T')[0],
        accountId: accounts[0]?.id.toString() || '',
        categoryId: ''
      })
      onSuccess()
      onOpenChange(false)
    } catch (err: any) {
      setError(err.response?.data?.message || 'Erro ao criar transação')
    } finally {
      setLoading(false)
    }
  }

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-[500px]">
        <DialogHeader>
          <DialogTitle>Nova Transação</DialogTitle>
          <DialogDescription>
            Adicione uma nova receita ou despesa
          </DialogDescription>
        </DialogHeader>

        <form onSubmit={handleSubmit}>
          <div className="grid gap-4 py-4">
            {error && (
              <div className="p-3 text-sm text-red-500 bg-red-50 border border-red-200 rounded-md">
                {error}
              </div>
            )}

            <div className="grid gap-2">
              <Label htmlFor="type">Tipo</Label>
              <Select
                value={formData.type}
                onValueChange={(value) => setFormData({ ...formData, type: value as 'Income' | 'Expense' })}
              >
                <SelectTrigger>
                  <SelectValue />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="Income">Receita</SelectItem>
                  <SelectItem value="Expense">Despesa</SelectItem>
                </SelectContent>
              </Select>
            </div>

            <div className="grid gap-2">
              <Label htmlFor="description">Descrição</Label>
              <Input
                id="description"
                value={formData.description}
                onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                placeholder="Ex: Salário, Aluguel, Compras..."
                required
              />
            </div>

            <div className="grid gap-2">
              <Label htmlFor="amount">Valor</Label>
              <Input
                id="amount"
                type="number"
                step="0.01"
                min="0"
                value={formData.amount}
                onChange={(e) => setFormData({ ...formData, amount: e.target.value })}
                placeholder="0.00"
                required
              />
            </div>

            <div className="grid gap-2">
              <Label htmlFor="date">Data</Label>
              <Input
                id="date"
                type="date"
                value={formData.date}
                onChange={(e) => setFormData({ ...formData, date: e.target.value })}
                required
              />
            </div>

            <div className="grid gap-2">
              <Label htmlFor="account">Conta</Label>
              <Select
                value={formData.accountId}
                onValueChange={(value) => setFormData({ ...formData, accountId: value })}
              >
                <SelectTrigger>
                  <SelectValue placeholder="Selecione uma conta" />
                </SelectTrigger>
                <SelectContent>
                  {accounts.map((account) => (
                    <SelectItem key={account.id} value={account.id.toString()}>
                      {account.name}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>

            <div className="grid gap-2">
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
                    <SelectItem key={category.id} value={category.id.toString()}>
                      <span className="flex items-center gap-2">
                        <span>{category.icon}</span>
                        {category.name}
                      </span>
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>
          </div>

          <DialogFooter>
            <Button type="button" variant="outline" onClick={() => onOpenChange(false)}>
              Cancelar
            </Button>
            <Button type="submit" disabled={loading}>
              {loading ? 'Criando...' : 'Criar Transação'}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  )
}
