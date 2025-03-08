import type { DeliverySort } from "../../types";

interface DeliverySortProps {
    onSortChange: (field: keyof DeliverySort) => void
    sort: DeliverySort
}

const DeliverySort: React.FC<DeliverySortProps> = ({onSortChange, sort}) => {
    return (
        <div>
            <button onClick={() => onSortChange("serviceName")}>
                Service Name {sort.serviceName === true ? "↑" : sort.serviceName === false ? "↓" : ""}
            </button>
        </div>
    )
}

export default DeliverySort