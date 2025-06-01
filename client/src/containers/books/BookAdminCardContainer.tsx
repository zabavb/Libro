import React from "react"
import { useNavigate } from "react-router-dom"
// import { BookToBookView } from "../../api/adapters/bookAdapter"
import BookAdminCard from "@/components/book/admin/BookAdminCard"
import { deleteBookService } from "@/services/bookService"
import { useDispatch } from "react-redux"
import { AppDispatch } from "@/state/redux"
import { addNotification } from "@/state/redux/slices/notificationSlice"
import { BookCard } from "@/types/types/book/BookDetails"

interface BookCardContainerProps {
  book: BookCard
}

const BookAdminCardContainer: React.FC<BookCardContainerProps> = ({ book }) => {
    const dispatch = useDispatch<AppDispatch>()
  // const bookView = BookToBookView(book) // TEMPORARY REMOVED
  const navigate = useNavigate()
  const handleNavigate = () => {
    navigate(`/admin/booksRelated/book/${book.bookId}`)
  }

  const handleDelete = async (e: React.MouseEvent) => {
    e.stopPropagation()
    const response = await deleteBookService(book.bookId)
    dispatch(
        response.error
        ? addNotification({
          message: response.error,
          type: 'error',
          })
        : addNotification({
          message: 'Book successfully deleted.',
          type: 'success',
          }),
      );
  }


  return (
    <BookAdminCard
      book={book}
      onNavigate={handleNavigate}
      onDelete={handleDelete}
    />
  )
}

export default BookAdminCardContainer
