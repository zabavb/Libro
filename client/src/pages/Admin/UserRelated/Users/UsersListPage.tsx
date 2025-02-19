import { useNavigate } from "react-router-dom"
import UserListContainer from "../../../../containers/user/UserListContainer"

const UserListPage = () => {
	const navigate = useNavigate()

	return (
		<div>
			<header>
				<h1>User Management</h1>
				<button onClick={() => navigate("/admin")}>Back to Admin Dashboard</button>
			</header>
			<main>
				<div>
					<button onClick={() => navigate("/admin/users/add")}>Add User</button>
				</div>
				<UserListContainer />
			</main>
		</div>
	)
}

export default UserListPage
