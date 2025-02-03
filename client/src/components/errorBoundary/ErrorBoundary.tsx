/* import { Component, ErrorInfo, ReactNode } from "react"

interface ErrorBoundaryProps {
	children: ReactNode
	fallback?: ReactNode
}

interface ErrorBoundaryState {
	hasError: boolean
	error: Error | null
}

export class ErrorBoundary extends Component<ErrorBoundaryProps, ErrorBoundaryState> {
	constructor(props: ErrorBoundaryProps) {
		super(props)
		this.state = { hasError: false, error: null }
	}

	static getDerivedStateFromError(error: Error): ErrorBoundaryState {
		return { hasError: true, error }
	}

	componentDidCatch(error: Error, errorInfo: ErrorInfo): void {
		console.error("ErrorBoundary caught an error:", error, errorInfo)
	}

	render() {
		if (this.state.hasError) {
			return (
				this.props.fallback || (
					<div>
						<h2>Something went wrong.</h2>
						<p>We are working to fix it. Please try again later.</p>
					</div>
				)
			)
		}

		return this.props.children
	}
}
 */