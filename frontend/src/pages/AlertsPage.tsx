import { useState, useEffect } from 'react';
import { alertService } from '../services/alertService';
import { Alert, AlertType, alertTypeLabels } from '../types/alert';

export default function AlertsPage() {
  const [alerts, setAlerts] = useState<Alert[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadAlerts();
  }, []);

  const loadAlerts = async () => {
    const accountId = localStorage.getItem('accountId');
    if (!accountId) return;
    
    setLoading(true);
    try {
      const data = await alertService.getByAccount(accountId);
      setAlerts(data);
    } catch (error) {
      console.error('Erro ao carregar alertas:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleToggle = async (id: string) => {
    try {
      await alertService.toggle(id);
      setAlerts(alerts.map(a => a.id === id ? { ...a, isActive: !a.isActive } : a));
    } catch (error) {
      console.error('Erro ao alternar alerta:', error);
    }
  };

  const handleDelete = async (id: string) => {
    if (!window.confirm('Deseja realmente excluir este alerta?')) return;
    
    try {
      await alertService.delete(id);
      setAlerts(alerts.filter(a => a.id !== id));
    } catch (error) {
      console.error('Erro ao excluir alerta:', error);
    }
  };

  if (loading) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="text-gray-500">Carregando alertas...</div>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold">Alertas e Notificações</h1>
        <button className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700">
          + Novo Alerta
        </button>
      </div>

      {alerts.length === 0 ? (
        <div className="bg-white rounded-lg shadow p-8 text-center">
          <p className="text-gray-500 mb-4">Você ainda não tem alertas configurados</p>
          <p className="text-sm text-gray-400">
            Crie alertas para ser notificado sobre gastos, saldo baixo e muito mais!
          </p>
        </div>
      ) : (
        <div className="space-y-4">
          {alerts.map((alert) => (
            <div
              key={alert.id}
              className="bg-white rounded-lg shadow p-6 hover:shadow-lg transition-shadow"
            >
              <div className="flex items-start justify-between">
                <div className="flex-1">
                  <div className="flex items-center gap-3 mb-2">
                    <h3 className="text-xl font-semibold">{alert.name}</h3>
                    <span className={`px-3 py-1 rounded-full text-sm ${
                      alert.isActive 
                        ? 'bg-green-100 text-green-800' 
                        : 'bg-gray-100 text-gray-800'
                    }`}>
                      {alert.isActive ? 'Ativo' : 'Inativo'}
                    </span>
                  </div>
                  
                  <p className="text-gray-600 mb-2">{alert.description}</p>
                  
                  <div className="flex items-center gap-4 text-sm text-gray-500">
                    <span className="bg-blue-50 text-blue-700 px-2 py-1 rounded">
                      {alertTypeLabels[alert.type as AlertType]}
                    </span>
                    {alert.thresholdAmount && (
                      <span>Limite: R$ {alert.thresholdAmount.toFixed(2)}</span>
                    )}
                    {alert.categoryName && (
                      <span>Categoria: {alert.categoryName}</span>
                    )}
                  </div>

                  {alert.lastTriggeredAt && (
                    <p className="text-xs text-gray-400 mt-2">
                      Último disparo: {new Date(alert.lastTriggeredAt).toLocaleString('pt-BR')}
                    </p>
                  )}
                </div>

                <div className="flex gap-2 ml-4">
                  <button
                    onClick={() => handleToggle(alert.id)}
                    className={`px-4 py-2 rounded ${
                      alert.isActive
                        ? 'bg-gray-200 text-gray-700 hover:bg-gray-300'
                        : 'bg-green-600 text-white hover:bg-green-700'
                    }`}
                  >
                    {alert.isActive ? 'Desativar' : 'Ativar'}
                  </button>
                  <button
                    onClick={() => handleDelete(alert.id)}
                    className="px-4 py-2 bg-red-600 text-white rounded hover:bg-red-700"
                  >
                    Excluir
                  </button>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
