import React, { useCallback, useEffect, useMemo, useState } from "react";
import DropdownWrapper from "./DropdownWrapper";
import { Category } from "@/types";
import { useDispatch } from "react-redux";
import { AppDispatch } from "@/state/redux";
import { fetchCategoriesService } from "@/services";
import { addNotification } from "@/state/redux/slices/notificationSlice";
import { BookFilter } from "@/types/filters/BookFilter";

interface CategoryFiltersProps {
    onSelect: (option: keyof BookFilter, value: string) => void;
}

const CategoryFilters: React.FC<CategoryFiltersProps> = ({ onSelect }) => {
    const dispatch = useDispatch<AppDispatch>()
    const [selectedCategory, setSelectedCategory] = useState<string>();
    const [categories, setCategories] = useState<Category[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [pagination, setPagination] = useState({
        pageNumber: 1,
        pageSize: 1000,
        totalCount: 0,
    })


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
                console.log(paginatedData)
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
    },[pagination.pageNumber])


    return (
        <div className="filter-container">
            <DropdownWrapper triggerLabel="Category">
                <div className="flex flex-col ">
                {!loading ? 
                    categories.map((category) => (
                        <button 
                        key={category.categoryId}
                        className={`text-start ${selectedCategory == category.categoryId && "text-[#FF642E]"}`}
                        onClick={() => {
                            onSelect("categoryId",category.categoryId)
                            setSelectedCategory(category.categoryId)
                            }}>
                            {category.name}
                        </button>
                    ))
                    :
                    (<div>Loading</div>)
                }
                </div>
            </DropdownWrapper>
        </div>
    )
}

export default CategoryFilters