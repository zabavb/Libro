import * as z from "zod";

export const subscriptionSchema = z.object({
  email: z.string().email("Невірний email"),
  name: z.string().min(2, "Мінімум 2 символи"),
  plan: z.enum(["basic", "premium", "vip"], {
    required_error: "Оберіть план підписки",
  }),
  startDate: z.string().optional(),
  endDate: z.string().optional(),
});

export type SubscriptionFormValues = z.infer<typeof subscriptionSchema>;
