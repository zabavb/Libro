import { Outlet } from "react-router-dom"

const AdminLayout: React.FC = () => {
	return (
		<div style={{ display: "flex", minHeight: "100vh" }}>
			{/* Sidebar */}
			<aside style={{ width: "250px", padding: "20px", background: "#f4f4f4", flexShrink: 0 }}>
				<nav>
					<ul style={{ listStyle: "none", padding: 0 }}>
						<li style={{ marginBottom: "10px" }}>
							<a
								href="/admin"
								style={{ textDecoration: "none", color: "#333" }}>
								Dashboard
							</a>
						</li>
						<li>
							<a
								href="/admin/users"
								style={{ textDecoration: "none", color: "#333" }}>
								Users
							</a>
						</li>
						<li>
							<a
								href="/admin/books"
								style={{ textDecoration: "none", color: "#333" }}>
								Books
							</a>
						</li>
					</ul>
				</nav>
			</aside>

			{/* Main Content Area */}
			<div style={{ display: "flex", flexDirection: "column", flex: 1 }}>
				{/* Header */}
				<header style={{ background: "#333", color: "#fff", padding: "15px", textAlign: "center" }}>
					<h1>Admin Panel</h1>
				</header>

				{/* Main Content */}
				<main style={{ flex: 1, padding: "20px", overflowY: "auto" }}>
					<Outlet />
				</main>
			</div>
		</div>
	)
}

export default AdminLayout
