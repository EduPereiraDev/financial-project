import { useState, useEffect } from 'react';
import { recurringTransactionService } from '../services/recurringTransactionService';
import {
  RecurringTransaction,
  RecurrenceFrequency,
  TransactionType,
  FREQUENCY_LABELS,
} from '../types/recurringTransaction';
import { categoryService } from '../services/categoryService';

interface RecurringTransactionModalProps {
  transaction: RecurringTransaction | null;
  accountId: string;
  onClose: (success: boolean) => void;
}

interface Category {
  id: string;
  name: string;
  type: string;
}

export default function RecurringTransactionModal({
  transaction,
  accountId,
  onClose,
}: RecurringTransactionModalProps) {
  const [loading, setLoading] = useState(false);
  const [categories, setCategories] = useState<Category[]>([]);
  const [formData, setFormData] = useState({
    description: transaction?.description || '',
    amount: transaction?.amount || 0,
    type: (transaction?.type || 'Expense') as TransactionType,
    categoryId: transaction?.categoryId || '',
    frequency: (transaction?.frequency || 'Monthly') as RecurrenceFrequency,
    dayOfMonth: transaction?.dayOfMonth || 1,
    startDate: transaction?.startDate
      ? new Date(transaction.startDate).toISOString().split('T')[0]
      : new Date().toISOString().split('T')[0],
    endDate: transaction?.endDate
      ? new Date(transaction.endDate).toISOString().split('T')[0]
      : '',
    isActive: transaction?.isActive ?? true,
  });

  useEffect(() => {
    loadCategories();
  }, []);

  const loadCategories = async () => {
    try {
      const data = await categoryService.getByAccount(accountId);
      setCategories(data);
    } catch (error) {
      console.error('Erro ao carregar categorias:', error);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);

    try {
      if (transaction) {
        await recurringTransactionService.update(transaction.id, accountId, {
          categoryId: formData.categoryId,
          description: formData.description,
          amount: formData.amount,
          frequency: formData.frequency,
          dayOfMonth: formData.dayOfMonth,
          endDate: formData.endDate || undefined,
          isActive: formData.isActive,
        });
      } else {
        await recurringTransactionService.create({
          accountId,
          categoryId: formData.categoryId,
          description: formData.description,
          amount: formData.amount,
          type: formData.type,
          frequency: formData.frequency,
          dayOfMonth: formData.dayOfMonth,
          startDate: new Date(formData.startDate).toISOString(),
          endDate: formData.endDate ? new Date(formData.endDate).toISOString() : undefined,
        });
      }
      onClose(true);
    } catch (error) {
      console.error('Erro ao salvar recorrência:', error);
      alert('Erro ao salvar recorrência');
    } finally {
      setLoading(false);
    }
  };

  const filteredCategories = categories.filter(
    (cat) => cat.type === formData.type
  );

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
      <div className="bg-white rounded-lg max-w-2xl w-full max-h-[90vh] overflow-y-auto">
        <div className="p-6">
          <h2 className="text-2xl font-bold mb-6">
            {transaction ? 'Editar Recorrência' : 'Nova Recorrência'}
          </h2>

          <form onSubmit={handleSubmit} className="space-y-4">
            <div>
              <label className="block text-sm font-medium mb-2">Descrição</label>
              <input
                type="text"
                value={formData.description}
                onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-blue-500"
                required
              />
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="block text-sm font-medium mb-2">Tipo</label>
                <select
                  value={formData.type}
                  onChange={(e) =>
                    setFormData({
                      ...formData,
                      type: e.target.value as TransactionType,
                      categoryId: '',
                    })
                  }
                  className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-blue-500"
                  disabled={!!transaction}
                >
                  <option value="Income">Receita</option>
                  <option value="Expense">Despesa</option>
                </select>
              </div>

              <div>
                <label className="block text-sm font-medium mb-2">Valor</label>
                <input
                  type="number"
                  step="0.01"
                  value={formData.amount}
                  onChange={(e) => setFormData({ ...formData, amount: parseFloat(e.target.value) })}
                  className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-blue-500"
                  required
                />
              </div>
            </div>

            <div>
              <label className="block text-sm font-medium mb-2">Categoria</label>
              <select
                value={formData.categoryId}
                onChange={(e) => setFormData({ ...formData, categoryId: e.target.value })}
                className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-blue-500"
                required
              >
                <option value="">Selecione uma categoria</option>
                {filteredCategories.map((cat) => (
                  <option key={cat.id} value={cat.id}>
                    {cat.name}
                  </option>
                ))}
              </select>
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="block text-sm font-medium mb-2">Frequência</label>
                <select
                  value={formData.frequency}
                  onChange={(e) =>
                    setFormData({ ...formData, frequency: e.target.value as RecurrenceFrequency })
                  }
                  className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-blue-500"
                >
                  {Object.entries(FREQUENCY_LABELS).map(([key, label]) => (
                    <option key={key} value={key}>
                      {label}
                    </option>
                  ))}
                </select>
              </div>

              {formData.frequency === 'Monthly' && (
                <div>
                  <label className="block text-sm font-medium mb-2">Dia do Mês</label>
                  <input
                    type="number"
                    min="1"
                    max="31"
                    value={formData.dayOfMonth}
                    onChange={(e) =>
                      setFormData({ ...formData, dayOfMonth: parseInt(e.target.value) })
                    }
                    className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-blue-500"
                    required
                  />
                </div>
              )}
            </div>

            <div className="grid grid-cols-2 gap-4">
              {!transaction && (
                <div>
                  <label className="block text-sm font-medium mb-2">Data de Início</label>
                  <input
                    type="date"
                    value={formData.startDate}
                    onChange={(e) => setFormData({ ...formData, startDate: e.target.value })}
                    className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-blue-500"
                    required
                  />
                </div>
              )}

              <div>
                <label className="block text-sm font-medium mb-2">
                  Data de Término (Opcional)
                </label>
                <input
                  type="date"
                  value={formData.endDate}
                  onChange={(e) => setFormData({ ...formData, endDate: e.target.value })}
                  className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-blue-500"
                />
              </div>
            </div>

            {transaction && (
              <div className="flex items-center gap-2">
                <input
                  type="checkbox"
                  id="isActive"
                  checked={formData.isActive}
                  onChange={(e) => setFormData({ ...formData, isActive: e.target.checked })}
                  className="w-4 h-4"
                />
                <label htmlFor="isActive" className="text-sm font-medium">
                  Recorrência ativa
                </label>
              </div>
            )}

            <div className="flex gap-4 pt-4">
              <button
                type="button"
                onClick={() => onClose(false)}
                className="flex-1 px-6 py-2 border rounded-lg hover:bg-gray-50 transition"
                disabled={loading}
              >
                Cancelar
              </button>
              <button
                type="submit"
                className="flex-1 bg-blue-600 text-white px-6 py-2 rounded-lg hover:bg-blue-700 transition disabled:opacity-50"
                disabled={loading}
              >
                {loading ? 'Salvando...' : transaction ? 'Atualizar' : 'Criar'}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}
