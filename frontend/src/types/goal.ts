export interface Goal {
  id: string
  userId: string
  name: string
  description: string
  targetAmount: number
  currentAmount: number
  targetDate: string
  status: string
  priority: string
  imageUrl?: string
  percentageComplete: number
  remainingAmount: number
  daysRemaining: number
  requiredMonthlyContribution: number
  createdAt: string
  updatedAt: string
  recentContributions: GoalContribution[]
}

export interface CreateGoal {
  name: string
  description: string
  targetAmount: number
  targetDate: string
  priority: string
  imageUrl?: string
}

export interface UpdateGoal {
  name?: string
  description?: string
  targetAmount?: number
  targetDate?: string
  status?: string
  priority?: string
  imageUrl?: string
}

export interface GoalContribution {
  id: string
  goalId: string
  amount: number
  note?: string
  contributedAt: string
}

export interface CreateContribution {
  amount: number
  note?: string
}

export interface GoalSummary {
  totalGoals: number
  activeGoals: number
  completedGoals: number
  totalTargetAmount: number
  totalCurrentAmount: number
  overallProgress: number
  goals: Goal[]
}
