import { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { fetchSubscriptions } from "../../../../state/redux/slices/subscriptionSlice.ts";
import { RootState, AppDispatch } from "../../../../state/redux/store";
import { useMemo } from "react";

const SubscriptionsListPage = () => {
  // Dispatching and accessing the Redux store
  const dispatch: AppDispatch = useDispatch();
  const { list, loading, error } = useSelector((state: RootState) => state.subscriptions);

  // Fetching subscriptions when the component mounts
  useEffect(() => {
    dispatch(fetchSubscriptions());
  }, [dispatch]);

  // Memoizing the sorted subscriptions list to avoid unnecessary re-sorting
  const sortedSubscriptions = useMemo(() => {
    return [...list].sort((a, b) => a.plan.localeCompare(b.plan)); // Sorting by subscription plan
  }, [list]);

  // Display loading message while fetching data
  if (loading) return <p>Loading...</p>;
  
  // Display error message if there's an error fetching the data
  if (error) return <p style={{ color: "red" }}>Error: {error}</p>;

  return (
    <div>
      <h1>Subscriptions List</h1>
      <ul>
        {/* Mapping through the sorted subscription list and displaying each one */}
        {sortedSubscriptions.map((sub) => (
          <li key={sub.id}>
            {sub.plan} — <strong>{sub.status}</strong> {/* Displaying the plan and status */}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default SubscriptionsListPage;
