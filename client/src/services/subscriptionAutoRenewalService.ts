import subscriptionService from "./subscriptionService";

/**
 * Handle the auto-renewal of subscriptions.
 * It checks all subscriptions and renews the ones with auto-renewal enabled.
 */
export const handleAutoRenewal = async () => {
  try {
    // Fetch all subscriptions
    const subscriptions = await subscriptionService.getAll();

    // Filter subscriptions that have auto-renewal enabled
    const autoRenewableSubscriptions = subscriptions.filter((sub) => sub.autoRenewal);

    // Renew each subscription
    for (const sub of autoRenewableSubscriptions) {
      console.log(`Renewing subscription for user ${sub.userId}`);
      await subscriptionService.create({ userId: sub.userId, plan: sub.plan });
    }
  } catch (error) {
    console.error("Error with auto-renewal:", error);
  }
};
