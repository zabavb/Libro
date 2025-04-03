import { configureStore } from '@reduxjs/toolkit';
import bookReducer from './slices/bookSlice';
import notificationReducer from './slices/notificationSlice';
const store = configureStore({
  reducer: {
    books: bookReducer,
    notifications: notificationReducer,
  },
  devTools: process.env.NODE_ENV !== 'production',
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
export default store;
