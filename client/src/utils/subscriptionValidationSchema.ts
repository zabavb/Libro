import { z } from 'zod';

export const subscriptionSchema = z.object({
  title: z
    .string()
    .min(2, 'Title must be at least 2 characters')
    .max(30, 'Title too long'),
  expirationDays: z.coerce
    .number()
    .int()
    .min(1, 'Expiration days must be at least 1')
    .max(730, 'Expiration days are too long'),
  price: z.coerce
    .number()
    .min(0, 'Price must be at least 0')
    .max(5000, 'Price is too high'),
  subdescription: z
    .string()
    .min(2, 'Subdescription must be at least 2 characters')
    .max(40, 'Subdescription is too long')
    .optional(),
  description: z
    .string()
    .min(2, 'Description must be at least 2 characters')
    .max(500, 'Description is too long')
    .optional(),
  image: z
    .instanceof(File)
    .optional()
    .refine(
      (file) => {
        if (!file) return true;
        const allowedFormats = ['image/png', 'image/jpeg', 'image/jpg'];
        return allowedFormats.includes(file.type);
      },
      {
        message: 'Image must be in PNG, JPG, or JPEG format.',
      },
    ),
});

export type SubscriptionFormData = z.infer<typeof subscriptionSchema>;
