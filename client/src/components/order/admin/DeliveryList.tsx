import React from "react";
import { DeliveryType } from "../../../types";
import DeliveryTypeAdminCardContainer from "../../../containers/order/DeliveryTypeAdminCardContainer";
import Pagination from "../../common/Pagination";
import Search from "../../common/Search";
import DeliverySort from "../DeliverySort";
import "@/assets/styles/base/table-styles.css"
import "@/assets/styles/components/list-styles.css"
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPlus } from "@fortawesome/free-solid-svg-icons";
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
        <div>
            <header className="header-container">
                <Search
                    searchTerm={searchTerm}
                    onSearchTermChange={onSearchTermChange} />
                <button className="add-button" onClick={() => handleNavigate("/admin/delivery/add")}>
                    <FontAwesomeIcon icon={faPlus} />
                    <p>
                        Add Delivery
                    </p>
                </button>
                {/* Temporary implementation, replace with user pfp component */}
                <div className="profile-icon">
                    <div style={{ borderRadius: "50%", backgroundColor: "grey", height: "43px", width: "43px" }}></div>
                    <p className="profile-name">Name Surname</p>
                </div>

            </header>
            <main className="main-container">
                {deliveryTypes.length > 0 ? (
                    <div className="flex flex-col w-full">
                        <div className="flex flex-row-reverse">
                            <p className="counter">
                                ({pagination.totalCount}) delivery types
                            </p>
                        </div>
                        <div className="table-wrapper">
                            <table>
                                <thead className="m-5">
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
            </main>
            <footer>
                <div className="pagination-container">
                    <Pagination
                        pagination={pagination}
                        onPageChange={onPageChange}
                    />
                </div>
            </footer>
        </div>
    )


}

export default DeliveryTypeList