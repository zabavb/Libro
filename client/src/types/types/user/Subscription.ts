export interface Subscription {
  id: string;
  title: string;
  expirationDays: number;
  price: number;
  subdescription: string | null;
  description: string | null;
  imageUrl: string;
  image: File | null;
}
