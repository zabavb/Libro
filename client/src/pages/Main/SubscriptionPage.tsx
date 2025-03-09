import { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { fetchSubscriptions } from "../../state/redux/slices/subscriptionSlice";
import { RootState, AppDispatch } from "../../state/redux/store"; 

const SubscriptionPage = () => {
  const dispatch = useDispatch<AppDispatch>(); 
  // Accessing subscriptions state from the Redux store
  const { list, loading, error } = useSelector((state: RootState) => state.subscriptions);

  // Fetching subscriptions data when the component mounts
  useEffect(() => {
    dispatch(fetchSubscriptions());
  }, [dispatch]);

  // Display loading message while fetching data
  if (loading) return <p>Loading...</p>;

  // Display error message if there's an error fetching the data
  if (error) return <p style={{ color: "red" }}>Error: {error}</p>;

  return (
    <div>
      <h1>My Subscriptions</h1>
      <ul>
        {/* Mapping through the list of subscriptions and displaying each one */}
        {list.map((sub) => (
          <li key={sub.id}>
            {sub.plan} - <strong>{sub.status}</strong> {/* Displaying the plan and its status */}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default SubscriptionPage;
