import { Book, BookSort } from "@/types";
import { BookFilter } from "@/types/filters/BookFilter";
import Pagination from "../common/Pagination";
import BookCardContainer from "@/containers/books/BookCardContainer";
import "@/assets/styles/components/book/catalog.css"
import CatalogSort from "./CatalogSort";
import CatalogFilter from "./CatalogFilter";

interface BookCatalogProps {
    books?: Book[]
    loading: boolean
    pagination: { pageNumber: number; pageSize: number; totalCount: number }
    onPageChange: (pageNumber: number) => void
    onNavigate: (path: string) => void
    onSearchTermChange: (searchTerm: string) => void
    searchTerm: string
    onFilterChange: (filters: BookFilter) => void
    filters: BookFilter
    onSortChange: (field: keyof BookSort) => void
    sort: BookSort
    isAudioOnly?: boolean
}

const BookCatalog: React.FC<BookCatalogProps> = ({
    books = [],
    loading,
    pagination,
    onPageChange,
    filters,
    onFilterChange,
    sort,
    onSortChange,
    isAudioOnly = false
}) => {
    return (
        <div className="catalog-container">
            <aside className={`options-panel overflow-scroll ${isAudioOnly ? "audio-only" : ""}`}>
                <CatalogFilter filters={filters} onFilterChange={onFilterChange} isAudioOnly={isAudioOnly} />
            </aside>
            <main className={`books-panel ${isAudioOnly ? "audio-only" : ""}`}>
                <div className={`books-panel-header ${isAudioOnly ? "audio-only" : ""}`}>
                    <div className={`books-counter ${isAudioOnly ? "audio-only" : ""}`}>
                        {pagination.totalCount} books
                    </div>
                    {!isAudioOnly && (
                        <div className="opacity-50 font-semibold text-base">
                            <CatalogSort sort={sort} onSortChange={onSortChange} />
                        </div>
                    )}
                </div>

                <div className="books-panel-main">
                    {loading ? (
                        <>Loading...</>
                    ) : books.length > 0 ? (
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
                    <button className={`load-btn ${isAudioOnly ? "audio-only" : ""}`}>
                        Load more books
                    </button>
                    <Pagination pagination={pagination} onPageChange={onPageChange} />
                </div>
            </main>
        </div>
    );
}

export default BookCatalog;
