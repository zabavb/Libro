export interface Subscription {
  id: string;
  userId: string;
  plan: "basic" | "premium" | "pro";
  status: "active" | "inactive" | "cancelled";
  autoRenewal: boolean;
}
