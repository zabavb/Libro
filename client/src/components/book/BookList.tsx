import React from "react"
import { FixedSizeList as List } from "react-window"
import { Book } from "../../types/objects/Book"
import BookCardContainer from "../../containers/books/BookCardContainer"
import Pagination from "../common/Pagination"
import Loading from "../common/Loading"

interface BookListProps {
	books?: Book[]
	loading: boolean
	pagination: { pageNumber: number; pageSize: number; totalCount: number }
	onPageChange: (pageNumber: number) => void
	onNavigate: (path: string) => void
}

const BookList: React.FC<BookListProps> = ({
	books = [],
	loading,
	pagination,
	onPageChange,
	onNavigate,
}) => {
	const Card = ({ index, style }: { index: number; style: React.CSSProperties }) => (
		<div style={style}>
			<BookCardContainer book={books[index]} />
		</div>
	)
	return (
		<div>
			<p onClick={() => onNavigate("/admin")}>Back to Admin Dashboard</p>
			<h1>Book List</h1>

			{loading ? (
				<Loading />
			) : books.length > 0 ? (
				<List
					height={600}
					itemCount={books.length}
					itemSize={100}
					width={"100%"}>
					{Card}
				</List>
			) : (
				<p>No books found.</p>
			)}

			<hr />

			<Pagination
				pagination={pagination}
				onPageChange={onPageChange}
			/>
		</div>
	)
}

export default BookList
