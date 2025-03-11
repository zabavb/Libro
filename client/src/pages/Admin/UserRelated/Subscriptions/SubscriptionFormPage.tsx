import { useState } from "react";
import { useDispatch } from "react-redux";
import { createSubscription } from "../../../../state/redux/slices/subscriptionSlice";
import { validateSubscription } from "../../../../utils/subscriptionValidationSchema";
import { Subscription } from "../../../../types/types/order/Subscription"; 

// Valid values for the plan
const validPlans = ["basic", "premium", "pro"] as const;
// Valid values for the status
const validStatuses = ["active", "inactive", "cancelled"] as const;

const SubscriptionFormPage = () => {
  const dispatch = useDispatch<any>();

  // Initializing form state with required fields
  const [form, setForm] = useState({
    userId: "",
    plan: "", // This field will hold a string that corresponds to one of the valid plan values
    id: "", // Can be generated or left empty
    status: "active", // Default status
    autoRenewal: true, // Auto-renewal is enabled by default
  });

  const [error, setError] = useState<string | null>(null);

  // Handling form input changes
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  // Handling form submission
  const handleSubmit = () => {
    // Checking if the plan is one of the valid values
    if (!validPlans.includes(form.plan as "basic" | "premium" | "pro")) {
      setError("Invalid plan!");
      return;
    }

    // Checking if the status is one of the valid values
    if (!validStatuses.includes(form.status as "active" | "inactive" | "cancelled")) {
      setError("Invalid status!");
      return;
    }

    // Additional form validation (if necessary)
    if (!validateSubscription(form)) {
      setError("Invalid data!");
      return;
    }

    setError(null);

    // Creating the subscription object with necessary fields
    const subscriptionData: Subscription = {
      ...form,
      id: form.id || "generated-id", // Can generate an ID if needed
      status: form.status as "active" | "inactive" | "cancelled", // Casting to the correct type
      autoRenewal: form.autoRenewal !== undefined ? form.autoRenewal : true, // Default value
      plan: form.plan as "basic" | "premium" | "pro", // Casting to the correct type
    };

    // Dispatching the action to create a subscription
    dispatch(createSubscription(subscriptionData)).catch(console.error);
  };

  return (
    <div>
      <h1>Create Subscription</h1>
      {error && <p style={{ color: "red" }}>{error}</p>}
      <input name="userId" value={form.userId} onChange={handleChange} placeholder="User ID" />
      
      {/* Using validation to ensure the plan is valid */}
      <input name="plan" value={form.plan} onChange={handleChange} placeholder="Plan" />
      
      {/* You can add a field for entering the status if needed */}
      <input name="status" value={form.status} onChange={handleChange} placeholder="Status" />
      
      <button onClick={handleSubmit}>Create</button>
    </div>
  );
};

export default SubscriptionFormPage;
