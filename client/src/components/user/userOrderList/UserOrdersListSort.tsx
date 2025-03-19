import type { OrderSort } from "../../../types";

interface UserOrdersListSortProps {
    onSortChange: (field: keyof OrderSort) => void
    sort: OrderSort
}

const UserOrdersListSort: React.FC<UserOrdersListSortProps> = ({ onSortChange, sort}) => {
    return (
        <div>
            <button onClick={() => onSortChange("orderDate")}>
                Order Date {sort.orderDate === true ? "↑" : sort.orderDate === false ? "↓" : ""}
            </button>
            <button onClick={() => onSortChange("booksAmount")}>
                Books Amount {sort.booksAmount === true ? "↑" : sort.booksAmount === false ? "↓" : ""}
            </button>
            <button onClick={() => onSortChange("orderPrice")}>
                Order Price {sort.orderPrice === true ? "↑" : sort.orderPrice === false ? "↓" : ""}
            </button>
            <button onClick={() => onSortChange("deliveryPrice")}>
                Delivery Price {sort.deliveryPrice === true ? "↑" : sort.deliveryPrice === false ? "↓" : ""}
            </button>
            <button onClick={() => onSortChange("deliveryDate")}>
                Delivery Date {sort.deliveryDate === true ? "↑" : sort.deliveryDate === false ? "↓" : ""}
            </button>
            <button onClick={() => onSortChange("statusSort")}>
                Status {sort.statusSort === true ? "↑" : sort.statusSort === false ? "↓" : ""}
            </button>
        </div>
    )
}

export default UserOrdersListSort