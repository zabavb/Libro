import { configureStore } from "@reduxjs/toolkit"
import userReducer from "./slices/userSlice"
import notificationReducer from "./slices/notificationSlice"

const store = configureStore({
	reducer: {
		users: userReducer,
		notifications: notificationReducer,
	},
	devTools: process.env.NODE_ENV !== "production",
})

export type RootState = ReturnType<typeof store.getState>
export type AppDispatch = typeof store.dispatch
export default store
