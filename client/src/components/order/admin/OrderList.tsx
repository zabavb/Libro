import { OrderCard } from "@/types/types/order/OrderCard";
import OrderAdminCardContainter from "../../../containers/order/OrderAdminCardContainer";
import Pagination from "../../common/Pagination";
import Search from "../../common/Search";
import OrderFilter from "../OrderFilter";
import OrderSort from "../OrderSort";


interface OrderListProps {
    orders?: OrderCard[]
    loading: boolean
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
	pagination,
	onPageChange,
	searchTerm,
	onSearchTermChange,
	filters,
	onFilterChange,
	sort,
	onSortChange,
}) => {
    if (loading) return <p>Loading...</p>
    return(
        <div>

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
                        key={order.orderId}
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