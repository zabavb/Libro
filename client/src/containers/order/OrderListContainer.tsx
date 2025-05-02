import { useNavigate } from "react-router-dom"
import { useDispatch } from "react-redux"
import { AppDispatch } from "../../state/redux"
import { useCallback, useEffect, useMemo, useState } from "react"
import { Order, OrderFilter, OrderSort} from "../../types"
import OrderList from "../../components/order/admin/OrderList"
import { fetchOrdersService } from "../../services"
import { addNotification } from "../../state/redux/slices/notificationSlice"


const OrderListContainer = () => {
    const dispatch = useDispatch<AppDispatch>()
    const navigate = useNavigate()
    const [orders, setOrders] = useState<Order[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [searchTerm, setSearchTerm] = useState<string>('');
    const [filters, setFilters] = useState<OrderFilter>({});
    const [sort, setSort] = useState<OrderSort>({});
    const [pagination, setPagination] = useState({
        pageNumber: 1,
        pageSize: 10,
        totalCount: 0,
    })
    
    const paginationMemo = useMemo(() => ({...pagination}), [pagination]);

    const fetchOrderList = useCallback(async () => {
        (async () => {
            setLoading(true);
            try {
                const response = await fetchOrdersService(
                    paginationMemo.pageNumber,
                    paginationMemo.pageSize,
                    searchTerm,
                    filters,
                    sort,
                );

                if (response.error)
                    dispatch(
                        addNotification({
                            message: response.error,
                            type: 'error',
                        }),
                    );

                if (response.data) {
                    const paginatedData = response.data;

                    setOrders((prev) =>
                        JSON.stringify(prev) === JSON.stringify(paginatedData.items) ? prev : paginatedData.items
                    );
                    setPagination((prev) => {
                        if (
                            prev.pageNumber === paginatedData.pageNumber &&
                            prev.pageSize === paginatedData.pageSize &&
                            prev.totalCount === paginatedData.totalCount
                        ) {
                            return prev;
                        }
                        return {
                            pageNumber: paginatedData.pageNumber,
                            pageSize: paginatedData.pageSize,
                            totalCount: paginatedData.totalCount,
                        };
                    });
                } else if (response.error) throw Error(response.error);
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
        })();
    }, [paginationMemo, searchTerm, filters, sort, dispatch])

    useEffect(() => {
        fetchOrderList();
    }, [fetchOrderList]);

    const handleNavigate = (path: string) => navigate(path);

    const handleSearchTermChange = (newSearchTerm: string) => {
        setSearchTerm(newSearchTerm);
        setPagination((prev) => ({ ...prev, pageNumber: 1}));
    };

    const handleFilterChange = (filters: OrderFilter) => {
        setFilters(filters);
        setPagination((prev) => ({ ...prev, pageNumber: 1 }));
    };

    const handleSortChange = (sort: OrderSort) => {
        setSort(sort);
        setPagination((prev) => ({ ...prev, pageNumber: 1 }));
    }

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
            onFilterChange={handleFilterChange}
            filters={filters}
            onSortChange={handleSortChange}
            sort={sort}
        />
    )
}

export default OrderListContainer