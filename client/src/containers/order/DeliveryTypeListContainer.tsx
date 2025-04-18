import { useDispatch } from "react-redux"
import { AppDispatch } from "../../state/redux"
import { useNavigate } from "react-router-dom"
import { useCallback, useEffect, useMemo, useState } from "react"
import DeliveryTypeList from "../../components/order/admin/DeliveryList"
import { DeliverySort, DeliveryType } from "../../types"
import { fetchDeliveryTypesService } from "../../services"
import { addNotification } from "../../state/redux/slices/notificationSlice"

const DeliveryTypeListContainer = () => {
    const dispatch = useDispatch<AppDispatch>()
    const navigate = useNavigate()
    const [deliveryTypes, setDeliveryTypes] = useState<DeliveryType[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [searchTerm, setSearchTerm] = useState<string>('');
    const [sort, setSort] = useState<DeliverySort>({});
    const [pagination, setPagination] = useState({
        pageNumber: 1,
        pageSize: 10,
        totalCount: 0,
    })

    const paginationMemo = useMemo(() => ({...pagination}), [pagination]);

    const fetchDeliveryTypeList = useCallback(async () => {
        setLoading(true);
        try{
            const response = await fetchDeliveryTypesService(
                paginationMemo.pageNumber,
                paginationMemo.pageSize,
                searchTerm,
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
    }, [paginationMemo, searchTerm, sort,dispatch])

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

    const handleSortChange = (field: keyof DeliverySort) => {
        setSort({ [field]: true});
        setPagination((prev) => ({ ...prev,pageNumber: 1}));
    }

    return (
        <DeliveryTypeList
        deliveryTypes={deliveryTypes}
        loading={loading}
        pagination={pagination}
        onPageChange={handlePageChange}
        onNavigate={handleNavigate}
        onSearchTermChange={handleSearchTermChange}
        searchTerm={searchTerm}
        onSortChange={handleSortChange}
        sort={sort}
        handleNavigate={handleNavigate}
        />
    )
}

export default DeliveryTypeListContainer