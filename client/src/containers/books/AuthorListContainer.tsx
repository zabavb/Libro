import { useEffect, useCallback, useState, useMemo } from "react"
import { useDispatch } from "react-redux"
import { AppDispatch } from "../../state/redux/index"
import { useNavigate } from "react-router-dom"
import { Author, AuthorFilter, AuthorSort } from "@/types"
import { fetchAuthorsService } from "@/services/authorService"
import { addNotification } from "@/state/redux/slices/notificationSlice"
import AuthorList from "@/components/book/admin/AuthorList"

const AuthorListContainer = () => {
	const dispatch = useDispatch<AppDispatch>()
    const navigate = useNavigate()
    const [authors, setDeliveryTypes] = useState<Author[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [searchTerm, setSearchTerm] = useState<string>('');
    const [sort, setSort] = useState<AuthorSort>({});
    const [filters, setFilters] = useState<AuthorFilter>({});
    const [pagination, setPagination] = useState({
        pageNumber: 1,
        pageSize: 10,
        totalCount: 0,
    })

    const paginationMemo = useMemo(() => ({...pagination}), [pagination]);

    const fetchDeliveryTypeList = useCallback(async () => {
        setLoading(true);
        try{
            const response = await fetchAuthorsService(
                paginationMemo.pageNumber,
                paginationMemo.pageSize,
                searchTerm,
				filters,
                sort
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

                setDeliveryTypes(paginatedData.items);
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
            setDeliveryTypes([])
        }
        setLoading(false);
    }, [paginationMemo, searchTerm, sort, filters, dispatch])

    useEffect(() => {
        fetchDeliveryTypeList();
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

    const handleSortChange = (field: keyof AuthorSort) => {
        setSort({ [field]: true});
        setPagination((prev) => ({ ...prev,pageNumber: 1}));
    }

		const handleFilterChange = (filters: AuthorFilter) => {
			setFilters(filters);
			setPagination((prev) => ({ ...prev, pageNumber: 1 }));
		};
	

    return (
		<AuthorList
			authors={authors}
			loading={loading}
			onPageChange={handlePageChange}
			pagination={pagination}
			onSortChange={handleSortChange}
			sort={sort}
			onFilterChange={handleFilterChange}
			filters={filters}
			onNavigate={handleNavigate}
			onSearchTermChange={handleSearchTermChange}
			searchTerm={searchTerm}
		/>
    )
}

export default AuthorListContainer
