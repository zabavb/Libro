import { useDispatch } from "react-redux"
import { AppDispatch } from "../../state/redux"
import { useNavigate } from "react-router-dom"
import { useCallback, useEffect, useMemo, useState } from "react"
import { Order} from "../../types"
import OrderList from "../../components/order/admin/OrderList"

import { fetchOrdersService } from "../../services"
import { addNotification } from "../../state/redux/slices/notificationSlice"


const OrderListContainer = () => {
    const dispatch = useDispatch<AppDispatch>()
    const navigate = useNavigate()
    const [orders, setOrders] = useState<Order[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [searchTerm, setSearchTerm] = useState<string>('');
    const [pagination, setPagination] = useState({
        pageNumber: 1,
        pageSize: 10,
        totalCount: 0,
    })
    
    const paginationMemo = useMemo(() => ({...pagination}), [pagination]);

    const fetchOrderList = useCallback(async () => {
        setLoading(true);
        try{
            const response = await fetchOrdersService(
                paginationMemo.pageNumber,
                paginationMemo.pageSize,
                searchTerm,
            );

            if(response.error)
                dispatch(
                    addNotification({
                        message: response.error,
                        type: 'error',
                    }),
                );

            if (response && response.data) {
                const paginatedData = response.data;

                setOrders(paginatedData.items);
                setPagination({
                    pageNumber: paginatedData.pageNumber,
                    pageSize: paginatedData.pageSize,
                    totalCount: paginatedData.totalCount,
                });
            }else throw new Error('Invalid response structure');
        } catch (error) {
            dispatch(
                addNotification({
                    message: error instanceof Error ? error.message : String(error),
                    type: 'error',
                }),
            );
            setOrders([])
        }
        setLoading(false);
    }, [paginationMemo, searchTerm, dispatch])

    useEffect(() => {
        fetchOrderList();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    },[pagination.pageNumber, searchTerm])

    const handleNavigate = (path: string) => navigate(path);
    

    const handleSearchTermChange = (newSearchTerm: string) => {
        setSearchTerm(newSearchTerm);
        setPagination((prev) => ({ ...prev, pageNumber: 1}));
    };

    const handlePageChange = (pageNumber: number) => {
        setPagination((prev) => ({...prev, pageNumber}))
    }

    return (
        <OrderList
            orders={orders}
            loading={loading}
            pagination={pagination}
            onPageChange={handlePageChange}
            onNavigate={handleNavigate}
            onSearchTermChange={handleSearchTermChange}
            searchTerm={searchTerm}
        />
    )
}

export default OrderListContainer