export interface DashboardStats {
  totalIncome: number;
  totalExpenses: number;
  balance: number;
  previousMonthIncome: number;
  previousMonthExpenses: number;
  monthlyData: MonthlyData[];
  categoryExpenses: CategoryExpense[];
  dailyBalance: DailyBalance[];
}

export interface MonthlyData {
  month: string;
  income: number;
  expenses: number;
}

export interface CategoryExpense {
  category: string;
  amount: number;
  color: string;
}

export interface DailyBalance {
  date: string;
  balance: number;
}
