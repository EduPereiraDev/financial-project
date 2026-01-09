import api from './api';
import { NotificationListItem } from '../types/notification';

export const notificationService = {
  getAll: async (unreadOnly: boolean = false, limit: number = 50): Promise<NotificationListItem[]> => {
    const response = await api.get('/notifications', {
      params: { unreadOnly, limit }
    });
    return response.data;
  },

  getUnreadCount: async (): Promise<number> => {
    const response = await api.get('/notifications/unread-count');
    return response.data.count;
  },

  markAsRead: async (id: string): Promise<void> => {
    await api.put(`/notifications/${id}/read`);
  },

  markAllAsRead: async (): Promise<void> => {
    await api.put('/notifications/read-all');
  },

  delete: async (id: string): Promise<void> => {
    await api.delete(`/notifications/${id}`);
  },
};
