import React from "react";
import { Status } from "../../../types";
import type { OrderFilter } from "../../../types";

interface OrdersListFilterProps {
    onFilterChange: (filters: OrderFilter) => void
    filters: OrderFilter
}

const UserOrdersListFilter: React.FC<OrdersListFilterProps> = ({onFilterChange, filters}) => {
    return (
        <div>
            <h3>Filters</h3>
            <label>
                Order date (Start):
                <input
                    type="date"
                    value={filters.orderDateStart ? filters.orderDateStart : ""}
                    onChange={(e) => {
                        onFilterChange({
                            ...filters,
                            orderDateStart: e.target.value
                        })
                    }}
                />
            </label>
            <label>
                Order date (End):
                <input
                    type="date"
                    value={filters.orderDateEnd ? filters.orderDateEnd : ""}
                    onChange={(e) => {
                        onFilterChange({
                            ...filters,
                            orderDateEnd: e.target.value
                        })
                    }}
                />
            </label>
            <label>
                Delivery date (Start):
                <input
                    type="date"
                    value={filters.deliveryDateStart ? filters.deliveryDateStart : ""}
                    onChange={(e) => {
                        onFilterChange({
                            ...filters,
                            deliveryDateStart: e.target.value
                        })
                    }}
                />
            </label>
            <label>
                Delivery date (End):
                <input
                    type="date"
                    value={filters.deliveryDateEnd ? filters.deliveryDateEnd : ""}
                    onChange={(e) => {
                        onFilterChange({
                            ...filters,
                            deliveryDateEnd: e.target.value
                        })
                    }}
                />
            </label>
            <label>
                Status:
                <select
                    value={filters.status || ""}
                    onChange={(e) => onFilterChange({ ...filters, status: e.target.value as Status})}>
                    <option value="">All</option>
                    {Object.values(Status).map((status) => (
                        <option
                            key={status}
                            value={status}>
                            {status}
                        </option>
                    ))}
                </select>
            </label>
            {/* Temporary implementation. This filter may be changed in the future */}
            <label>
                Delivery Id:
                <input
                type="text"
                onChange={(e) => onFilterChange({...filters, deliveryId: e.target.value})}/>
            </label>
        </div>
    )
}

export default UserOrdersListFilter