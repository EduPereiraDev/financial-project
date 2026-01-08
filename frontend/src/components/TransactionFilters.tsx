import { useState } from 'react'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select'
import { Card, CardContent } from '@/components/ui/card'

export interface TransactionFiltersData {
  type?: 'Income' | 'Expense' | 'All'
  startDate?: string
  endDate?: string
  minAmount?: number
  maxAmount?: number
  search?: string
}

interface TransactionFiltersProps {
  onApplyFilters: (filters: TransactionFiltersData) => void
  onClearFilters: () => void
}

export default function TransactionFilters({ onApplyFilters, onClearFilters }: TransactionFiltersProps) {
  const [filters, setFilters] = useState<TransactionFiltersData>({
    type: 'All',
    startDate: '',
    endDate: '',
    minAmount: undefined,
    maxAmount: undefined,
    search: ''
  })

  const handleApply = () => {
    const cleanFilters: TransactionFiltersData = {}
    
    if (filters.type && filters.type !== 'All') {
      cleanFilters.type = filters.type
    }
    if (filters.startDate) {
      cleanFilters.startDate = filters.startDate
    }
    if (filters.endDate) {
      cleanFilters.endDate = filters.endDate
    }
    if (filters.minAmount !== undefined && filters.minAmount > 0) {
      cleanFilters.minAmount = filters.minAmount
    }
    if (filters.maxAmount !== undefined && filters.maxAmount > 0) {
      cleanFilters.maxAmount = filters.maxAmount
    }
    if (filters.search && filters.search.trim()) {
      cleanFilters.search = filters.search.trim()
    }

    onApplyFilters(cleanFilters)
  }

  const handleClear = () => {
    setFilters({
      type: 'All',
      startDate: '',
      endDate: '',
      minAmount: undefined,
      maxAmount: undefined,
      search: ''
    })
    onClearFilters()
  }

  return (
    <Card className="mb-6">
      <CardContent className="pt-6">
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {/* Busca por descrição */}
          <div className="grid gap-2">
            <Label htmlFor="search">Buscar</Label>
            <Input
              id="search"
              placeholder="Descrição da transação..."
              value={filters.search}
              onChange={(e) => setFilters({ ...filters, search: e.target.value })}
            />
          </div>

          {/* Filtro por tipo */}
          <div className="grid gap-2">
            <Label htmlFor="type">Tipo</Label>
            <Select
              value={filters.type}
              onValueChange={(value) => setFilters({ ...filters, type: value as 'Income' | 'Expense' | 'All' })}
            >
              <SelectTrigger>
                <SelectValue />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="All">Todos</SelectItem>
                <SelectItem value="Income">Receitas</SelectItem>
                <SelectItem value="Expense">Despesas</SelectItem>
              </SelectContent>
            </Select>
          </div>

          {/* Data inicial */}
          <div className="grid gap-2">
            <Label htmlFor="startDate">Data Inicial</Label>
            <Input
              id="startDate"
              type="date"
              value={filters.startDate}
              onChange={(e) => setFilters({ ...filters, startDate: e.target.value })}
            />
          </div>

          {/* Data final */}
          <div className="grid gap-2">
            <Label htmlFor="endDate">Data Final</Label>
            <Input
              id="endDate"
              type="date"
              value={filters.endDate}
              onChange={(e) => setFilters({ ...filters, endDate: e.target.value })}
            />
          </div>

          {/* Valor mínimo */}
          <div className="grid gap-2">
            <Label htmlFor="minAmount">Valor Mínimo</Label>
            <Input
              id="minAmount"
              type="number"
              step="0.01"
              min="0"
              placeholder="0.00"
              value={filters.minAmount || ''}
              onChange={(e) => setFilters({ ...filters, minAmount: e.target.value ? parseFloat(e.target.value) : undefined })}
            />
          </div>

          {/* Valor máximo */}
          <div className="grid gap-2">
            <Label htmlFor="maxAmount">Valor Máximo</Label>
            <Input
              id="maxAmount"
              type="number"
              step="0.01"
              min="0"
              placeholder="0.00"
              value={filters.maxAmount || ''}
              onChange={(e) => setFilters({ ...filters, maxAmount: e.target.value ? parseFloat(e.target.value) : undefined })}
            />
          </div>
        </div>

        {/* Botões de ação */}
        <div className="flex gap-2 mt-4">
          <Button onClick={handleApply}>
            Aplicar Filtros
          </Button>
          <Button variant="outline" onClick={handleClear}>
            Limpar Filtros
          </Button>
        </div>
      </CardContent>
    </Card>
  )
}
