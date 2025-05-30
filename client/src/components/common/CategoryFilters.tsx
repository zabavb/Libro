import React, { useCallback, useEffect, useMemo, useState } from "react";
import DropdownWrapper from "./DropdownWrapper";
import { Category } from "@/types";
import { useDispatch } from "react-redux";
import { AppDispatch } from "@/state/redux";
import { fetchCategoriesService } from "@/services";
import { addNotification } from "@/state/redux/slices/notificationSlice";
import { BookFilter } from "@/types/filters/BookFilter";
import SubCategoryFilters from "./SubcategoryFilters";

interface CategoryFiltersProps {
    onSelect: (field: keyof BookFilter, value: string) => void;
    filters: BookFilter;
}

const CategoryFilters: React.FC<CategoryFiltersProps> = ({ onSelect, filters }) => {
    const dispatch = useDispatch<AppDispatch>()
    const [categories, setCategories] = useState<Category[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [pagination, setPagination] = useState({
        pageNumber: 1,
        pageSize: 10,
        totalCount: 0,
    })


    const handleLoadMore = () => {
        if (pagination.pageSize < pagination.totalCount) {
            const newSize = pagination.pageSize + 10
            setPagination((prev) => ({ ...prev, pageSize: newSize }))
        }
    }

    const paginationMemo = useMemo(() => ({ ...pagination }), [pagination]);

    const fetchCategoriesList = useCallback(async () => {
        setLoading(true);
        try {
            const response = await fetchCategoriesService(
                paginationMemo.pageNumber,
                paginationMemo.pageSize,
            );

            if (response.error)
                dispatch(
                    addNotification({
                        message: response.error,
                        type: 'error',
                    }),
                );

            if (response && response.data) {
                const paginatedData = response.data;
                setCategories(paginatedData.items);
                setPagination({
                    pageNumber: paginatedData.pageNumber,
                    pageSize: paginatedData.pageSize,
                    totalCount: paginatedData.totalCount
                })
            } else throw new Error('invalid response structure');
        } catch (error) {
            dispatch(
                addNotification({
                    message: error instanceof Error ? error.message : String(error),
                    type: 'error'
                })
            )
            setCategories([])
        }
        setLoading(false);
    }, [paginationMemo, dispatch])

    useEffect(() => {
        fetchCategoriesList();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [pagination.pageNumber, pagination.pageSize])


    return (
        <div className="filter-container">
            <DropdownWrapper 
            triggerLabel="Category"
            triggerClassName={`transition-colors duration-100 hover:text-[#FF642E] ${filters.categoryId !== undefined && "text-[#FF642E]" }`}>
                <div className="flex flex-col gap-2 ">
                    {!loading || categories.length > 0 ?
                        categories.map((category) => (
                            <>
                                <button
                                    key={category.categoryId}
                                    className={`text-start transition-colors duration-100 hover:text-[#FF642E] ${filters.categoryId == category.categoryId && "text-[#FF642E]"}`}
                                    onClick={() => {
                                        onSelect("categoryId", category.categoryId)
                                    }}>
                                    {category.name}
                                </button>
                                {filters.categoryId == category.categoryId &&
                                    (<SubCategoryFilters
                                        onSelect={onSelect}
                                        filters={filters}
                                        categoryId={category.categoryId} />)
                                }
                            </>
                        ))
                        :
                        (<div>Loading</div>)
                    }
                    {pagination.totalCount > pagination.pageSize &&
                        <p onClick={handleLoadMore} aria-disabled={loading} className="cursor-pointer transition-colors duration-100 hover:text-[#FF642E]">
                            {loading ? "Loading..." : "Load more..."}
                        </p>
                    }
                </div>

            </DropdownWrapper>
        </div>
    )
}

export default CategoryFilters