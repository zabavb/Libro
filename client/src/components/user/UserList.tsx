import { useEffect } from "react"
import { useDispatch, useSelector } from "react-redux"
import { RootState, AppDispatch, fetchUsers } from "../../state/redux/index"
import UserCardContainer from "../../containers/user/UserCardContainer"

const UserList = () => {
	const dispatch = useDispatch<AppDispatch>()
	const { data: users, loading, error } = useSelector((state: RootState) => state.users)

	useEffect(() => {
		dispatch(fetchUsers())
	}, [dispatch])

	if (loading) return <p>Loading...</p>
	if (error) return <p>Error: {error}</p>

	return (
		<div>
			<h1>User List</h1>
			<div>
				{users.map((user) => (
					<UserCardContainer
						key={user.id}
						user={user}
					/>
				))}
				<hr />
			</div>
		</div>
	)
}

export default UserList
