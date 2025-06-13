import React, { useState } from "react";
import { Status } from "../../../types";
import type { OrderFilter } from "../../../types";
import '@/assets/styles/components/user/user-orders-filter.css'
import { statusEnumToStatusView } from "@/api/adapters/orderAdapters";
interface OrdersListFilterProps {
    onFilterChange: (filters: OrderFilter) => void
    filters: OrderFilter
}

const UserOrdersListFilter: React.FC<OrdersListFilterProps> = ({ onFilterChange, filters }) => {
    const [activeStatus, setActiveStatus] = useState<Status>()

    const handleFilterChange = (newStatus?: Status) => {
        onFilterChange({ ...filters, status:newStatus })
        setActiveStatus(newStatus)
    }

    return (
        <div className="user-orders-filter-container">
            <div className="flex gap-5 cursor-pointer" 
                tabIndex={0}
                role="button"
                onClick={() => handleFilterChange(undefined)}
                  onKeyDown={(e) => {
                    if (e.key === 'Enter' || e.key === ' ') {
                    e.preventDefault();
                    handleFilterChange(undefined);
                    }
                }}>
                <div className={`w-1 rounded-r-[10px] ${activeStatus === undefined ? 'bg-[#FF642E]' : 'bg-transparent'}`}></div>
                <p className={`${activeStatus === undefined ? 'text-[#FF642E]' : 'text-[#1A1D23]'}`}>All</p>
            </div>
            {Object.values(Status).map((status) => (
            <div className="flex gap-5 cursor-pointer"
                tabIndex={0}
                role="button"
                onClick={() => handleFilterChange(status)}
                onKeyDown={(e) => {
                    if (e.key === 'Enter' || e.key === ' ') {
                    e.preventDefault();
                    handleFilterChange(status);
                    }
                }}>
                <div className={`w-1 rounded-r-[10px] ${activeStatus === status ? 'bg-[#FF642E]' : 'bg-transparent'}`}></div>
                <p className={`${activeStatus === status ? 'text-[#FF642E]' : 'text-[#1A1D23]'}`}>{statusEnumToStatusView(status)}</p>
            </div>
            ))}
        </div>
    )
}

export default UserOrdersListFilter