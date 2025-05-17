import React, { useEffect, useCallback, useState, useMemo } from "react"
import { useDispatch } from "react-redux"
import { AppDispatch } from "../../state/redux/index"
import { SubCategory, SubCategoryFilter, SubCategorySort } from "@/types"
import { addNotification } from "@/state/redux/slices/notificationSlice"
import { useNavigate } from "react-router-dom"
import { fetchSubCategoriesService, removeSubCategoryService } from "@/services"
import SubCategoryList from "@/components/book/admin/SubCategoryList"

interface SubCategoryListContainerProps {
    categoryId: string
}

const SubCategoryListContainer: React.FC<SubCategoryListContainerProps> = ({ categoryId }) => {
    const dispatch = useDispatch<AppDispatch>()
    const navigate = useNavigate()
    const [subCategories, setSubCategories] = useState<SubCategory[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [searchTerm, setSearchTerm] = useState<string>('');
    const [sort, setSort] = useState<SubCategorySort>({});
    const [filters, setFilters] = useState<SubCategoryFilter>({ categoryId:categoryId });
    const [pagination, setPagination] = useState({
        pageNumber: 1,
        pageSize: 10,
        totalCount: 0,
    })

    const paginationMemo = useMemo(() => ({ ...pagination }), [pagination]);

    const fetchSubCategoriesList = useCallback(async () => {
        setLoading(true);
        try {
            const response = await fetchSubCategoriesService(
                paginationMemo.pageNumber,
                paginationMemo.pageSize,
                searchTerm,
                sort,
                filters
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

                setSubCategories(paginatedData.items);
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
            setSubCategories([])
        }
        setLoading(false);
    }, [paginationMemo, searchTerm, filters, sort, dispatch])

    useEffect(() => {
        fetchSubCategoriesList();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [pagination.pageNumber, searchTerm])

    const handlePageChange = (pageNumber: number) => {
        setPagination((prev) => ({ ...prev, pageNumber }))
    }

    const handleSearchTermChange = (newSearchTerm: string) => {
        setSearchTerm(newSearchTerm);
        setPagination((prev) => ({ ...prev, pageNumber: 1 }));
    }

    const handleSortChange = (field: keyof SubCategorySort) => {
        setSort({ [field]: true });
        setPagination((prev) => ({ ...prev, pageNumber: 1 }));
    }

    const handleFilterChange = (filters: SubCategoryFilter) => {
        setFilters({ ...filters, categoryId });
        setPagination((prev) => ({ ...prev, pageNumber: 1 }));
    };

    const handleDelete = async (e: React.MouseEvent, id: string) => {
        e.stopPropagation()
        const response = await removeSubCategoryService(id)
        dispatch(
            response.error
                ? addNotification({
                    message: response.error,
                    type: 'error',
                })
                : addNotification({
                    message: 'Subcategory successfully deleted.',
                    type: 'success',
                }),
        );
    }

    const handleNavigate = (path: string) => navigate(path)

    return (
        <SubCategoryList
            subCategories={subCategories}
            loading={loading}
            onSearchTermChange={handleSearchTermChange}
            searchTerm={searchTerm}
            onSortChange={handleSortChange}
            sort={sort}
            onFilterChange={handleFilterChange}
            filters={filters}
            onPageChange={handlePageChange}
            pagination={pagination}
            onNavigate={handleNavigate}
            onDelete={handleDelete}
        />
    )
}

export default SubCategoryListContainer
