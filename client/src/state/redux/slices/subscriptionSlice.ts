import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import { subscriptionApi } from "../api/subscriptionApi";
import { Subscription } from "../types";

interface SubscriptionState {
  subscriptions: Subscription[];
  loading: boolean;
  error: string | null;
}

const initialState: SubscriptionState = {
  subscriptions: [],
  loading: false,
  error: null,
};

export const fetchSubscriptions = createAsyncThunk("subscriptions/fetch", async () => {
  const response = await subscriptionApi.fetchAll();
  return response.data.items;
});

const subscriptionSlice = createSlice({
  name: "subscriptions",
  initialState,
  reducers: {},
  extraReducers: (builder: any) => {
    builder
      .addCase(fetchSubscriptions.pending, (state: SubscriptionState) => {
        state.loading = true;
      })
      .addCase(fetchSubscriptions.fulfilled, (state: SubscriptionState, action: { payload: Subscription[] }) => {
        state.loading = false;
        state.subscriptions = action.payload;
      })
      .addCase(fetchSubscriptions.rejected, (state: SubscriptionState, action: { error: { message: string } }) => {
        state.loading = false;
        state.error = action.error.message || "Помилка";
      });
  },
});

export default subscriptionSlice.reducer;
