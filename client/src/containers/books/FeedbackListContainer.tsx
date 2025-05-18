import { useEffect, useCallback, useState, useMemo } from "react"
import { useDispatch } from "react-redux"
import { AppDispatch } from "../../state/redux/index"
import { Feedback, FeedbackFilter, FeedbackSort } from "@/types"
import { addNotification } from "@/state/redux/slices/notificationSlice"
import { fetchFeedbacksService } from "@/services"
import FeedbackList from "@/components/book/admin/FeedbackList"

const FeedbackListContainer = () => {
	const dispatch = useDispatch<AppDispatch>()
    const [feedbacks, setFeedbacks] = useState<Feedback[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [searchTerm, setSearchTerm] = useState<string>('');
    const [sort, setSort] = useState<FeedbackSort>({});
    const [filters, setFilters] = useState<FeedbackFilter>({});
    const [pagination, setPagination] = useState({
        pageNumber: 1,
        pageSize: 10,
        totalCount: 0,
    })

    const paginationMemo = useMemo(() => ({...pagination}), [pagination]);

    const fetchFeedbacksList = useCallback(async () => {
        setLoading(true);
        try{
            const response = await fetchFeedbacksService(
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

                setFeedbacks(paginatedData.items);
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
            setFeedbacks([])
        }
        setLoading(false);
    }, [paginationMemo, searchTerm, sort, filters, dispatch])

    useEffect(() => {
        fetchFeedbacksList();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    },[pagination.pageNumber, searchTerm])

    const handlePageChange = (pageNumber: number) => {
        setPagination((prev) => ({...prev, pageNumber}))
    }

    const handleSearchTermChange = (newSearchTerm: string) => {
        setSearchTerm(newSearchTerm);
        setPagination((prev) => ({ ...prev, pageNumber: 1}));
    }

    const handleSortChange = (field: keyof FeedbackSort) => {
        setSort({ [field]: true});
        setPagination((prev) => ({ ...prev,pageNumber: 1}));
    }

		const handleFilterChange = (filters: FeedbackFilter) => {
			setFilters(filters);
			setPagination((prev) => ({ ...prev, pageNumber: 1 }));
		};
	

    return (
        <FeedbackList
            feedbacks={feedbacks}
            loading={loading}
            onFilterChange={handleFilterChange}
            filters={filters}
            onSortChange={handleSortChange}
            sort={sort}
            onSearchTermChange={handleSearchTermChange}
            searchTerm={searchTerm}
            pagination={pagination}
            onPageChange={handlePageChange}
        />
    )
}

export default FeedbackListContainer
