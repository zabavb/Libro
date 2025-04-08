import OrderAdminCardContainer from "../../../containers/order/OrderAdminCardContainer";
import { Order } from "../../../types";
import Pagination from "../../common/Pagination";
import Search from "../../common/Search";

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
            <style>
                {`
                    .table-wrapper{
                        border-radius: 30px;
                        border: 1px solid black;
                        padding: 20px;
                        margin: 1% 5%;
                    }
                    table{
                        width: 100%;

                        border-collapse: separate;
                        border-spacing: 0;

                        overflow: hidden;
                    }

                    th, td{
                        padding: 15px;
                        border-bottom: 1px solid black;
                    }

                `}
            </style>
            <Search
                searchTerm={searchTerm}
                onSearchTermChange={onSearchTermChange} />
            {orders.length > 0 ? (
                <div style={{ display: "flex", flexDirection: "column" }}>
                    <div style={{ display: "flex", flexDirection: "row-reverse" }}>
                        <p style={{ margin: "0 5%" }}>
                            <strong style={{ color: "#ff642e" }}>
                                ({pagination.totalCount}) orders
                            </strong>
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
                                        <td colSpan={5} style={{ textAlign: "center", height:`${orders.length * 65}px` }}>
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
                <p>No orders found.</p>
            )}


            <div style={{ float: "right", padding: "0 5%" }}>
                <Pagination
                    pagination={pagination}
                    onPageChange={onPageChange}
                />
            </div>
        </div>
    )
}

export default OrderList