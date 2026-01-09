export interface Budget {
  id: string
  userId: string
  categoryId: string
  categoryName: string
  categoryColor: string
  amount: number
  period: string
  month: number
  year: number
  spent: number
  remaining: number
  percentageUsed: number
  createdAt: string
  updatedAt: string
}

export interface CreateBudget {
  categoryId: string
  amount: number
  period: string
  month: number
  year: number
}

export interface UpdateBudget {
  amount: number
}

export interface BudgetSummary {
  totalBudget: number
  totalSpent: number
  totalRemaining: number
  overallPercentage: number
  categoriesCount: number
  overBudgetCount: number
  budgets: Budget[]
}
