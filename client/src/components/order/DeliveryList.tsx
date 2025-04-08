import React from "react";
import { DeliveryType } from "../../types";
import DeliveryTypeAdminCardContainer from "../../containers/order/DeliveryTypeAdminCardContainer";
import Pagination from "../common/Pagination";
import Search from "../common/Search";
import DeliverySort from "./DeliverySort";

interface DeliveryTypeListProps {
    deliveryTypes?: DeliveryType[]
    loading: boolean
    pagination: { pageNumber: number; pageSize: number; totalCount: number }
    onPageChange: (pageNumber: number) => void
    onNavigate: (path: string) => void
    onSortChange: (field: keyof DeliverySort) => void
    sort: DeliverySort
    onSearchTermChange: (searchTerm: string) => void
    searchTerm: string
    handleNavigate: (path: string) => void
}

const DeliveryTypeList: React.FC<DeliveryTypeListProps> = ({
    deliveryTypes = [],
    loading,
    pagination,
    onPageChange,
    searchTerm,
    onSearchTermChange,
    handleNavigate
}) => {
    if (loading) return <p>Loading...</p>
    return (
        <div style={{height:"100dvh"}}>
            <link rel="stylesheet" href="/src/styles/orderTableStyles.css" />
            <div style={{ height: "5dvh", display:"flex" }}>
                <Search
                    searchTerm={searchTerm}
                    onSearchTermChange={onSearchTermChange} />
                <button onClick={() => handleNavigate("/admin/deliveryTypes/add")}>
                    Add Delivery
                </button>
            </div>
            <div style={{ height: "100%" }}>
                {deliveryTypes.length > 0 ? (
                    <div style={{ display: "flex", flexDirection: "column" }}>
                        <div style={{ display: "flex", flexDirection: "row-reverse" }}>
                            <p style={{ margin: "0 5%" }}>
                                <strong style={{ color: "#ff642e" }}>
                                    ({pagination.totalCount}) delivery types
                                </strong>
                            </p>
                        </div>
                        <div className="table-wrapper">
                            <table>
                                <thead style={{ margin: "20px" }}>
                                    <tr>
                                        <th style={{ width: "50%" }}>Service name</th>
                                        <th style={{ width: "40%" }}>Delivery</th>
                                        <th style={{ width: "10%" }}></th>
                                    </tr>
                                </thead>
                                {loading ? (
                                    <tr>
                                        <td colSpan={5} style={{ textAlign: "center", height: `${deliveryTypes.length * 65}px` }}>
                                            Loading...
                                        </td>
                                    </tr>
                                )
                                    : (
                                        <tbody>
                                            {deliveryTypes.map((deliveries) => (
                                                <DeliveryTypeAdminCardContainer
                                                    key={deliveries.id}
                                                    deliveryType={deliveries}
                                                />
                                            ))}
                                        </tbody>
                                    )}
                            </table>
                        </div>
                    </div>
                ) : (
                    <p>No delivery types found.</p>
                )}
            </div>
            <div style={{ height: "5dvh" }}>
                <div style={{ float: "right", padding: "0 5%" }}>
                    <Pagination
                        pagination={pagination}
                        onPageChange={onPageChange}
                    />
                </div>
            </div>
        </div>
    )


}

export default DeliveryTypeList