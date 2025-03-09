import { configureStore } from "@reduxjs/toolkit";
import userReducer from "./slices/userSlice";
import bookReducer from "./slices/bookSlice";
import notificationReducer from "./slices/notificationSlice";
import orderReducer from "./slices/orderSlice";
import deliveryTypeSlice from "./slices/deliveryTypeSlice";
import subscriptionsReducer from "./slices/subscriptionSlice";

const rootReducer = {
  users: userReducer,
  orders: orderReducer,
  deliveryTypes: deliveryTypeSlice,
  books: bookReducer,
  notifications: notificationReducer,
  subscriptions: subscriptionsReducer,
};

const store = configureStore({
  reducer: rootReducer,
  devTools: process.env.NODE_ENV !== "production",
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export default store;
