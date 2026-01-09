import api from './api';
import {
  RecurringTransaction,
  CreateRecurringTransactionRequest,
  UpdateRecurringTransactionRequest,
} from '../types/recurringTransaction';

export const recurringTransactionService = {
  async getByAccount(accountId: string): Promise<RecurringTransaction[]> {
    const response = await api.get(`/recurringtransactions/account/${accountId}`);
    return response.data;
  },

  async getById(id: string, accountId: string): Promise<RecurringTransaction> {
    const response = await api.get(`/recurringtransactions/${id}?accountId=${accountId}`);
    return response.data;
  },

  async create(data: CreateRecurringTransactionRequest): Promise<RecurringTransaction> {
    const response = await api.post('/recurringtransactions', data);
    return response.data;
  },

  async update(
    id: string,
    accountId: string,
    data: UpdateRecurringTransactionRequest
  ): Promise<RecurringTransaction> {
    const response = await api.put(`/recurringtransactions/${id}?accountId=${accountId}`, data);
    return response.data;
  },

  async delete(id: string, accountId: string): Promise<void> {
    await api.delete(`/recurringtransactions/${id}?accountId=${accountId}`);
  },

  async process(): Promise<{ message: string; count: number }> {
    const response = await api.post('/recurringtransactions/process');
    return response.data;
  },
};
