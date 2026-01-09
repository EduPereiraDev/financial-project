import { useState, useEffect } from 'react';
import { recurringTransactionService } from '../services/recurringTransactionService';
import { RecurringTransaction, FREQUENCY_LABELS, FREQUENCY_ICONS } from '../types/recurringTransaction';
import RecurringTransactionModal from '../components/RecurringTransactionModal';
import AccountIdMissingAlert from '../components/AccountIdMissingAlert';

export default function RecurringTransactionsPage() {
  const [recurringTransactions, setRecurringTransactions] = useState<RecurringTransaction[]>([]);
  const [loading, setLoading] = useState(true);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingTransaction, setEditingTransaction] = useState<RecurringTransaction | null>(null);
  const accountId = localStorage.getItem('accountId') || '';

  // Se não tiver accountId, mostrar alerta
  if (!accountId) {
    return <AccountIdMissingAlert />;
  }

  useEffect(() => {
    loadRecurringTransactions();
  }, []);

  const loadRecurringTransactions = async () => {
    if (!accountId) {
      console.warn('AccountId não encontrado. Faça login novamente.');
      setLoading(false);
      return;
    }

    try {
      setLoading(true);
      const data = await recurringTransactionService.getByAccount(accountId);
      setRecurringTransactions(data);
    } catch (error) {
      console.error('Erro ao carregar recorrências:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleCreate = () => {
    setEditingTransaction(null);
    setIsModalOpen(true);
  };

  const handleEdit = (transaction: RecurringTransaction) => {
    setEditingTransaction(transaction);
    setIsModalOpen(true);
  };

  const handleDelete = async (id: string) => {
    if (!confirm('Tem certeza que deseja excluir esta recorrência?')) return;

    try {
      await recurringTransactionService.delete(id, accountId);
      await loadRecurringTransactions();
    } catch (error) {
      console.error('Erro ao excluir recorrência:', error);
      alert('Erro ao excluir recorrência');
    }
  };

  const handleToggleActive = async (transaction: RecurringTransaction) => {
    try {
      await recurringTransactionService.update(transaction.id, accountId, {
        categoryId: transaction.categoryId,
        description: transaction.description,
        amount: transaction.amount,
        frequency: transaction.frequency,
        dayOfMonth: transaction.dayOfMonth,
        endDate: transaction.endDate,
        isActive: !transaction.isActive,
      });
      await loadRecurringTransactions();
    } catch (error) {
      console.error('Erro ao atualizar recorrência:', error);
      alert('Erro ao atualizar recorrência');
    }
  };

  const handleModalClose = async (success: boolean) => {
    setIsModalOpen(false);
    setEditingTransaction(null);
    if (success) {
      await loadRecurringTransactions();
    }
  };

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL',
    }).format(value);
  };

  const formatDate = (dateString?: string) => {
    if (!dateString) return '-';
    return new Date(dateString).toLocaleDateString('pt-BR');
  };

  if (loading) {
    return (
      <div className="flex justify-center items-center min-h-screen">
        <div className="text-xl">Carregando...</div>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold">Receitas/Despesas Recorrentes</h1>
        <button
          onClick={handleCreate}
          className="bg-blue-600 text-white px-6 py-2 rounded-lg hover:bg-blue-700 transition"
        >
          + Nova Recorrência
        </button>
      </div>

      {recurringTransactions.length === 0 ? (
        <div className="text-center py-12 bg-gray-50 rounded-lg">
          <p className="text-gray-500 text-lg mb-4">Nenhuma recorrência cadastrada</p>
          <button
            onClick={handleCreate}
            className="bg-blue-600 text-white px-6 py-2 rounded-lg hover:bg-blue-700 transition"
          >
            Criar Primeira Recorrência
          </button>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {recurringTransactions.map((transaction) => (
            <div
              key={transaction.id}
              className={`bg-white rounded-lg shadow-md p-6 border-l-4 ${
                transaction.type === 'Income' ? 'border-green-500' : 'border-red-500'
              } ${!transaction.isActive ? 'opacity-60' : ''}`}
            >
              <div className="flex justify-between items-start mb-4">
                <div className="flex-1">
                  <h3 className="text-lg font-semibold mb-1">{transaction.description}</h3>
                  <p className="text-sm text-gray-500">
                    {transaction.category?.name || 'Sem categoria'}
                  </p>
                </div>
                <div className="flex gap-2">
                  <button
                    onClick={() => handleToggleActive(transaction)}
                    className={`px-3 py-1 rounded text-sm ${
                      transaction.isActive
                        ? 'bg-green-100 text-green-700'
                        : 'bg-gray-100 text-gray-700'
                    }`}
                  >
                    {transaction.isActive ? 'Ativa' : 'Inativa'}
                  </button>
                </div>
              </div>

              <div className="mb-4">
                <p className={`text-2xl font-bold ${
                  transaction.type === 'Income' ? 'text-green-600' : 'text-red-600'
                }`}>
                  {formatCurrency(transaction.amount)}
                </p>
              </div>

              <div className="space-y-2 text-sm">
                <div className="flex items-center gap-2">
                  <span>{FREQUENCY_ICONS[transaction.frequency]}</span>
                  <span className="font-medium">{FREQUENCY_LABELS[transaction.frequency]}</span>
                  {transaction.frequency === 'Monthly' && (
                    <span className="text-gray-500">- Dia {transaction.dayOfMonth}</span>
                  )}
                </div>

                {transaction.nextExecutionDate && (
                  <div className="flex items-center gap-2 text-gray-600">
                    <span>📅</span>
                    <span>Próxima: {formatDate(transaction.nextExecutionDate)}</span>
                  </div>
                )}

                {transaction.lastExecutionDate && (
                  <div className="flex items-center gap-2 text-gray-500">
                    <span>✓</span>
                    <span>Última: {formatDate(transaction.lastExecutionDate)}</span>
                  </div>
                )}

                {transaction.endDate && (
                  <div className="flex items-center gap-2 text-gray-500">
                    <span>🏁</span>
                    <span>Termina: {formatDate(transaction.endDate)}</span>
                  </div>
                )}
              </div>

              <div className="flex gap-2 mt-4 pt-4 border-t">
                <button
                  onClick={() => handleEdit(transaction)}
                  className="flex-1 bg-blue-50 text-blue-600 px-4 py-2 rounded hover:bg-blue-100 transition"
                >
                  Editar
                </button>
                <button
                  onClick={() => handleDelete(transaction.id)}
                  className="flex-1 bg-red-50 text-red-600 px-4 py-2 rounded hover:bg-red-100 transition"
                >
                  Excluir
                </button>
              </div>
            </div>
          ))}
        </div>
      )}

      {isModalOpen && (
        <RecurringTransactionModal
          transaction={editingTransaction}
          accountId={accountId}
          onClose={handleModalClose}
        />
      )}
    </div>
  );
}
