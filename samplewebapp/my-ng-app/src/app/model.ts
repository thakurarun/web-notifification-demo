export interface PushNotification {
  title: string;
  lang: string;
  body: string;
  tag: string;
  image: string;
  icon: string;
  badge: string;
  timestamp: Date;
  requireInteraction: boolean;
  actions: NotificationAction[];
}

export interface NotificationAction {
  action: string;
  title: string;
}
