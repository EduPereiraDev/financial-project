export enum AlertType {
  MonthlySpendingLimit = 1,
  LowBalance = 2,
  GoalDeadlineApproaching = 3,
  RecurringTransactionProcessed = 4,
  InvitationAccepted = 5,
  UnusualSpending = 6,
  CategoryBudgetExceeded = 7,
}

export interface Alert {
  id: string;
  accountId: string;
  userId: string;
  type: AlertType;
  name: string;
  description: string;
  isActive: boolean;
  thresholdAmount?: number;
  thresholdDays?: number;
  categoryId?: string;
  categoryName?: string;
  createdAt: string;
  lastTriggeredAt?: string;
}

export interface CreateAlertRequest {
  accountId: string;
  type: AlertType;
  name: string;
  description: string;
  thresholdAmount?: number;
  thresholdDays?: number;
  categoryId?: string;
}

export interface UpdateAlertRequest {
  name?: string;
  description?: string;
  isActive?: boolean;
  thresholdAmount?: number;
  thresholdDays?: number;
  categoryId?: string;
}

export const alertTypeLabels: Record<AlertType, string> = {
  [AlertType.MonthlySpendingLimit]: 'Gastos Mensais Acima do Limite',
  [AlertType.LowBalance]: 'Saldo Baixo',
  [AlertType.GoalDeadlineApproaching]: 'Meta Próxima do Prazo',
  [AlertType.RecurringTransactionProcessed]: 'Transação Recorrente Processada',
  [AlertType.InvitationAccepted]: 'Convite Aceito',
  [AlertType.UnusualSpending]: 'Gasto Incomum',
  [AlertType.CategoryBudgetExceeded]: 'Orçamento de Categoria Excedido',
};

export const alertTypeDescriptions: Record<AlertType, string> = {
  [AlertType.MonthlySpendingLimit]: 'Receba um alerta quando seus gastos mensais ultrapassarem um valor definido',
  [AlertType.LowBalance]: 'Seja notificado quando o saldo da conta ficar abaixo de um valor mínimo',
  [AlertType.GoalDeadlineApproaching]: 'Receba lembretes quando uma meta estiver próxima do prazo',
  [AlertType.RecurringTransactionProcessed]: 'Seja notificado quando uma transação recorrente for processada',
  [AlertType.InvitationAccepted]: 'Receba uma notificação quando alguém aceitar seu convite',
  [AlertType.UnusualSpending]: 'Seja alertado sobre gastos fora do padrão',
  [AlertType.CategoryBudgetExceeded]: 'Receba um alerta quando o orçamento de uma categoria for excedido',
};
