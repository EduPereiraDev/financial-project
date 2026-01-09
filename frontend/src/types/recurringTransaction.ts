export type RecurrenceFrequency = 'Daily' | 'Weekly' | 'Biweekly' | 'Monthly' | 'Quarterly' | 'Yearly';

export type TransactionType = 'Income' | 'Expense';

export interface RecurringTransaction {
  id: string;
  accountId: string;
  categoryId: string;
  description: string;
  amount: number;
  type: TransactionType;
  frequency: RecurrenceFrequency;
  dayOfMonth: number;
  startDate: string;
  endDate?: string;
  isActive: boolean;
  lastExecutionDate?: string;
  nextExecutionDate?: string;
  createdAt: string;
  updatedAt: string;
  category?: {
    id: string;
    accountId: string;
    name: string;
    color: string;
    icon: string;
    type: string;
    createdAt: string;
  };
}

export interface CreateRecurringTransactionRequest {
  accountId: string;
  categoryId: string;
  description: string;
  amount: number;
  type: TransactionType;
  frequency: RecurrenceFrequency;
  dayOfMonth: number;
  startDate: string;
  endDate?: string;
}

export interface UpdateRecurringTransactionRequest {
  categoryId: string;
  description: string;
  amount: number;
  frequency: RecurrenceFrequency;
  dayOfMonth: number;
  endDate?: string;
  isActive: boolean;
}

export const FREQUENCY_LABELS: Record<RecurrenceFrequency, string> = {
  Daily: 'Diária',
  Weekly: 'Semanal',
  Biweekly: 'Quinzenal',
  Monthly: 'Mensal',
  Quarterly: 'Trimestral',
  Yearly: 'Anual',
};

export const FREQUENCY_ICONS: Record<RecurrenceFrequency, string> = {
  Daily: '📅',
  Weekly: '📆',
  Biweekly: '🗓️',
  Monthly: '📋',
  Quarterly: '📊',
  Yearly: '🎯',
};
