import { useEffect, useState } from "react";
import { fetchSubscriptionsService, removeSubscriptionService } from "../../../services/subscriptionService";
import { Subscription } from "../../../types";
import { useNavigate } from "react-router-dom";

export default function SubscriptionsListPage() {
  const [subscriptions, setSubscriptions] = useState<Subscription[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const navigate = useNavigate();

  useEffect(() => {
    loadSubscriptions();
  }, []);

  const loadSubscriptions = async () => {
    setLoading(true);
    try {
      const data = await fetchSubscriptionsService();
      setSubscriptions(data.items);
    } catch (err) {
      setError("Помилка завантаження підписок");
    }
    setLoading(false);
  };

  const handleDelete = async (id: string) => {
    if (!confirm("Ви впевнені, що хочете видалити підписку?")) return;
    try {
      await removeSubscriptionService(id);
      setSubscriptions(subscriptions.filter((s) => s.id !== id));
    } catch (err) {
      alert("Помилка при видаленні підписки");
    }
  };

  return (
    <div className="max-w-4xl mx-auto p-6 bg-white rounded shadow">
      <h2 className="text-xl font-bold mb-4">Список підписок</h2>

      {loading && <p>Завантаження...</p>}
      {error && <p className="text-red-500">{error}</p>}

      <button onClick={() => navigate("/admin/subscriptions/new")} className="bg-green-500 text-white p-2 rounded mb-4">
        + Додати підписку
      </button>

      <table className="w-full border-collapse border">
        <thead>
          <tr className="bg-gray-200">
            <th className="border p-2">ID</th>
            <th className="border p-2">Email</th>
            <th className="border p-2">План</th>
            <th className="border p-2">Дії</th>
          </tr>
        </thead>
        <tbody>
          {subscriptions.map((sub) => (
            <tr key={sub.id} className="border">
              <td className="border p-2">{sub.id}</td>
              <td className="border p-2">{sub.email}</td>
              <td className="border p-2">{sub.plan}</td>
              <td className="border p-2">
                <button onClick={() => navigate(`/admin/subscriptions/edit/${sub.id}`)} className="text-blue-500 mr-2">
                  Редагувати
                </button>
                <button onClick={() => handleDelete(sub.id)} className="text-red-500">
                  Видалити
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
