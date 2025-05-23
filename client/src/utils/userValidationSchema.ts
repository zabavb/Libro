import { z } from 'zod';
import { RoleView } from '../types';

const today = new Date();
const minDateOfBirth = new Date(
  today.getFullYear() - 90,
  today.getMonth(),
  today.getDate(),
);

export const userSchema = z
  .object({
    lastName: z
      .string()
      .min(2, 'Last name must be at least 2 characters')
      .max(50, 'Last name too long')
      .optional(),
    firstName: z
      .string()
      .min(2, 'First name must be at least 2 characters')
      .max(50, 'First name too long'),
    email: z.string().email('Invalid email').optional().or(z.literal('')),
    phoneNumber: z
      .string()
      .min(10, 'Phone number must be at least 10 digits')
      .max(15, 'Phone number too long')
      .optional()
      .or(z.literal('')),
    dateOfBirth: z
      .string()
      .refine((val) => !isNaN(Date.parse(val)), 'Invalid date')
      .refine(
        (val) => new Date(val) >= minDateOfBirth,
        `Date of birth must be at least ${minDateOfBirth.getFullYear()} or later`,
      )
      .refine(
        (val) => new Date(val) <= today,
        'Date of birth cannot be in the future',
      ),
    role: z.nativeEnum(RoleView, {
      errorMap: () => ({ message: 'Invalid role selected' }),
    }),
  })
  .refine((data) => data.email !== '' || data.phoneNumber !== '', {
    message: 'Either Email or Phone Number must be provided',
    path: ['email'],
  });

export type UserFormData = z.infer<typeof userSchema>;

export const userProfileSchema = z
  .object({
    lastName: z
      .string()
      .min(2, 'Last name must be at least 2 characters')
      .max(50, 'Last name too long')
      .optional(),
    firstName: z
      .string()
      .min(2, 'First name must be at least 2 characters')
      .max(50, 'First name too long'),
    email: z.string().email('Invalid email').optional().or(z.literal('')),
    phoneNumber: z
      .string()
      .min(10, 'Phone number must be at least 10 digits')
      .max(15, 'Phone number too long')
      .optional()
      .or(z.literal('')),
    password: z
      .string()
      .min(8, 'Password must be between 8 and 100 characters')
      .regex(
        /^(?=.*[A-Za-z])(?=.*\d).{8,}$/,
        'Password must be at least 8 characters long and contain at least one letter and one number',
      ),
    confirmPassword: z.string(),
    dateOfBirth: z
      .string()
      .refine((val) => !isNaN(Date.parse(val)), 'Invalid date')
      .refine(
        (val) => new Date(val) >= minDateOfBirth,
        `Date of birth must be at least ${minDateOfBirth.getFullYear()} or later`,
      )
      .refine(
        (val) => new Date(val) <= today,
        'Date of birth cannot be in the future',
      ),
  })
  .refine((data) => data.email !== '' || data.phoneNumber !== '', {
    message: 'Either Email or Phone Number must be provided',
    path: ['email'],
  })
  .refine((data) => data.password === data.confirmPassword, {
    message: 'Passwords do not match',
    path: ['confirmPassword'],
  });

export type UserProfileFormData = z.infer<typeof userProfileSchema>;
