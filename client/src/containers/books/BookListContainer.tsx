import { useEffect, useCallback } from "react"
import { useDispatch, useSelector } from "react-redux"
import { RootState, AppDispatch, fetchBooks } from "../../state/redux/index"
import { useNavigate } from "react-router-dom"
import BookList from "../../components/book/BookList"

const BookListContainer = () => {
	const dispatch = useDispatch<AppDispatch>()
	const {
		data: books,
		loading,
		pagination,
	} = useSelector((state: RootState) => state.books)
	const navigate = useNavigate()

	const fetchBookList = useCallback(() => {
		dispatch(
			fetchBooks({
				pageNumber: pagination.pageNumber,
				pageSize: pagination.pageSize,
			})
		)
	}, [dispatch, pagination.pageNumber, pagination.pageSize])
	useEffect(() => {
		fetchBookList();
	}, [fetchBookList]); 
	
	const handleNavigate = (path: string) => {
		navigate(path)
	}

	const handlePageChange = (pageNumber: number) => {
		dispatch(fetchBooks({ pageNumber, pageSize: pagination.pageSize }))
	}

	return (
		<BookList
			books={books}
			loading={loading}
			pagination={pagination}
			onPageChange={handlePageChange}
			onNavigate={handleNavigate}
		/>
	)
}

export default BookListContainer
