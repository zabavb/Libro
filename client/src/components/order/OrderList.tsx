import OrderAdminCardContainter from "../../containers/order/OrderAdminCardContainer";
import { Order } from "../../types";
import Pagination from "../common/Pagination";
import Search from "../common/Search";
import OrderFilter from "./OrderFilter";
import OrderSort from "./OrderSort";


interface OrderListProps {
    orders?: Order[]
    loading: boolean
    error: string | null | undefined
    pagination: {pageNumber: number; pageSize: number; totalCount: number}
    onPageChange: (pageNumber: number) => void
    onNavigate: (path: string) => void
    onSearchTermChange: (searchTerm: string) => void
    searchTerm: string
    onFilterChange: (filters: OrderFilter) => void
    filters: OrderFilter
    onSortChange: (field: keyof OrderSort) => void
    sort: OrderSort
}

const OrderList: React.FC<OrderListProps> = ({
    orders = [],
	loading,
	error,
	pagination,
	onPageChange,
	searchTerm,
	onSearchTermChange,
	filters,
	onFilterChange,
	sort,
	onSortChange,
	onNavigate,
}) => {
    if (loading) return <p>Loading...</p>
    if (error) return <p>Error: {error}</p>
    return(
        <div>
            <p onClick={() => onNavigate("/admin")}>Back to Admin Dashboard</p>
            <p onClick={() => onNavigate("/admin/orders/add")}>Add Order</p>
            <h1>Order List</h1>

            <Search 
                searchTerm={searchTerm}
                onSearchTermChange={onSearchTermChange}/>

            <OrderFilter
                filters={filters}
                onFilterChange={onFilterChange}
            />

            <OrderSort
                sort={sort}
                onSortChange={onSortChange}/>
            
            {loading ? (<>tmp</>) : orders.length > 0 ? (
                orders.map((order) => (
                    <OrderAdminCardContainter
                        key={order.id}
                        order={order}
                    />
                ))
            ) : (
                <p>No orders found.</p>
            )}

            <hr/>

            <Pagination
                pagination={pagination}
                onPageChange={onPageChange}
            />
        </div>
    )
}

export default OrderList