import React, { useState } from "react";
import { Category, User } from "../../../types";
import Pagination from "../../common/Pagination";
import Search from "../../common/Search";
import { icons } from "@/lib/icons"
import { getUserFromStorage } from "@/utils/storage";
import SubCategoryDropdownWrapper from "@/components/common/SubCategoryDropdownWrapper";
import SubCategoryListContainer from "@/containers/books/SubCategoryListContainer";
interface CategoryListProps {
    categories?: Category[];
    loading: boolean;
    pagination: { pageNumber: number; pageSize: number; totalCount: number };
    onPageChange: (pageNumber: number) => void;
    onNavigate: (path: string) => void;
    onDelete: (e: React.MouseEvent, id: string) => void
    onSearchTermChange: (searchTerm: string) => void;
    searchTerm: string;
}

const CategoryList: React.FC<CategoryListProps> = ({
    categories = [],
    loading,
    pagination,
    onPageChange,
    searchTerm,
    onSearchTermChange,
    onDelete,
    onNavigate,
}) => {
    const user: User | null = getUserFromStorage();
    const [openCategory, setOpenCategory] = useState<string>();

    const handleOpenCategory = (id: string) => {
        if (openCategory === id)
            setOpenCategory(undefined);
        else
            setOpenCategory(id);
    }

    if (loading) return <p>Loading...</p>
    return (
        <div>
            <header className="header-container">
                <Search
                    searchTerm={searchTerm}
                    onSearchTermChange={onSearchTermChange} />

                <div className="profile-icon">
                    <img src={user?.imageUrl ? user.imageUrl : icons.bUser} className={`w-[43px] ${user?.imageUrl ? "bg-transparent" : "bg-[#FF642E]"} rounded-full`} />

                    <p className="profile-name">{user?.firstName ?? "Unknown User"} {user?.lastName}</p>
                </div>

            </header>
            <main className="main-container">
                {categories.length > 0 ? (
                    <div className="flex flex-col w-full gap-2.5">

                        <button className="add-button"
                            onClick={() => onNavigate(openCategory ? `/admin/booksRelated/subcategory/add/${openCategory}` : "/admin/booksRelated/category/add")}>
                            <img src={icons.bPlus} />
                            <p>
                                {openCategory ? "Add Subcategory" : "Add Category"}
                            </p>
                        </button>
                        <div className="flex flex-col gap-2.5">
                            {categories.map((category) => (
                                <div className="flex flex-col">
                                    <SubCategoryDropdownWrapper
                                        id={category.categoryId}
                                        isOpen={openCategory === category.categoryId}
                                        onStateChange={handleOpenCategory}
                                        onDelete={onDelete}
                                        triggerLabel={category.name.toUpperCase()}
                                    >
                                        <SubCategoryListContainer categoryId={category.categoryId} />
                                    </SubCategoryDropdownWrapper>
                                </div>
                            ))}
                        </div>
                    </div>
                ) : (
                    <p>No categories found.</p>
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

export default CategoryList;