import OrderCardContainter from "../../../containers/order/OrderCardContainer";
import { Order, OrderFilter as OrderFilterType, OrderSort as OrderSortType } from "../../../types";
import Pagination from "../../common/Pagination";
import Search from "../../common/Search";
import UserOrdersListFilter from "./UserOrdersListFilter";
import UserOrdersListSort from "./UserOrdersListSort";


interface OrderListProps {
    orders?: Order[]
    loading: boolean
    error: string | null | undefined
    pagination: {pageNumber: number; pageSize: number; totalCount: number}
    onPageChange: (pageNumber: number) => void
    onNavigate: (path: string) => void
    onSearchTermChange: (searchTerm: string) => void
    searchTerm: string
    onFilterChange: (filters: OrderFilterType) => void
    filters: OrderFilterType
    onSortChange: (field: keyof OrderSortType) => void
    sort: OrderSortType
}

const UserOrdersList: React.FC<OrderListProps> = ({
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
            <p onClick={() => onNavigate("/")}>Go Back</p>
            <h1>Order List</h1>

            <Search 
                searchTerm={searchTerm}
                onSearchTermChange={onSearchTermChange}/>

            <UserOrdersListFilter
                filters={filters}
                onFilterChange={onFilterChange}
            />

            <UserOrdersListSort
                sort={sort}
                onSortChange={onSortChange}/> 
            
            {loading ? (<>tmp</>) : orders.length > 0 ? (
                orders.map((order) => (
                    <OrderCardContainter
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

export default UserOrdersList