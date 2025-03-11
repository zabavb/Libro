import * as Yup from "yup";

// Validation schema for subscription
export const subscriptionValidationSchema = Yup.object().shape({
  userId: Yup.string().required("User ID is required"), // Ensure user ID is provided
  plan: Yup.string().oneOf(["basic", "premium", "pro"], "Invalid subscription plan"), // Ensure the plan is one of the allowed options
  autoRenewal: Yup.boolean(), // Boolean value for auto-renewal
});

// Function to validate the subscription data
export const validateSubscription = (data: any) => {
  try {
    // Validate data against the schema, and return true if successful
    subscriptionValidationSchema.validateSync(data, { abortEarly: false });
    return true;
  } catch (error) {
    console.error("Validation errors:", error);
    return false;
  }
};
