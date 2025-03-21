import { useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import axios from "axios";

const subscriptionSchema = z.object({
  email: z.string().email("Невірний email"),
  name: z.string().min(2, "Мінімум 2 символи"),
  plan: z.enum(["basic", "premium", "vip"], {
    required_error: "Оберіть план підписки",
  }),
});

type SubscriptionFormValues = z.infer<typeof subscriptionSchema>;

export default function SubscriptionForm() {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<SubscriptionFormValues>({
    resolver: zodResolver(subscriptionSchema),
  });

  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState("");

  const onSubmit = async (data: SubscriptionFormValues) => {
    setLoading(true);
    setMessage("");
    try {
      await axios.post("/api/subscriptions", data);
      setMessage("Підписка успішно оформлена!");
    } catch (error) {
      setMessage("Помилка при оформленні підписки");
    }
    setLoading(false);
  };

  return (
    <div className="max-w-md mx-auto bg-white p-6 rounded-lg shadow-md">
      <h2 className="text-xl font-bold mb-4">Оформлення підписки</h2>
      <form onSubmit={handleSubmit(onSubmit)}>
        <div className="mb-4">
          <label className="block text-gray-700">Email</label>
          <input
            type="email"
            {...register("email")}
            className="w-full p-2 border rounded"
          />
          {errors.email && <p className="text-red-500 text-sm">{errors.email.message}</p>}
        </div>

        <div className="mb-4">
          <label className="block text-gray-700">Ім'я</label>
          <input
            type="text"
            {...register("name")}
            className="w-full p-2 border rounded"
          />
          {errors.name && <p className="text-red-500 text-sm">{errors.name.message}</p>}
        </div>

        <div className="mb-4">
          <label className="block text-gray-700">Оберіть план</label>
          <select {...register("plan")} className="w-full p-2 border rounded">
            <option value="">-- Виберіть --</option>
            <option value="basic">Basic</option>
            <option value="premium">Premium</option>
            <option value="vip">VIP</option>
          </select>
          {errors.plan && <p className="text-red-500 text-sm">{errors.plan.message}</p>}
        </div>

        <button
          type="submit"
          className="bg-blue-500 text-white p-2 rounded w-full"
          disabled={loading}
        >
          {loading ? "Обробка..." : "Підписатися"}
        </button>
      </form>

      {message && <p className="mt-4 text-center text-green-500">{message}</p>}
    </div>
  );
}
