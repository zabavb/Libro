import React from "react";
import "@/assets/styles/base/table-styles.css"
import "@/assets/styles/components/list-styles.css"
import {icons} from "@/lib/icons"
import { getUserFromStorage } from "@/utils/storage";
import { Book, BookSort, User } from "@/types";
import { BookFilter } from "@/types/filters/BookFilter";
import Search from "../common/Search";
import BookAdminCardContainer from "@/containers/books/BookAdminCardContainer";
import Pagination from "../common/Pagination";
interface BookListProps {
    books?: Book[];
    loading: boolean;
    pagination: { pageNumber: number; pageSize: number; totalCount: number };
    onPageChange: (pageNumber: number) => void;
    onNavigate: (path: string) => void;
    onSortChange: (field: keyof BookSort) => void;
    sort: BookSort;
    onFilterChange: (filters: BookFilter) => void;
    filters: BookFilter;
    onSearchTermChange: (searchTerm: string) => void;
    searchTerm: string;
}

const BookList: React.FC<BookListProps> = ({
    books = [],
    loading,
    pagination,
    onPageChange,
    searchTerm,
    onSortChange,
    sort,
    onFilterChange,
    filters,
    onSearchTermChange,
    onNavigate,
}) => {
    const user: User | null = getUserFromStorage();
    if (loading) return <p>Loading...</p>
    return (
        <div>
            <header className="header-container">
                <Search
                    searchTerm={searchTerm}
                    onSearchTermChange={onSearchTermChange} />
                <button className="add-button" onClick={() => onNavigate("/admin/booksRelated/book/add")}>
                    <img src={icons.bPlus}/>
                    <p>
                        Add Book
                    </p>
                </button>
        <div className="profile-icon">
          <div className="icon-container-pfp">
            <img src={user?.imageUrl ? user.imageUrl : icons.bUser} className="panel-icon" />
          </div>
          <p className="profile-name">{user?.firstName ?? "Unknown User"} {user?.lastName}</p>
        </div>

            </header>
            <main className="main-container">
                {books.length > 0 ? (
                    <div className="flex flex-col w-full">
                        <div className="flex flex-row-reverse">
                            <p className="counter">
                                ({pagination.totalCount}) books
                            </p>
                        </div>
                        <div className="table-wrapper">
                            <table>
                                <thead className="m-5">
                                    <tr>
                                        <th style={{ width: "5%" }} className='table-header'>Photo</th>
                                        <th style={{ width: "35%" }} className='table-header'>Title</th>
                                        <th style={{ width: "20%" }} className='table-header'>Author</th>
                                        <th style={{ width: "5%" }} className='table-header'>Code</th>
                                        <th style={{ width: "15%" }} className='table-header'>Genre</th>
                                        <th style={{ width: "10%" }} className='table-header'>Price</th>
                                        <th style={{ width: "10%" }} className='table-header'>Actions</th>
                                    </tr>
                                </thead>
                                {loading ? (
                                    <tr>
                                        <td colSpan={5} style={{ textAlign: "center", height: `${pagination.pageSize * 65}px` }}>
                                            Loading...
                                        </td>
                                    </tr>
                                )
                                    : (
                                        <tbody>
                                            {books.map((book) => (
                                                <BookAdminCardContainer book={book} key={book.bookId}/>
                                            ))}
                                        </tbody>
                                    )}
                            </table>
                        </div>
                    </div>
                ) : (
                    <p>No books found.</p>
                )}
            </main>
            <footer>
                <div className="pagination-container">
                    <Pagination
                        pagination={pagination}
                        onPageChange={onPageChange}
                    />
                </div>
            </footer>
        </div>
    )


}

export default BookList;
