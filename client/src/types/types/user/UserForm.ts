export interface UserForm {
  id: string;
  lastName: string | null;
  firstName: string;
  email: string | null;
  phoneNumber: string | null;
  dateOfBirth: Date | null;
  role: number;
  imageUrl: string | null;

  orders: OrderDetailsSnippet[];
  feedbacksCount: number;
  feedbacks: FeedbackDetailsSnippet[];
  // Subscriptions: SubscriptionDetailsSnippet[];
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

/* interface SubscriptionDetailsSnippet {
  title: string;
  description: string;
  imageUrl: string;
} */
