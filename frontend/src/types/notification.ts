export enum NotificationType {
  Info = 1,
  Warning = 2,
  Error = 3,
  Success = 4,
  Alert = 5,
}

export interface Notification {
  id: string;
  userId: string;
  alertId?: string;
  type: NotificationType;
  title: string;
  message: string;
  isRead: boolean;
  createdAt: string;
  readAt?: string;
  actionUrl?: string;
  metadata?: string;
}

export interface NotificationListItem {
  id: string;
  type: NotificationType;
  title: string;
  message: string;
  isRead: boolean;
  createdAt: string;
  actionUrl?: string;
}

export const notificationTypeColors: Record<NotificationType, string> = {
  [NotificationType.Info]: 'bg-blue-100 text-blue-800',
  [NotificationType.Warning]: 'bg-yellow-100 text-yellow-800',
  [NotificationType.Error]: 'bg-red-100 text-red-800',
  [NotificationType.Success]: 'bg-green-100 text-green-800',
  [NotificationType.Alert]: 'bg-orange-100 text-orange-800',
};

export const notificationTypeIcons: Record<NotificationType, string> = {
  [NotificationType.Info]: '💡',
  [NotificationType.Warning]: '⚠️',
  [NotificationType.Error]: '❌',
  [NotificationType.Success]: '✅',
  [NotificationType.Alert]: '🔔',
};
