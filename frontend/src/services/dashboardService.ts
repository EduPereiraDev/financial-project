import api from './api';
import { DashboardStats } from '../types/dashboard';

export const dashboardService = {
  getStats: async (months: number = 6): Promise<DashboardStats> => {
    const response = await api.get(`/dashboard/stats?months=${months}`);
    return response.data;
  }
};
