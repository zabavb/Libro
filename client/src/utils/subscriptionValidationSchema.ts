import { z } from 'zod';

export const subscriptionSchema = z.object({
  title: z
    .string()
    .min(2, 'Titile must be at least 2 characters')
    .max(50, 'Title too long'),
  expirationDays: z
    .number()
    .int()
    .min(1, 'Expiration days must be at least 1')
    .max(730, 'Expiration days are too long'),
  price: z
    .number()
    .min(0, 'Price must be at least 0')
    .max(5000, 'Price are too high'),
  description: z
    .string()
    .min(2, 'Description must be at least 2 characters')
    .max(50, 'Description is too long')
    .optional(),
  image: z.instanceof(File),
});

export type SubscriptionFormData = z.infer<typeof subscriptionSchema>;
