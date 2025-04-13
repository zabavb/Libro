import { Outlet, useLocation } from "react-router-dom"
import "@/assets/styles/layout/admin-layout.css"
import logoUrl from "@/assets/logoLibro.svg"
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome'
import {faCreditCard, faFile, faUser} from '@fortawesome/free-regular-svg-icons'
import {faArrowRightFromBracket, faBook, faChartSimple} from "@fortawesome/free-solid-svg-icons"
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
							<FontAwesomeIcon icon={faChartSimple}/>	
							<a
								href="/admin"
								>
								Home
							</a>
						</li>
						<li className={isActive("/admin/books") ? "active-link link" : "link"}>
							<FontAwesomeIcon icon={faBook}/>
							<a
								href="/admin/books"
								>
								Books
							</a>
						</li>
						<li className={isActive("/admin/users") ? "active-link link" : "link"}>
							<FontAwesomeIcon icon={faUser}/>
							<a
								href="/admin/users"
								>
								Users
							</a>
						</li>
						<li className={isActive("/admin/orders") ? "active-link link" : "link"}>
							<FontAwesomeIcon icon={faFile}/>
							<a
								href="/admin/orders"
								>
								Orders
							</a>
						</li>
						<li className={isActive("/admin/subscriptions") ? "active-link link" : "link"}>
							<FontAwesomeIcon icon={faCreditCard}/>
							<a>
								Subscriptions
							</a>
						</li>
					</ul>
					<div className="link logout-item">
					<FontAwesomeIcon 
					style={{ transform: 'scaleX(-1)' }}
					icon={faArrowRightFromBracket} />	
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
