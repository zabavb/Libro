export interface Notification {
  id: string;
  message: string | null;
  type: 'success' | 'error' | 'info';
}

export interface NotificationData {
  message: string | null;
  type: 'success' | 'error' | 'info';
}
