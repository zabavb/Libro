import type { OrderSort } from "../../types";

interface OrderSortProps {
    onSortChange: (field: keyof OrderSort) => void
    sort: OrderSort
}

const OrderSort: React.FC<OrderSortProps> = ({ onSortChange, sort}) => {
    return (
        <div>
            <button 
                style={{marginLeft: "10px", marginRight:"10px"}}
                onClick={() => onSortChange("orderDate")}>
                Order Date {sort.orderDate === true ? "↑" : sort.orderDate === false ? "↓" : ""}
            </button>
            <button 
                style={{marginLeft: "10px", marginRight:"10px"}}
                onClick={() => onSortChange("booksAmount")}>
                Books Amount {sort.booksAmount === true ? "↑" : sort.booksAmount === false ? "↓" : ""}
            </button>
            <button 
                style={{marginLeft: "10px", marginRight:"10px"}}
                onClick={() => onSortChange("orderPrice")}>
                Order Price {sort.orderPrice === true ? "↑" : sort.orderPrice === false ? "↓" : ""}
            </button>
            <button 
                style={{marginLeft: "10px", marginRight:"10px"}}
                onClick={() => onSortChange("deliveryPrice")}>
                Delivery Price {sort.deliveryPrice === true ? "↑" : sort.deliveryPrice === false ? "↓" : ""}
            </button>
            <button 
                style={{marginLeft: "10px", marginRight:"10px"}}
                onClick={() => onSortChange("deliveryDate")}>
                Delivery Date {sort.deliveryDate === true ? "↑" : sort.deliveryDate === false ? "↓" : ""}
            </button>
            <button 
                style={{marginLeft: "10px", marginRight:"10px"}}
                onClick={() => onSortChange("statusSort")}>
                Status {sort.statusSort === true ? "↑" : sort.statusSort === false ? "↓" : ""}
            </button>
        </div>
    )
}

export default OrderSort