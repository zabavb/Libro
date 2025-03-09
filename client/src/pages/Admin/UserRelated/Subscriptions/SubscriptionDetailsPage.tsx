import { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { fetchSubscriptionById } from "../../../../state/redux/slices/subscriptionSlice";
import { RootState, AppDispatch } from "../../../../state/redux/store";
import { useParams } from "react-router-dom";

const SubscriptionDetailsPage = () => {
  // Extracting the subscription ID from the URL params.
  const { id } = useParams<{ id: string }>();

  // Dispatch and selector hooks from Redux to manage state.
  const dispatch: AppDispatch = useDispatch();
  // Accessing the current state of subscriptions, including loading status and errors.
  const { current, loading, error } = useSelector((state: RootState) => state.subscriptions);

  // Using useEffect to fetch subscription details when the component mounts
  // or when the `id` from the URL changes.
  useEffect(() => {
    if (id) {
      // Dispatching the action to fetch subscription by ID.
      dispatch(fetchSubscriptionById(id));
    }
  }, [id, dispatch]);

  // Rendering loading state while the subscription data is being fetched.
  if (loading) return <p>Loading...</p>;

  // Displaying an error message if there is an error fetching the subscription data.
  if (error) return <p style={{ color: "red" }}>Error: {error}</p>;

  // Displaying a message if no subscription data is found for the given ID.
  if (!current) return <p>Subscription not found</p>;

  // Rendering the details of the subscription once the data is available.
  return (
    <div>
      <h1>Subscription Details</h1>
      <p>ID: {current.id}</p>
      <p>User ID: {current.userId}</p>
      <p>Plan: {current.plan}</p>
      <p>Status: {current.status}</p>
    </div>
  );
};

export default SubscriptionDetailsPage;
