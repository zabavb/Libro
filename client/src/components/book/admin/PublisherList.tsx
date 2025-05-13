import React from "react";
import { User } from "../../../types";
import Pagination from "../../common/Pagination";
import Search from "../../common/Search";
import "@/assets/styles/base/table-styles.css"
import "@/assets/styles/components/list-styles.css"
import { icons } from "@/lib/icons"
import { getUserFromStorage } from "@/utils/storage";
import { Publisher } from "@/types/types/book/Publisher";
import PublisherAdminCardContainer from "@/containers/books/PublisherAdminCardContainer";
interface PublisherListProps {
    publishers?: Publisher[];
    loading: boolean;
    pagination: { pageNumber: number; pageSize: number; totalCount: number };
    onPageChange: (pageNumber: number) => void;
    onNavigate: (path: string) => void;
    onSearchTermChange: (searchTerm: string) => void;
    searchTerm: string;
}

const PublisherList: React.FC<PublisherListProps> = ({
    publishers = [],
    loading,
    pagination,
    onPageChange,
    searchTerm,
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
                <button className="add-button" onClick={() => onNavigate("/admin/booksRelated/publisher/add")}>
                    <img src={icons.bPlus} />
                    <p>
                        Add Publisher
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
                {publishers.length > 0 ? (
                    <div className="flex flex-col w-full gap-6">
                        <p className="w-full text-[#FF642E] text-2xl font-semibold">
                            All publishers
                        </p>
                        <div className="flex flex-col gap-2.5">
                        {loading ? (
                                <div style={{ textAlign: "center", height: `${pagination.pageSize * 65}px` }}>
                                    Loading...
                                </div>
                        )
                            : (
                                <>
                                    {publishers.map((publisher) => (
                                        <PublisherAdminCardContainer publisher={publisher} key={publisher.publisherId} />
                                    ))}
                                </>
                            )}
                        </div>
                    </div>
                ) : (
                    <>
                        <p className="w-full text-[#FF642E] text-2xl font-semibold">
                            All publishers
                        </p>
                        <p>No publishers found.</p>
                    </>
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

export default PublisherList;