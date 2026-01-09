export enum BankConnectionStatus {
  Connected = 1,
  Disconnected = 2,
  Error = 3,
  Syncing = 4,
  PendingAuth = 5,
}

export enum BankTransactionType {
  Debit = 1,
  Credit = 2,
}

export enum BankTransactionStatus {
  Pending = 1,
  Imported = 2,
  Ignored = 3,
  Duplicate = 4,
}

export interface BankConnection {
  id: string;
  accountId: string;
  bankName: string;
  bankCode: string;
  status: BankConnectionStatus;
  connectedAt: string;
  lastSyncAt?: string;
  autoSync: boolean;
  pendingTransactionsCount: number;
}

export interface BankTransaction {
  id: string;
  bankConnectionId: string;
  bankName: string;
  description: string;
  amount: number;
  date: string;
  type: BankTransactionType;
  status: BankTransactionStatus;
}

export interface CreateBankConnectionRequest {
  accountId: string;
  bankName: string;
  bankCode: string;
  institutionId: string;
  itemId: string;
}

export interface SyncResult {
  success: boolean;
  transactionsFound: number;
  newTransactions: number;
  errorMessage?: string;
}

export const bankConnectionStatusLabels: Record<BankConnectionStatus, string> = {
  [BankConnectionStatus.Connected]: 'Conectado',
  [BankConnectionStatus.Disconnected]: 'Desconectado',
  [BankConnectionStatus.Error]: 'Erro',
  [BankConnectionStatus.Syncing]: 'Sincronizando',
  [BankConnectionStatus.PendingAuth]: 'Aguardando Autorização',
};

export const bankConnectionStatusColors: Record<BankConnectionStatus, string> = {
  [BankConnectionStatus.Connected]: 'bg-green-100 text-green-800',
  [BankConnectionStatus.Disconnected]: 'bg-gray-100 text-gray-800',
  [BankConnectionStatus.Error]: 'bg-red-100 text-red-800',
  [BankConnectionStatus.Syncing]: 'bg-blue-100 text-blue-800',
  [BankConnectionStatus.PendingAuth]: 'bg-yellow-100 text-yellow-800',
};
