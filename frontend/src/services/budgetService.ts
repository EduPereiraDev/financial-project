import api from './api'
import { Budget, CreateBudget, UpdateBudget, BudgetSummary } from '@/types/budget'

export const budgetService = {
  async getAll(month?: number, year?: number): Promise<Budget[]> {
    const params = new URLSearchParams()
    if (month) params.append('month', month.toString())
    if (year) params.append('year', year.toString())
    const response = await api.get(`/budget?${params.toString()}`)
    return response.data
  },

  async getById(id: string): Promise<Budget> {
    const response = await api.get(`/budget/${id}`)
    return response.data
  },

  async getSummary(month?: number, year?: number): Promise<BudgetSummary> {
    const params = new URLSearchParams()
    if (month) params.append('month', month.toString())
    if (year) params.append('year', year.toString())
    const response = await api.get(`/budget/summary?${params.toString()}`)
    return response.data
  },

  async create(data: CreateBudget): Promise<Budget> {
    const response = await api.post('/budget', data)
    return response.data
  },

  async update(id: string, data: UpdateBudget): Promise<Budget> {
    const response = await api.put(`/budget/${id}`, data)
    return response.data
  },

  async delete(id: string): Promise<void> {
    await api.delete(`/budget/${id}`)
  }
}
