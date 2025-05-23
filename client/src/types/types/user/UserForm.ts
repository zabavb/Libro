import { Role } from "@/types";

export interface UserForm {
  id: string;
  lastName: string | null;
  firstName: string;
  email: string | null;
  phoneNumber: string | null;
  dateOfBirth: Date | null;
  role: Role;
  imageUrl: string | null;

  orders: OrderDetailsSnippet[];
  feedbacksCount: number;
  feedbacks: FeedbackDetailsSnippet[];
  subscriptions: SubscriptionDetailsSnippet[];
}
interface OrderDetailsSnippet {
  orderUiId: string;
  bookNames: string[];
  price: number;
}

interface FeedbackDetailsSnippet {
  headLabel: string;
  rating: number;
  comment: string;
  date: Date;
}

interface SubscriptionDetailsSnippet {
  title: string;
  description: string;
  imageUrl: string;
}
