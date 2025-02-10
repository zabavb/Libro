import { configureStore } from "@reduxjs/toolkit"
import userReducer from "./slices/userSlice"
import notificationReducer from "./slices/notificationSlice"
import orderReducer from "./slices/orderSlice"
const store = configureStore({
	reducer: {
		users: userReducer,
		orders: orderReducer,
		notifications: notificationReducer,
	},
	devTools: process.env.NODE_ENV !== "production",
})

export type RootState = ReturnType<typeof store.getState>
export type AppDispatch = typeof store.dispatch
export default store
