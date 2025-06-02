import NewsletterAdvert from "@/components/common/NewsletterAdvert";
import OrderCardContainter from "../../../containers/order/OrderCardContainer";
import { Order, OrderFilter as OrderFilterType, OrderSort as OrderSortType } from "../../../types";
import Pagination from "../../common/Pagination";
import Search from "../../common/Search";
import UserOrdersListFilter from "./UserOrdersListFilter";
import UserOrdersListSort from "./UserOrdersListSort";
import '@/assets/styles/components/user/user-orders-list.css'
import linkIconUrl from '@/assets/link.svg'
interface OrderListProps {
    orders?: Order[]
    loading: boolean
    pagination: { pageNumber: number; pageSize: number; totalCount: number }
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
    return (
        <div className="user-orders-wrapper">
            <h1 className="text-white text-2xl font-semibold">Orders</h1>
            <div className="flex gap-5">
                <div className="user-orders-filter-panel">
                    <UserOrdersListFilter
                        filters={filters}
                        onFilterChange={onFilterChange}
                    />
                </div>
                <div className="user-orders-main-panel">
                    {loading ? (<>Loading...</>) : orders.length > 0 ? (
                        orders.map((order) => (
                            <OrderCardContainter
                                key={order.id}
                                order={order}
                            />
                        ))
                    ) : (
                        <div className="flex flex-col gap-3.5">
                            <p className="text-[#9C9C97] font-bold text-lg">No orders yet</p>
                            <div className="flex gap-2.5">
                                <a className="tosite-btn" href="/">Go to the website</a>
                                <a href="/"><img src={linkIconUrl} /></a>
                            </div>
                        </div>
                    )}
                    <Pagination
                        pagination={pagination}
                        onPageChange={onPageChange}
                    />
                </div>
            </div>
            <NewsletterAdvert />
        </div>
    )
}

export default UserOrdersList