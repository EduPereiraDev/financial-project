import { useState, useEffect } from 'react';
import { PluggyConnect } from 'react-pluggy-connect';
import { bankingService } from '../services/bankingService';
import { BankConnection, BankTransaction, bankConnectionStatusLabels, bankConnectionStatusColors, BankConnectionStatus } from '../types/banking';

export default function BankingPage() {
  const [connections, setConnections] = useState<BankConnection[]>([]);
  const [pendingTransactions, setPendingTransactions] = useState<BankTransaction[]>([]);
  const [loading, setLoading] = useState(true);
  const [syncing, setSyncing] = useState<string | null>(null);
  const [connectToken, setConnectToken] = useState<string | null>(null);
  const [showConnectWidget, setShowConnectWidget] = useState(false);

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    setLoading(true);
    try {
      const [connectionsData, transactionsData] = await Promise.all([
        bankingService.getConnections(),
        bankingService.getPendingTransactions(),
      ]);
      setConnections(connectionsData);
      setPendingTransactions(transactionsData);
    } catch (error) {
      console.error('Erro ao carregar dados:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleOpenConnect = async () => {
    try {
      const token = await bankingService.createConnectToken();
      setConnectToken(token);
      setShowConnectWidget(true);
    } catch (error) {
      console.error('Erro ao criar Connect Token:', error);
      alert('Erro ao iniciar conexão bancária. Verifique as configurações do Pluggy.');
    }
  };

  const handleConnectSuccess = async (itemData: any) => {
    try {
      const accountId = localStorage.getItem('accountId');
      if (!accountId) {
        alert('Conta não encontrada');
        return;
      }

      await bankingService.createConnection({
        accountId,
        bankName: itemData.connector.name,
        bankCode: itemData.connector.id.toString(),
        institutionId: itemData.connector.id.toString(),
        itemId: itemData.item.id,
      });

      setShowConnectWidget(false);
      setConnectToken(null);
      await loadData();
      alert('Banco conectado com sucesso!');
    } catch (error) {
      console.error('Erro ao salvar conexão:', error);
      alert('Erro ao salvar conexão bancária');
    }
  };

  const handleConnectError = (error: any) => {
    console.error('Erro no Pluggy Connect:', error);
    setShowConnectWidget(false);
    setConnectToken(null);
    alert('Erro ao conectar com o banco. Tente novamente.');
  };

  const handleSync = async (connectionId: string) => {
    setSyncing(connectionId);
    try {
      const result = await bankingService.syncConnection(connectionId);
      if (result.success) {
        alert(`Sincronização concluída! ${result.newTransactions} novas transações encontradas.`);
        await loadData();
      } else {
        alert(`Erro na sincronização: ${result.errorMessage}`);
      }
    } catch (error: any) {
      alert(`Erro ao sincronizar: ${error.message}`);
    } finally {
      setSyncing(null);
    }
  };

  const handleDelete = async (connectionId: string) => {
    if (!window.confirm('Deseja realmente excluir esta conexão bancária?')) return;

    try {
      await bankingService.deleteConnection(connectionId);
      await loadData();
    } catch (error) {
      console.error('Erro ao excluir conexão:', error);
    }
  };

  const handleIgnoreTransaction = async (transactionId: string) => {
    try {
      await bankingService.ignoreTransaction(transactionId);
      await loadData();
    } catch (error) {
      console.error('Erro ao ignorar transação:', error);
    }
  };

  if (loading) {
    return (
      <div className="flex justify-center items-center h-64">
        <div className="text-gray-500">Carregando...</div>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8">
      {/* Pluggy Connect Widget */}
      {showConnectWidget && connectToken && (
        <PluggyConnect
          connectToken={connectToken}
          onSuccess={handleConnectSuccess}
          onError={handleConnectError}
          onClose={() => {
            setShowConnectWidget(false);
            setConnectToken(null);
          }}
        />
      )}

      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold">🏦 Integração Bancária</h1>
        <button 
          onClick={handleOpenConnect}
          className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
        >
          + Conectar Banco
        </button>
      </div>

      {/* Conexões Bancárias */}
      <div className="mb-8">
        <h2 className="text-2xl font-semibold mb-4">Suas Conexões</h2>
        {connections.length === 0 ? (
          <div className="bg-white rounded-lg shadow p-8 text-center">
            <p className="text-gray-500 mb-4">Você ainda não tem conexões bancárias</p>
            <p className="text-sm text-gray-400">
              Conecte sua conta bancária para sincronizar transações automaticamente!
            </p>
          </div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            {connections.map((connection) => (
              <div key={connection.id} className="bg-white rounded-lg shadow p-6">
                <div className="flex items-start justify-between mb-4">
                  <div>
                    <h3 className="text-xl font-semibold">{connection.bankName}</h3>
                    <p className="text-sm text-gray-500">Código: {connection.bankCode}</p>
                  </div>
                  <span className={`px-3 py-1 rounded-full text-sm ${bankConnectionStatusColors[connection.status]}`}>
                    {bankConnectionStatusLabels[connection.status]}
                  </span>
                </div>

                <div className="space-y-2 mb-4">
                  <p className="text-sm text-gray-600">
                    Conectado em: {new Date(connection.connectedAt).toLocaleDateString('pt-BR')}
                  </p>
                  {connection.lastSyncAt && (
                    <p className="text-sm text-gray-600">
                      Última sincronização: {new Date(connection.lastSyncAt).toLocaleString('pt-BR')}
                    </p>
                  )}
                  {connection.pendingTransactionsCount > 0 && (
                    <p className="text-sm font-semibold text-blue-600">
                      {connection.pendingTransactionsCount} transações pendentes
                    </p>
                  )}
                </div>

                <div className="flex gap-2">
                  <button
                    onClick={() => handleSync(connection.id)}
                    disabled={syncing === connection.id || connection.status === BankConnectionStatus.Syncing}
                    className="flex-1 px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 disabled:bg-blue-300"
                  >
                    {syncing === connection.id ? 'Sincronizando...' : '🔄 Sincronizar'}
                  </button>
                  <button
                    onClick={() => handleDelete(connection.id)}
                    className="px-4 py-2 bg-red-600 text-white rounded hover:bg-red-700"
                  >
                    Excluir
                  </button>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>

      {/* Transações Pendentes */}
      {pendingTransactions.length > 0 && (
        <div>
          <h2 className="text-2xl font-semibold mb-4">Transações Pendentes</h2>
          <div className="bg-white rounded-lg shadow overflow-hidden">
            <table className="min-w-full divide-y divide-gray-200">
              <thead className="bg-gray-50">
                <tr>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Banco</th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Descrição</th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Data</th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Valor</th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Ações</th>
                </tr>
              </thead>
              <tbody className="bg-white divide-y divide-gray-200">
                {pendingTransactions.map((transaction) => (
                  <tr key={transaction.id}>
                    <td className="px-6 py-4 whitespace-nowrap text-sm">{transaction.bankName}</td>
                    <td className="px-6 py-4 text-sm">{transaction.description}</td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm">
                      {new Date(transaction.date).toLocaleDateString('pt-BR')}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm">
                      <span className={transaction.type === 1 ? 'text-red-600' : 'text-green-600'}>
                        {transaction.type === 1 ? '-' : '+'} R$ {transaction.amount.toFixed(2)}
                      </span>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm space-x-2">
                      <button className="text-blue-600 hover:text-blue-800">Importar</button>
                      <button
                        onClick={() => handleIgnoreTransaction(transaction.id)}
                        className="text-gray-600 hover:text-gray-800"
                      >
                        Ignorar
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      )}
    </div>
  );
}
