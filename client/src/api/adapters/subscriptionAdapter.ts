/* import { Subscription } from '../../types';
import { SubscriptionFormData } from '../../utils';

export const SubscriptionFormDataToSubscription = (
  form: SubscriptionFormData,
): Subscription => ({
  ...form,
  id: '00000000-0000-0000-0000-000000000000',
  title: form.title ?? null,
  expirationDays: form.expirationDays ?? 0,
  description: form.description ?? null,
  image: form.image ?? null,
  price: form.price ?? 0,
  imageUrl: '',
});
 */