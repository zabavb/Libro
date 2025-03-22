import React from "react";
import { DeliveryType } from "../../types";
import DeliveryTypeAdminCardContainer from "../../containers/order/DeliveryTypeAdminCardContainer";
import Pagination from "../common/Pagination";
import Search from "../common/Search";
import DeliverySort from "./DeliverySort";

interface DeliveryTypeListProps {
    deliveryTypes?: DeliveryType[]
    loading: boolean
    error: string | null
    pagination: {pageNumber: number; pageSize: number; totalCount: number}
    onPageChange: (pageNumber: number) => void
    onNavigate: (path: string) => void
    onSortChange: (field: keyof DeliverySort) => void
    sort: DeliverySort
    onSearchTermChange: (searchTerm: string) => void
    searchTerm: string
}

const DeliveryTypeList: React.FC<DeliveryTypeListProps> = ({
    deliveryTypes = [],
    loading,
    error,
    pagination,
    onPageChange,
    searchTerm,
    onSearchTermChange,
    sort,
    onSortChange
}) => {
    if (loading) return <p>Loading...</p>
    if (error) return <p>Error: {error}</p>
    return (
        <div>
            <Search
                searchTerm={searchTerm}
                onSearchTermChange={onSearchTermChange}/>

            <DeliverySort
                sort={sort}
                onSortChange={onSortChange}/>

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