export interface Subscription {
  id: string;
  title: string;
  expirationDays: number;
  price: number;
  description: string | null;
  imageUrl: string;
  image: File | null;
}
