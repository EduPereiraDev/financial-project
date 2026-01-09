import { useState } from 'react';
import { alertService } from '../services/alertService';
import { AlertType, alertTypeLabels, alertTypeDescriptions } from '../types/alert';

interface CreateAlertModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSuccess: () => void;
}

export default function CreateAlertModal({ isOpen, onClose, onSuccess }: CreateAlertModalProps) {
  const [formData, setFormData] = useState({
    type: AlertType.MonthlySpendingLimit,
    name: '',
    description: '',
    thresholdAmount: '',
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      const accountId = localStorage.getItem('accountId');
      if (!accountId) {
        setError('Conta não encontrada');
        return;
      }

      await alertService.create({
        accountId,
        type: formData.type,
        name: formData.name,
        description: formData.description,
        thresholdAmount: formData.thresholdAmount ? parseFloat(formData.thresholdAmount) : undefined,
      });

      onSuccess();
      onClose();
      setFormData({
        type: AlertType.MonthlySpendingLimit,
        name: '',
        description: '',
        thresholdAmount: '',
      });
    } catch (err: any) {
      setError(err.response?.data?.message || 'Erro ao criar alerta');
    } finally {
      setLoading(false);
    }
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
      <div className="bg-white rounded-lg p-6 max-w-md w-full mx-4">
        <div className="flex justify-between items-center mb-4">
          <h2 className="text-2xl font-bold">Novo Alerta</h2>
          <button
            onClick={onClose}
            className="text-gray-500 hover:text-gray-700 text-2xl"
          >
            ×
          </button>
        </div>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Tipo de Alerta
            </label>
            <select
              value={formData.type}
              onChange={(e) => setFormData({ ...formData, type: parseInt(e.target.value) as AlertType })}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            >
              {Object.entries(alertTypeLabels).map(([key, label]) => (
                <option key={key} value={key}>
                  {label}
                </option>
              ))}
            </select>
            <p className="text-xs text-gray-500 mt-1">
              {alertTypeDescriptions[formData.type]}
            </p>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Nome do Alerta
            </label>
            <input
              type="text"
              value={formData.name}
              onChange={(e) => setFormData({ ...formData, name: e.target.value })}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              placeholder="Ex: Limite de gastos mensais"
              required
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Descrição
            </label>
            <textarea
              value={formData.description}
              onChange={(e) => setFormData({ ...formData, description: e.target.value })}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              placeholder="Descreva quando você quer ser notificado"
              rows={3}
              required
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">
              Valor Limite (R$)
            </label>
            <input
              type="number"
              step="0.01"
              value={formData.thresholdAmount}
              onChange={(e) => setFormData({ ...formData, thresholdAmount: e.target.value })}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              placeholder="0.00"
            />
          </div>

          {error && (
            <div className="bg-red-50 text-red-600 p-3 rounded-md text-sm">
              {error}
            </div>
          )}

          <div className="flex gap-3">
            <button
              type="button"
              onClick={onClose}
              className="flex-1 px-4 py-2 border border-gray-300 rounded-md text-gray-700 hover:bg-gray-50"
            >
              Cancelar
            </button>
            <button
              type="submit"
              disabled={loading}
              className="flex-1 px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 disabled:bg-blue-300"
            >
              {loading ? 'Criando...' : 'Criar Alerta'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
