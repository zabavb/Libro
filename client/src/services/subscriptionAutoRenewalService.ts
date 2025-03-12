import { subscriptionApi } from "../api/subscriptionApi";

export const autoRenewSubscription = async (id: string) => {
  try {
    const existingSubscription = await subscriptionApi.fetchById(id);
    if (existingSubscription.data.status !== "active") {
      throw new Error("Не можна продовжити неактивну підписку");
    }

    const newEndDate = new Date(existingSubscription.data.endDate);
    newEndDate.setMonth(newEndDate.getMonth() + 1); // продовження на місяць

    await subscriptionApi.update(id, { endDate: newEndDate.toISOString() });

    return "Підписка успішно продовжена!";
  } catch (error) {
    throw new Error("Помилка при продовженні підписки");
  }
};
