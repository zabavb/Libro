import { Book, BookSort } from "@/types";
import { BookFilter } from "@/types/filters/BookFilter";
import Pagination from "../common/Pagination";
import BookCardContainer from "@/containers/books/BookCardContainer";

interface BookCatalogProps {
    books?: Book[]
    loading: boolean
    pagination: {pageNumber: number; pageSize: number; totalCount:number}
    onPageChange: (pageNumber: number) => void
    onNavigate: (path: string) => void
    onSearchTermChange: (searchTerm: string) => void
    searchTerm: string
    onFilterChange: (filters: BookFilter) => void
    filters: BookFilter
    onSortChange: (field: keyof BookSort) => void
    sort: BookSort
}

const BookCatalog: React.FC<BookCatalogProps> = ({
    books = [],
    loading,
    pagination,
    onPageChange,
    //searchTerm,
    //onSearchTermChange,
    //filters,
    //onFilterChange,
    //sort,
    //onSortChange,
}) => {
    if (loading) return <p>Loading...</p>
    return(
        <div>
            {/* Search panel */}

            {/* Filter panel */}

            {/* Sort panel */}
            <div style={{display:'flex'}}>
            {loading ? (<>tmp</>) : books.length > 0 ? (
                books.map((book) => (
                    <BookCardContainer
                        key={book.bookId}
                        book={book}
                        />
                ))
            ) : (
                <p>No books found.</p>
            )}
            </div>

            <Pagination
                pagination={pagination}
                onPageChange={onPageChange}
            />
            
        </div>
    )
}

export default BookCatalog