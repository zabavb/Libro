import { StrictMode } from "react"
import { createRoot } from "react-dom/client"
import { Provider } from "react-redux"
import store from "./state/redux/store"
import AppRoutes from "./routes.tsx"
// import { ErrorBoundary } from "./components/errorBoundary/ErrorBoundary.tsx"

createRoot(document.getElementById("root")!).render(
	<StrictMode>
		{/* <ErrorBoundary> */}
			<Provider store={store}>
				<AppRoutes />
			</Provider>
		{/* </ErrorBoundary> */}
	</StrictMode>
)
