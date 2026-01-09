import api from './api';
import { Alert, CreateAlertRequest, UpdateAlertRequest } from '../types/alert';

export const alertService = {
  create: async (request: CreateAlertRequest): Promise<Alert> => {
    const response = await api.post('/alerts', request);
    return response.data;
  },

  getByAccount: async (accountId: string): Promise<Alert[]> => {
    const response = await api.get(`/alerts/account/${accountId}`);
    return response.data;
  },

  getById: async (id: string): Promise<Alert> => {
    const response = await api.get(`/alerts/${id}`);
    return response.data;
  },

  update: async (id: string, request: UpdateAlertRequest): Promise<Alert> => {
    const response = await api.put(`/alerts/${id}`, request);
    return response.data;
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/alerts/${id}`);
  },

  toggle: async (id: string): Promise<void> => {
    await api.put(`/alerts/${id}/toggle`);
  },
};
