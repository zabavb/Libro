import React from "react";
import { Author, AuthorFilter, AuthorSort, User } from "../../../types";
import Pagination from "../../common/Pagination";
import Search from "../../common/Search";
import "@/assets/styles/base/table-styles.css"
import "@/assets/styles/components/list-styles.css"
import {icons} from "@/lib/icons"
import { getUserFromStorage } from "@/utils/storage";
import AuthorAdminCardContainer from "@/containers/books/AuthorAdminCardContainer";
interface AuthorListProps {
    authors?: Author[];
    loading: boolean;
    pagination: { pageNumber: number; pageSize: number; totalCount: number };
    onPageChange: (pageNumber: number) => void;
    onNavigate: (path: string) => void;
    onSortChange: (field: keyof AuthorSort) => void;
    sort: AuthorSort;
    onFilterChange: (filters: AuthorFilter) => void;
    filters: AuthorFilter;
    onSearchTermChange: (searchTerm: string) => void;
    searchTerm: string;
}

const AuthorList: React.FC<AuthorListProps> = ({
    authors = [],
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
                <button className="add-button" onClick={() => onNavigate("/admin/booksRelated/author/add")}>
                    <img src={icons.bPlus}/>
                    <p>
                        Add Author
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
                {authors.length > 0 ? (
                    <div className="flex flex-col w-full">
                        <div className="flex flex-row-reverse">
                            <p className="counter">
                                ({pagination.totalCount}) authors
                            </p>
                        </div>
                        <div className="table-wrapper">
                            <table>
                                <thead className="m-5">
                                    <tr>
                                        <th style={{ width: "5%" }} className='table-header'>Photo</th>
                                        <th style={{ width: "85%" }} className='table-header'>Full name</th>
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
                                            {authors.map((author) => (
                                                <AuthorAdminCardContainer author={author} key={author.authorId}/>
                                            ))}
                                        </tbody>
                                    )}
                            </table>
                        </div>
                    </div>
                ) : (
                    <p>No authors found.</p>
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

export default AuthorList;