import { Book, BookSort } from "@/types";
import { BookFilter } from "@/types/filters/BookFilter";
import Pagination from "../common/Pagination";
import BookCardContainer from "@/containers/books/BookCardContainer";
import "@/assets/styles/components/book/catalog.css"
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
        <div className="catalog-container">
            <aside className="options-panel">
                {/* Temporary text, to be replaced 
                with actual components in the future */}
                <div className="options-container">
                    <div>Filters</div>
                    <div>Categories</div>
                    <div>Types</div>
                    <div>Availability</div>
                    <div>Language</div>
                    <div>Author</div>
                </div>
                <div>
                    <div>Price</div>
                </div>
            </aside>
            <main className="books-panel">
                <div className="books-panel-header">
                    <div className="books-counter">
                        {pagination.totalCount} books
                    </div>
                    <div className="opacity-50 font-semibold text-base">
                        SORT_COMPONENT_TMP
                    </div>
                </div>
                <div className="books-panel-main">
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
                <div className="books-panel-footer">
                    {/* Replace with custom pagination */}
                    <button className="load-btn">Load more books</button>
                    <Pagination
                        pagination={pagination}
                        onPageChange={onPageChange}
                    />
                </div>
            </main>


            
        </div>
    )
}

export default BookCatalog