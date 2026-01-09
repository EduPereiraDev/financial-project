import api from './api'
import { Goal, CreateGoal, UpdateGoal, GoalSummary, CreateContribution, GoalContribution } from '@/types/goal'

export const goalService = {
  async getAll(status?: string): Promise<Goal[]> {
    const params = new URLSearchParams()
    if (status) params.append('status', status)
    const response = await api.get(`/goal?${params.toString()}`)
    return response.data
  },

  async getById(id: string): Promise<Goal> {
    const response = await api.get(`/goal/${id}`)
    return response.data
  },

  async getSummary(): Promise<GoalSummary> {
    const response = await api.get('/goal/summary')
    return response.data
  },

  async create(data: CreateGoal): Promise<Goal> {
    const response = await api.post('/goal', data)
    return response.data
  },

  async update(id: string, data: UpdateGoal): Promise<Goal> {
    const response = await api.put(`/goal/${id}`, data)
    return response.data
  },

  async delete(id: string): Promise<void> {
    await api.delete(`/goal/${id}`)
  },

  async addContribution(goalId: string, data: CreateContribution): Promise<Goal> {
    const response = await api.post(`/goal/${goalId}/contributions`, data)
    return response.data
  },

  async getContributions(goalId: string): Promise<GoalContribution[]> {
    const response = await api.get(`/goal/${goalId}/contributions`)
    return response.data
  }
}
