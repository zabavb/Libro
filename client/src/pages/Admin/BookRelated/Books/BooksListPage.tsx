import { useNavigate } from "react-router-dom"
import BookListContainer from "../../../../containers/books/BookListContainer"

const BookListPage = () => {
	const navigate = useNavigate()

	return (
		<div>
			<header>
				<h1>Book Management</h1>
				<button onClick={() => navigate("/admin/books")}>Back to Admin Dashboard</button>
			</header>
			<main>
				<div>
					<button onClick={() => navigate("/admin/books/add")}>Add Book</button>
				</div>
				<BookListContainer />
			</main>
		</div>
	)
}

export default BookListPage
