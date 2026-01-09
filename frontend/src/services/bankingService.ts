import api from './api';
import { BankConnection, BankTransaction, CreateBankConnectionRequest, SyncResult } from '../types/banking';

export const bankingService = {
  getConnections: async (): Promise<BankConnection[]> => {
    const response = await api.get('/banking/connections');
    return response.data;
  },

  getConnection: async (id: string): Promise<BankConnection> => {
    const response = await api.get(`/banking/connections/${id}`);
    return response.data;
  },

  createConnection: async (request: CreateBankConnectionRequest): Promise<BankConnection> => {
    const response = await api.post('/banking/connections', request);
    return response.data;
  },

  updateConnection: async (id: string, autoSync: boolean): Promise<void> => {
    await api.put(`/banking/connections/${id}`, { autoSync });
  },

  deleteConnection: async (id: string): Promise<void> => {
    await api.delete(`/banking/connections/${id}`);
  },

  syncConnection: async (id: string): Promise<SyncResult> => {
    const response = await api.post(`/banking/connections/${id}/sync`);
    return response.data;
  },

  getPendingTransactions: async (): Promise<BankTransaction[]> => {
    const response = await api.get('/banking/transactions/pending');
    return response.data;
  },

  importTransaction: async (bankTransactionId: string, categoryId: string): Promise<void> => {
    await api.post('/banking/transactions/import', {
      bankTransactionId,
      categoryId,
    });
  },

  ignoreTransaction: async (id: string): Promise<void> => {
    await api.post(`/banking/transactions/${id}/ignore`);
  },
};
