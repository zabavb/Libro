import React from "react";
import { SubCategory, SubCategoryFilter, SubCategorySort } from "../../../types";
import '@/assets/styles/components/book/sub-category-list.css'
import {icons} from '@/lib/icons'
interface SubCategoryListProps {
    subCategories?: SubCategory[];
    loading: boolean;
    pagination: { pageNumber: number; pageSize: number; totalCount: number };
    onPageChange: (pageNumber: number) => void;
    onNavigate: (path: string) => void;
    onSortChange: (field: keyof SubCategorySort) => void;
    sort: SubCategorySort;
    onFilterChange: (filters: SubCategoryFilter) => void;
    filters: SubCategoryFilter;
    onSearchTermChange: (searchTerm: string) => void;
    searchTerm: string;
    onDelete: (e: React.MouseEvent,id:string) => void
}

const SubCategoryList: React.FC<SubCategoryListProps> = ({
    subCategories = [],
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
    onDelete,
}) => {
    if (loading) return <p>Loading...</p>
    return (
        <div className="mt-5">
            {subCategories.length > 0 ? (
                <div className="flex flex-col gap-2.5">
                    {subCategories.map((subCategory) => (
                        <div className="flex justify-between items-center">
                            <p className="font-semibold">{subCategory.name.toUpperCase()}</p>
                            <div className='flex gap-2'>
                                <button onClick={() => onNavigate(`/admin/bookRelated/subcategories/${subCategory.categoryId}`)} className='p-2.5 bg-[#FF642E] rounded-lg'><img src={icons.wPen} /></button>
                                <button onClick={(e) => onDelete(e,subCategory.categoryId)} className='p-2.5 border-[1px] border-[#FF642E] rounded-lg'><img src={icons.oTrash} /></button>
                            </div>
                        </div>
                    ))}
                </div>
            ) : (
                <p>No subcategories found.</p>
            )}

        </div>
    )


}

export default SubCategoryList;