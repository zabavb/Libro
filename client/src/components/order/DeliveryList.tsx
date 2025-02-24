import React from "react";
import { DeliveryType } from "../../types";
import DeliveryTypeAdminCardContainer from "../../containers/order/DeliveryTypeAdminCardContainer";
import Pagination from "../common/Pagination";

interface DeliveryTypeListProps {
    deliveryTypes?: DeliveryType[]
    loading: boolean
    error: string | null
    pagination: {pageNumber: number; pageSize: number; totalCount: number}
    onPageChange: (pageNumber: number) => void
    onNavigate: (path: string) => void
}

const DeliveryTypeList: React.FC<DeliveryTypeListProps> = ({
    deliveryTypes = [],
    loading,
    error,
    pagination,
    onPageChange,
    onNavigate,
}) => {
    if (loading) return <p>Loading...</p>
    if (error) return <p>Error: {error}</p>
    return (
        <div>
             <p onClick={() => onNavigate("/admin")}>Back to Admin Dashboard</p>
             <p onClick={() => onNavigate("/admin/deliverytypes/add")}>Add Delivery Type</p>
            <h1>Delivery type list</h1>

            {loading ? (<>tmp</>) : deliveryTypes.length > 0 ? (
                deliveryTypes.map((deliveryType) => (
                    <DeliveryTypeAdminCardContainer
                        key={deliveryType.id}
                        deliveryType={deliveryType}
                    />
                ))
            ) : (
                <p>No delivery types found.</p>
            )}

            <hr/>

            <Pagination
                pagination={pagination}
                onPageChange={onPageChange}
            />
        </div>
    )

    
}

export default DeliveryTypeList