import CategoryList from "@/components/book/admin/CategoryList";
import { fetchCategoriesService, removeCategoryService } from "@/services";
import { AppDispatch } from "@/state/redux";
import { addNotification } from "@/state/redux/slices/notificationSlice";
import { Category } from "@/types";
import { useCallback, useEffect, useMemo, useState } from "react";
import { useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";

const CategoryListContainer = () => {
	const dispatch = useDispatch<AppDispatch>()
    const navigate = useNavigate()
    const [category, setCategory] = useState<Category[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [searchTerm, setSearchTerm] = useState<string>('');
    const [pagination, setPagination] = useState({
        pageNumber: 1,
        pageSize: 10,
        totalCount: 0,
    })

    const paginationMemo = useMemo(() => ({...pagination}), [pagination]);

    const fetchCategoriesList = useCallback(async () => {
        setLoading(true);
        try{
            const response = await fetchCategoriesService(
                paginationMemo.pageNumber,
                paginationMemo.pageSize,
                searchTerm,
            );

            if(response.error)
                dispatch(
                    addNotification({
                        message: response.error,
                        type:'error',
                    }),
                );

            if(response && response.data) {
                const paginatedData = response.data;
                console.log(paginatedData)
                setCategory(paginatedData.items);
                setPagination({
                    pageNumber: paginatedData.pageNumber,
                    pageSize: paginatedData.pageSize,
                    totalCount: paginatedData.totalCount
                })
            }else throw new Error('invalid response structure');
        }catch(error){
            dispatch(
                addNotification({
                    message: error instanceof Error ? error.message : String(error),
                    type: 'error'
                })
            )
            setCategory([])
        }
        setLoading(false);
    }, [paginationMemo, searchTerm, dispatch])

    useEffect(() => {
        fetchCategoriesList();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    },[pagination.pageNumber, searchTerm])

    const handleNavigate = (path: string) => navigate(path)
    
    const handlePageChange = (pageNumber: number) => {
        setPagination((prev) => ({...prev, pageNumber}))
    }

    const handleSearchTermChange = (newSearchTerm: string) => {
        setSearchTerm(newSearchTerm);
        setPagination((prev) => ({ ...prev, pageNumber: 1}));
    }

     const handleDelete = async (e: React.MouseEvent, id: string) => {
         e.stopPropagation()
         const response = await removeCategoryService(id)
         dispatch(
             response.error
                 ? addNotification({
                     message: response.error,
                     type: 'error',
                 })
                 : addNotification({
                     message: 'Category successfully deleted.',
                     type: 'success',
                 }),
         );
     }
	

    return (
		<CategoryList
			categories={category}
			loading={loading}
			onPageChange={handlePageChange}
			pagination={pagination}
			onNavigate={handleNavigate}
			onSearchTermChange={handleSearchTermChange}
			searchTerm={searchTerm}
            onDelete={handleDelete}
		/>
    )
}

export default CategoryListContainer
