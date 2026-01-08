export interface User {
  id: string
  email: string
  name: string
  createdAt: string
}

export interface AuthResponse {
  token: string
  user: User
}

export interface LoginRequest {
  email: string
  password: string
}

export interface RegisterRequest {
  email: string
  password: string
  name: string
}

export interface Account {
  id: string
  name: string
  type: 'Personal' | 'Shared'
  ownerId: string
  ownerName: string
  createdAt: string
  members: AccountMember[]
}

export interface AccountMember {
  id: string
  userId: string
  userName: string
  userEmail: string
  role: 'Owner' | 'Editor' | 'Viewer'
  joinedAt: string
}

export interface Category {
  id: string
  accountId: string
  name: string
  color: string
  icon: string
  type: 'Income' | 'Expense'
  createdAt: string
}

export interface Transaction {
  id: string
  accountId: string
  accountName: string
  userId: string
  userName: string
  categoryId: string
  categoryName: string
  categoryColor: string
  categoryIcon: string
  amount: number
  description: string
  date: string
  type: 'Income' | 'Expense'
  createdAt: string
  updatedAt: string
}

export interface PagedResult<T> {
  items: T[]
  totalCount: number
  page: number
  pageSize: number
  totalPages: number
}
