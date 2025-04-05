import { StrictMode } from "react"
import { createRoot } from "react-dom/client"
import { Provider } from "react-redux"
import store from "./state/redux/store"
import AppRoutes from "./routes.tsx"
import NotificationContainer from "./containers/common/NotificationContainer.tsx"
import './index.css'
createRoot(document.getElementById("root")!).render(
	<StrictMode>
		<Provider store={store}>
			<NotificationContainer />
			<AppRoutes />
		</Provider>
	</StrictMode>
)
