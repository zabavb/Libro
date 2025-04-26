import { Outlet, useLocation } from "react-router-dom"
import "@/assets/styles/layout/admin-layout.css"
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome'
import {faTruck} from "@fortawesome/free-solid-svg-icons"

import logoUrl from "@/assets/logoLibro.svg"
import homeIcon from "@/assets/icons/adminMain.svg"
import bookIcon from "@/assets/icons/adminBook.svg"
import userIcon from "@/assets/icons/adminUser.svg"
import orderIcon from "@/assets/icons/adminOrder.svg"
import subscriptionIcon from "@/assets/icons/adminSubscription.svg"
import logoutIcon from "@/assets/icons/logoutIcon.svg"
const AdminLayout: React.FC = () => {
	const location = useLocation();
	const currentPath = location.pathname;

	const isActive = (path: string) => currentPath === path;


	return (
		<div className="admin-container">
			{/* Sidebar */}
			<aside className="admin-sidebar">
				<nav>
					<ul className="admin-nav-list">
						<li className="logo-container">
						<img src={logoUrl}/>
						</li>
						<li className={isActive("/admin") ? "active-link link" : "link"}>
							<img src={homeIcon} className={isActive("/admin") ? "invert" : ""}/>
							<a
								href="/admin"
								>
								Home
							</a>
						</li>
						<li className={isActive("/admin/books") ? "active-link link" : "link"}>
							<img src={bookIcon} className={isActive("/admin/books") ? "invert" : ""}/>
							<a
								href="/admin/books"
								>
								Books
							</a>
						</li>
						<li className={isActive("/admin/users") ? "active-link link" : "link"}>
							<img src={userIcon} className={isActive("/admin/users") ? "invert" : ""}/>
							<a
								href="/admin/users"
								>
								Users
							</a>
						</li>
						<li className={isActive("/admin/orders") ? "active-link link" : "link"}>
							<img src={orderIcon} className={isActive("/admin/orders") ? "invert" : ""}/>
							<a
								href="/admin/orders"
								>
								Orders
							</a>
						</li>
						<li className={isActive("/admin/delivery") ? "active-link link" : "link"}>
							<FontAwesomeIcon icon={faTruck}/>
							<a
								href="/admin/delivery"
								>
								Deliveries
							</a>
						</li>
						<li className={isActive("/admin/subscriptions") ? "active-link link" : "link"}>
							<img src={subscriptionIcon} className={isActive("/admin/subscriptions") ? "invert" : ""}/>
							<a>
								Subscriptions
							</a>
						</li>
					</ul>
					<div className="link logout-item">
					<img src={logoutIcon}/>

					<a>
					Logout
					</a>
					</div>

				</nav>
			</aside>

			{/* Main Content Area */}
			<div style={{ display: "flex", flexDirection: "column", flex: 1 }}>
				{/* Header */}
				{/* <header style={{ background: "#333", color: "#fff", padding: "15px", textAlign: "center" }}>
					<h1>Admin Panel</h1>
				</header> */}
				<header></header>
				{/* Main Content */}
				<main style={{ flex: 1, overflowY: "auto" }}>
					<Outlet />
				</main>
			</div>
		</div>
	)
}

export default AdminLayout
