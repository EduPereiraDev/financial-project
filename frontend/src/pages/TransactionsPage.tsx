import { useState, useEffect } from 'react'
import { Button } from '@/components/ui/button'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
} from '@/components/ui/alert-dialog'
import CreateTransactionDialog from '@/components/CreateTransactionDialog'
import EditTransactionDialog from '@/components/EditTransactionDialog'
import TransactionFilters, { type TransactionFiltersData } from '@/components/TransactionFilters'
import api from '@/services/api'
import type { Transaction, PagedResult } from '@/types'

export default function TransactionsPage() {
  const [transactions, setTransactions] = useState<Transaction[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [page, setPage] = useState(1)
  const [totalPages, setTotalPages] = useState(1)
  const [createDialogOpen, setCreateDialogOpen] = useState(false)
  const [editDialogOpen, setEditDialogOpen] = useState(false)
  const [selectedTransaction, setSelectedTransaction] = useState<Transaction | null>(null)
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false)
  const [transactionToDelete, setTransactionToDelete] = useState<Transaction | null>(null)
  const [deleting, setDeleting] = useState(false)
  const [filters, setFilters] = useState<TransactionFiltersData>({})

  useEffect(() => {
    loadTransactions()
  }, [page, filters])

  const loadTransactions = async () => {
    try {
      setLoading(true)
      setError('')
      const params: any = { page, pageSize: 10 }
      
      // Adiciona filtros aos parâmetros
      if (filters.type && filters.type !== 'All') {
        params.type = filters.type
      }
      if (filters.startDate) {
        params.startDate = filters.startDate
      }
      if (filters.endDate) {
        params.endDate = filters.endDate
      }
      if (filters.minAmount !== undefined) {
        params.minAmount = filters.minAmount
      }
      if (filters.maxAmount !== undefined) {
        params.maxAmount = filters.maxAmount
      }
      if (filters.search) {
        params.search = filters.search
      }

      const response = await api.get<PagedResult<Transaction>>('/transactions', { params })
      setTransactions(response.data.items)
      setTotalPages(response.data.totalPages)
    } catch (err: any) {
      setError(err.response?.data?.message || 'Erro ao carregar transações')
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

  const formatDate = (date: string) => {
    return new Date(date).toLocaleDateString('pt-BR')
  }

  const handleEdit = (transaction: Transaction) => {
    setSelectedTransaction(transaction)
    setEditDialogOpen(true)
  }

  const handleDeleteClick = (transaction: Transaction) => {
    setTransactionToDelete(transaction)
    setDeleteDialogOpen(true)
  }

  const handleDelete = async () => {
    if (!transactionToDelete) return

    setDeleting(true)
    try {
      await api.delete(`/transactions/${transactionToDelete.id}`)
      setDeleteDialogOpen(false)
      setTransactionToDelete(null)
      loadTransactions()
    } catch (err) {
      console.error('Erro ao excluir transação:', err)
    } finally {
      setDeleting(false)
    }
  }

  const handleApplyFilters = (newFilters: TransactionFiltersData) => {
    setFilters(newFilters)
    setPage(1) // Reset para primeira página ao aplicar filtros
  }

  const handleClearFilters = () => {
    setFilters({})
    setPage(1) // Reset para primeira página ao limpar filtros
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <header className="bg-white border-b">
        <div className="container mx-auto px-4 py-4">
          <h1 className="text-2xl font-bold">Transações</h1>
        </div>
      </header>

      <main className="container mx-auto px-4 py-8">
        <TransactionFilters
          onApplyFilters={handleApplyFilters}
          onClearFilters={handleClearFilters}
        />

        <Card>
          <CardHeader>
            <div className="flex items-center justify-between">
              <div>
                <CardTitle>Minhas Transações</CardTitle>
                <CardDescription>
                  Gerencie suas receitas e despesas
                </CardDescription>
              </div>
              <Button onClick={() => setCreateDialogOpen(true)}>+ Nova Transação</Button>
            </div>
          </CardHeader>
          <CardContent>
            {error && (
              <div className="p-3 mb-4 text-sm text-red-500 bg-red-50 border border-red-200 rounded-md">
                {error}
              </div>
            )}

            {loading ? (
              <div className="text-center py-8 text-gray-500">
                Carregando transações...
              </div>
            ) : transactions.length === 0 ? (
              <div className="text-center py-8 text-gray-500">
                Nenhuma transação encontrada.
                <br />
                <Button className="mt-4">Criar primeira transação</Button>
              </div>
            ) : (
              <>
                <div className="overflow-x-auto">
                  <table className="w-full">
                    <thead className="border-b">
                      <tr className="text-left text-sm text-gray-600">
                        <th className="pb-3 font-medium">Data</th>
                        <th className="pb-3 font-medium">Descrição</th>
                        <th className="pb-3 font-medium">Categoria</th>
                        <th className="pb-3 font-medium">Conta</th>
                        <th className="pb-3 font-medium text-right">Valor</th>
                        <th className="pb-3 font-medium text-right">Ações</th>
                      </tr>
                    </thead>
                    <tbody>
                      {transactions.map((transaction) => (
                        <tr key={transaction.id} className="border-b hover:bg-gray-50">
                          <td className="py-3 text-sm text-gray-600">
                            {formatDate(transaction.date)}
                          </td>
                          <td className="py-3 text-sm font-medium">
                            {transaction.description}
                          </td>
                          <td className="py-3 text-sm">
                            <span
                              className="inline-flex items-center gap-1 px-2 py-1 rounded-full text-xs"
                              style={{ backgroundColor: `${transaction.categoryColor}20` }}
                            >
                              <span>{transaction.categoryIcon}</span>
                              {transaction.categoryName}
                            </span>
                          </td>
                          <td className="py-3 text-sm text-gray-600">
                            {transaction.accountName}
                          </td>
                          <td className={`py-3 text-sm font-semibold text-right ${
                            transaction.type === 'Income' ? 'text-green-600' : 'text-red-600'
                          }`}>
                            {transaction.type === 'Income' ? '+' : '-'} {formatCurrency(transaction.amount)}
                          </td>
                          <td className="py-3 text-right">
                            <div className="flex items-center justify-end gap-2">
                              <Button 
                                variant="ghost" 
                                size="sm"
                                onClick={() => handleEdit(transaction)}
                              >
                                Editar
                              </Button>
                              <Button 
                                variant="ghost" 
                                size="sm"
                                onClick={() => handleDeleteClick(transaction)}
                              >
                                Excluir
                              </Button>
                            </div>
                          </td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>

                {totalPages > 1 && (
                  <div className="flex items-center justify-between mt-6">
                    <Button
                      variant="outline"
                      onClick={() => setPage(p => Math.max(1, p - 1))}
                      disabled={page === 1}
                    >
                      Anterior
                    </Button>
                    <span className="text-sm text-gray-600">
                      Página {page} de {totalPages}
                    </span>
                    <Button
                      variant="outline"
                      onClick={() => setPage(p => Math.min(totalPages, p + 1))}
                      disabled={page === totalPages}
                    >
                      Próxima
                    </Button>
                  </div>
                )}
              </>
            )}
          </CardContent>
        </Card>
      </main>

      <CreateTransactionDialog
        open={createDialogOpen}
        onOpenChange={setCreateDialogOpen}
        onSuccess={loadTransactions}
      />

      <EditTransactionDialog
        open={editDialogOpen}
        onOpenChange={setEditDialogOpen}
        onSuccess={loadTransactions}
        transaction={selectedTransaction}
      />

      <AlertDialog open={deleteDialogOpen} onOpenChange={setDeleteDialogOpen}>
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>Confirmar Exclusão</AlertDialogTitle>
            <AlertDialogDescription>
              Tem certeza que deseja excluir a transação "{transactionToDelete?.description}"?
              Esta ação não pode ser desfeita.
            </AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel disabled={deleting}>Cancelar</AlertDialogCancel>
            <AlertDialogAction onClick={handleDelete} disabled={deleting}>
              {deleting ? 'Excluindo...' : 'Excluir'}
            </AlertDialogAction>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>
    </div>
  )
}
