import { createSlice, createAsyncThunk, PayloadAction } from "@reduxjs/toolkit";
import { Subscription } from "../../../types/types/order/Subscription";
import subscriptionService from "../../../services/subscriptionService";

interface SubscriptionState {
  list: Subscription[];      // List of all subscriptions
  current: Subscription | null; // Currently selected subscription
  loading: boolean;           // Loading state for async actions
  error: string | null;       // Error message, if any
}

const initialState: SubscriptionState = {
  list: [],
  current: null,
  loading: false,
  error: null,
};

// Async thunk for creating a subscription
export const createSubscription = createAsyncThunk<Subscription, Subscription>(
  "subscriptions/create",
  async (newSubscription) => {
    return await subscriptionService.create(newSubscription); 
  }
);

// Async thunk for fetching all subscriptions
export const fetchSubscriptions = createAsyncThunk<Subscription[]>(
  "subscriptions/fetchAll",
  async () => {
    return await subscriptionService.getAll();
  }
);

// Async thunk for fetching a single subscription by its ID
export const fetchSubscriptionById = createAsyncThunk<Subscription, string>(
  "subscriptions/fetchById",
  async (id) => {
    return await subscriptionService.getById(id);
  }
);

const subscriptionSlice = createSlice({
  name: "subscriptions",
  initialState,
  reducers: {
    // Clear the current subscription
    clearCurrentSubscription(state) {
      state.current = null;
    },
  },
  extraReducers: (builder) => {
    builder
      // Fetch all subscriptions - loading state
      .addCase(fetchSubscriptions.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      // Fetch all subscriptions - success
      .addCase(fetchSubscriptions.fulfilled, (state, action: PayloadAction<Subscription[]>) => {
        state.loading = false;
        state.list = action.payload;
      })
      // Fetch all subscriptions - error
      .addCase(fetchSubscriptions.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message || "Error loading subscriptions";
      })
      // Fetch subscription by ID - success
      .addCase(fetchSubscriptionById.fulfilled, (state, action: PayloadAction<Subscription>) => {
        state.current = action.payload;
      })
      // Handle the create subscription case - loading state
      .addCase(createSubscription.pending, (state) => {
        state.loading = true;
      })
      // Handle the create subscription case - success
      .addCase(createSubscription.fulfilled, (state, action: PayloadAction<Subscription>) => {
        state.loading = false;
        state.list.push(action.payload); // Add the newly created subscription to the list
      })
      // Handle the create subscription case - error
      .addCase(createSubscription.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message || "Error creating subscription";
      });
  },
});

export const { clearCurrentSubscription } = subscriptionSlice.actions;
export default subscriptionSlice.reducer;
