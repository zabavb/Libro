import React, { useEffect } from "react"
import { useDispatch, useSelector } from "react-redux"
import { RootState } from "../../state/redux/store"
import { removeNotification } from "../../state/redux/slices/notificationSlice"
import Notification from "../../components/common/Notification"

const NotificationContainer: React.FC = () => {
	const dispatch = useDispatch()
	const notifications = useSelector((state: RootState) => state.notifications.notifications || [])

	useEffect(() => {
		notifications.forEach((notification) => {
			const timer = setTimeout(() => {
				dispatch(removeNotification(notification.id))
			}, 5000)

			return () => clearTimeout(timer)
		})
	}, [notifications, dispatch])

	const handleRemoveNotification = (id: string) => {
		dispatch(removeNotification(id))
	}

	return (
		<Notification
			notifications={notifications}
			onRemoveNotification={handleRemoveNotification}
		/>
	)
}

export default NotificationContainer
