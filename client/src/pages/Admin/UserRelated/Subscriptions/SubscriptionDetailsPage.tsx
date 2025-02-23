import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { subscriptionApi } from "../../../api/subscriptionApi";
import { Subscription } from "../../../types";

export default function SubscriptionDetailsPage() {
  const { id } = useParams<{ id: string }>();
  const [subscription, setSubscription] = useState<Subscription | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function fetchData() {
      try {
        const response = await subscriptionApi.fetchById(id!);
        setSubscription(response.data);
      } catch (error) {
        console.error("Error fetching subscription", error);
      }
      setLoading(false);
    }
    fetchData();
  }, [id]);

  if (loading) return <p>Завантаження...</p>;

  return (
    <div className="max-w-md mx-auto p-6 bg-white rounded shadow">
      <h2 className="text-xl font-bold">Деталі підписки</h2>
      {subscription && (
        <>
          <p><strong>Email:</strong> {subscription.email}</p>
          <p><strong>План:</strong> {subscription.plan}</p>
          <p><strong>Дата початку:</strong> {subscription.startDate}</p>
          <p><strong>Дата закінчення:</strong> {subscription.endDate}</p>
        </>
      )}
    </div>
  );
}
