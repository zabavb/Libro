export type SubscriptionPlan = "basic" | "premium" | "vip";

export interface Subscription {
  id: string;
  email: string;
  name: string;
  plan: SubscriptionPlan;
  startDate: string;
  endDate: string;
  status: "active" | "expired" | "canceled";
}