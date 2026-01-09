import api from './api';

export interface Category {
  id: string;
  accountId: string;
  name: string;
  color: string;
  icon: string;
  type: string;
  createdAt: string;
}

export const categoryService = {
  async getByAccount(accountId: string): Promise<Category[]> {
    const response = await api.get(`/categories/account/${accountId}`);
    return response.data;
  },
};
