import { Subscription } from '@/types';
import { SubscriptionFormData } from '@/utils';

export const SubscriptionFormDataToSubscription = (
  form: SubscriptionFormData,
  id?: string,
): Subscription => ({
  ...form,
  id: id ?? '00000000-0000-0000-0000-000000000000',
  title: form.title ?? null,
  expirationDays: form.expirationDays ?? null,
  price: form.price ?? null,
  description: form.description ?? null,
  imageUrl: '',
  image: form.image ?? null,
});
