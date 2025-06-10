import React, { useEffect, useRef } from "react"
import { useDispatch, useSelector } from "react-redux"
import { RootState } from "../../state/redux/store"
import { removeNotification } from "../../state/redux/slices/notificationSlice"
import { toast, ToastContainer, ToastOptions } from "react-toastify"
import 'react-toastify/dist/ReactToastify.css'

const toastOptions: ToastOptions = {
  position: "top-right",
  autoClose: 4000,
  hideProgressBar: false,
  closeOnClick: true,
  pauseOnHover: true,
  draggable: true,
  progress: undefined,
}

const NotificationContainer: React.FC = () => {
	const dispatch = useDispatch()
	const notifications = useSelector((state: RootState) => state.notifications.notifications || [])
	const shownNotificationsRef = useRef<Set<string>>(new Set())

	useEffect(() => {
		notifications.forEach((notification) => {
			if (!shownNotificationsRef.current.has(notification.id)) {
				shownNotificationsRef.current.add(notification.id)

				switch (notification.type) {
					case "success":
						toast.success(notification.message, toastOptions)
						break
					case "error":
						toast.error(notification.message, toastOptions)
						break
					case "info":
						toast.info(notification.message, toastOptions)
						break
					default:
						toast(notification.message, toastOptions)
				}

				setTimeout(() => {
					dispatch(removeNotification(notification.id))
					shownNotificationsRef.current.delete(notification.id)
				}, 4000)
			}
		})
	}, [notifications, dispatch])

	return <ToastContainer />
}

export default NotificationContainer
