import { createSlice, PayloadAction } from "@reduxjs/toolkit"
import { v4 as uuidv4 } from "uuid"
import { Notification } from "../../../types"

interface NotificationState {
	notifications: Notification[]
}

const initialState: NotificationState = {
	notifications: [],
}

const notificationSlice = createSlice({
	name: "notifications",
	initialState,
	reducers: {
		addNotification: (state, action: PayloadAction<Omit<Notification, "id">>) => {
			const id = uuidv4()
			state.notifications.push({ ...action.payload, id })
		},
		removeNotification: (state, action: PayloadAction<string>) => {
			state.notifications = state.notifications.filter((n) => n.id !== action.payload)
		},
	},
})

export const { addNotification, removeNotification } = notificationSlice.actions
export default notificationSlice.reducer
