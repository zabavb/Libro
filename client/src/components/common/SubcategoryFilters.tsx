import React, { useCallback, useEffect, useMemo, useState } from "react";
import { SubCategory } from "@/types";
import { useDispatch } from "react-redux";
import { AppDispatch } from "@/state/redux";
import { fetchSubCategoriesService } from "@/services";
import { addNotification } from "@/state/redux/slices/notificationSlice";
import { BookFilter } from "@/types/filters/BookFilter";

interface SubCategoryFiltersProps {
    onSelect: (option: keyof BookFilter, value: string) => void;
    filters: BookFilter;
    categoryId: string;
}

const SubCategoryFilters: React.FC<SubCategoryFiltersProps> = ({ onSelect, filters, categoryId }) => {
    const dispatch = useDispatch<AppDispatch>()
    const [subcategories, setSubcategories] = useState<SubCategory[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [pagination, setPagination] = useState({
        pageNumber: 1,
        pageSize: 10,
        totalCount: 0,
    })


    const handleLoadMore = () => {
        if(pagination.pageSize < pagination.totalCount){
            const newSize = pagination.pageSize + 10
            setPagination((prev) => ({...prev, pageSize:newSize}))
        }
    }

    const paginationMemo = useMemo(() => ({ ...pagination }), [pagination]);

    const fetchSubcategoriesList = useCallback(async () => {
        setLoading(true);
        try {
            const response = await fetchSubCategoriesService(
                paginationMemo.pageNumber,
                paginationMemo.pageSize,
                undefined,
                {categoryId}
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
                setSubcategories(paginatedData.items);
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
            setSubcategories([])
        }
        setLoading(false);
    }, [paginationMemo, dispatch, categoryId])

    useEffect(() => {
        fetchSubcategoriesList();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    },[pagination.pageNumber, pagination.pageSize])


    return (
        <div className="filter-container ml-4">
                <div className="flex flex-col ">
                {!loading || subcategories.length > 0 ? 
                    subcategories.map((subcategory) => (
                        <button 
                        key={subcategory.subCategoryId}
                        className={`text-start transition-colors duration-100 hover:text-[#FF642E] ${filters.subcategoryId == subcategory.subCategoryId && "text-[#FF642E]"}`}
                        onClick={() => {
                            onSelect("subcategoryId",subcategory.subCategoryId)
                            }}>
                            {subcategory.name}
                        </button>
                    ))
                    :
                    (<div>Loading</div>)
                }
                </div>
                {pagination.totalCount > pagination.pageSize &&
                <p onClick={handleLoadMore} aria-disabled={loading} className="cursor-pointer transition-colors duration-100 hover:text-[#FF642E]">
                    {loading ? "Loading..." : "Load more..." }
                </p>
                }
        </div>
    )
}

export default SubCategoryFilters