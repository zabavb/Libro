import type { Notification } from "../../types"

interface NotificationProps {
	notifications?: Notification[]
	onRemoveNotification: (id: string) => void
}

const Notification: React.FC<NotificationProps> = ({ notifications = [], onRemoveNotification }) => {
	return (
		<div>
			{notifications
				.filter((notification) => Boolean(notification.message?.trim()))
				.map((notification) => (
					<div key={notification.id}>
						{notification.message}
						<button onClick={() => onRemoveNotification(notification.id)}>X</button>
					</div>
				))}
		</div>
	)
}

export default Notification
