import OrderAdminCardContainer from "../../../containers/order/OrderAdminCardContainer";
import { Order } from "../../../types";
import Pagination from "../../common/Pagination";
import Search from "../../common/Search";
import "@/assets/styles/base/table-styles.css"
import "@/assets/styles/components/order-list.css"
interface OrderListProps {
    orders?: Order[]
    loading: boolean
    pagination: { pageNumber: number; pageSize: number; totalCount: number }
    onPageChange: (pageNumber: number) => void
    onNavigate: (path: string) => void
    onSearchTermChange: (searchTerm: string) => void
    searchTerm: string
}

const OrderList: React.FC<OrderListProps> = ({
    orders = [],
    loading,
    pagination,
    onPageChange,
    searchTerm,
    onSearchTermChange,
}) => {
    return (
        <div>
            <header className="header-container">
                <Search
                    searchTerm={searchTerm}
                    onSearchTermChange={onSearchTermChange} />
            </header>
            <main className="main-container">
                {orders.length > 0 ? (
                    <div style={{ display: "flex", flexDirection: "column" }}>
                        <div style={{ display: "flex", flexDirection: "row-reverse" }}>
                            <p className="counter">
                                ({pagination.totalCount}) orders
                            </p>
                        </div>
                        <div className="table-wrapper">
                            <table>
                                <thead style={{ margin: "20px" }}>
                                    <tr>
                                        <th style={{ width: "30%" }}>Name and Surname</th>
                                        <th style={{ width: "25%" }}>Order</th>
                                        <th style={{ width: "10%" }}>Price</th>
                                        <th style={{ width: "15%" }}>Status</th>
                                        <th style={{ width: "10%" }}></th>
                                    </tr>
                                </thead>
                                {loading ? (
                                    <tr>
                                        <td colSpan={5} style={{ textAlign: "center", height: `${orders.length * 65}px` }}>
                                            Loading...
                                        </td>
                                    </tr>
                                )
                                    : (
                                        <tbody>
                                            {orders.map((order) => (
                                                <OrderAdminCardContainer
                                                    key={order.id}
                                                    order={order}
                                                />
                                            ))}
                                        </tbody>
                                    )}
                            </table>
                        </div>
                    </div>
                ) : (
                    loading ? (<p>Loading...</p>) : (<p>No orders found.</p>)
                )}
            </main>
            <footer>
                <div style={{ float: "right", padding: "0 5%" }}>
                    <Pagination
                        pagination={pagination}
                        onPageChange={onPageChange}
                    />
                </div>
            </footer>
        </div>
    )
}

export default OrderList