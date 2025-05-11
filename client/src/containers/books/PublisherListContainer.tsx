import { useEffect, useCallback, useState, useMemo } from "react"
import { useDispatch } from "react-redux"
import { AppDispatch } from "../../state/redux/index"
import { useNavigate } from "react-router-dom"
import { addNotification } from "@/state/redux/slices/notificationSlice"
import { Publisher } from "@/types/types/book/Publisher"
import { fetchPublishersService } from "@/services"
import PublisherList from "@/components/book/admin/PublisherList"

const PublisherListContainer = () => {
	const dispatch = useDispatch<AppDispatch>()
    const navigate = useNavigate()
    const [publishers, setPublishers] = useState<Publisher[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [searchTerm, setSearchTerm] = useState<string>('');
    const [pagination, setPagination] = useState({
        pageNumber: 1,
        pageSize: 10,
        totalCount: 0,
    })

    const paginationMemo = useMemo(() => ({...pagination}), [pagination]);

    const fetchPublishersList = useCallback(async () => {
        setLoading(true);
        try{
            const response = await fetchPublishersService(
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

                setPublishers(paginatedData.items);
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
            setPublishers([])
        }
        setLoading(false);
    }, [paginationMemo, searchTerm, dispatch])

    useEffect(() => {
        fetchPublishersList();
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
	

    return (
        <PublisherList
            publishers={publishers}
            pagination={pagination}
            loading={loading}
            onNavigate={handleNavigate}
            onPageChange={handlePageChange}
            onSearchTermChange={handleSearchTermChange}
            searchTerm={searchTerm}
        />
    )
}

export default PublisherListContainer
