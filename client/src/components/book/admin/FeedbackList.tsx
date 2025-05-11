import React from "react";
import { Feedback, FeedbackFilter, FeedbackSort, User } from "../../../types";
import Pagination from "../../common/Pagination";
import Search from "../../common/Search";
import "@/assets/styles/base/table-styles.css"
import "@/assets/styles/components/list-styles.css"
import { icons } from "@/lib/icons"
import { getUserFromStorage } from "@/utils/storage";
import FeedbackAdminCard from "./FeedbackAdminCard";
interface FeedbackListProps {
    feedbacks?: Feedback[];
    loading: boolean;
    pagination: { pageNumber: number; pageSize: number; totalCount: number };
    onPageChange: (pageNumber: number) => void;
    onSortChange: (field: keyof FeedbackSort) => void;
    sort: FeedbackSort;
    onFilterChange: (filters: FeedbackFilter) => void;
    filters: FeedbackFilter;
    onSearchTermChange: (searchTerm: string) => void;
    searchTerm: string;
}

const FeedbackList: React.FC<FeedbackListProps> = ({
    feedbacks = [],
    loading,
    pagination,
    onPageChange,
    searchTerm,
    onSearchTermChange,
}) => {
    const user: User | null = getUserFromStorage();
    if (loading) return <p>Loading...</p>
    return (
        <div>
            <header className="header-container">
                <Search
                    searchTerm={searchTerm}
                    onSearchTermChange={onSearchTermChange} />
                <div className="profile-icon">
                    <div className="icon-container-pfp">
                        <img src={user?.imageUrl ? user.imageUrl : icons.bUser} className="panel-icon" />
                    </div>
                    <p className="profile-name">{user?.firstName ?? "Unknown User"} {user?.lastName}</p>
                </div>

            </header>
            <main className="main-container">
                {feedbacks.length > 0 ? (
                    <div className="flex flex-col w-full gap-6">
                        <div className="flex flex-col gap-2.5">
                            {loading ? (
                                <div style={{ textAlign: "center", height: `${pagination.pageSize * 65}px` }}>
                                    Loading...
                                </div>
                            )
                                : (
                                    <>
                                        {feedbacks.map((feedback) => (
                                            <FeedbackAdminCard feedback={feedback}/>
                                        ))}
                                    </>
                                )}
                        </div>
                    </div>
                ) : (
                    <>
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

export default FeedbackList;