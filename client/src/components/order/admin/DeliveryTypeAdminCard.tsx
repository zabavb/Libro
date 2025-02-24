import { DeliveryType } from "../../../types";

interface DeliveryTypeAdminCardProps {
    deliveryType: DeliveryType
    onDelete: (e: React.MouseEvent) => void
    onNavigate: () => void 
}

const DeliveryTypeAdminCard: React.FC<DeliveryTypeAdminCardProps> = ({deliveryType, onDelete,onNavigate}) => {
    return(
        <>
            <hr/>
            <li
                onClick={(e) => {
                    e.stopPropagation()
                    onNavigate()
                }}>
                <p>
                    <strong>Service Name:</strong>
                    {deliveryType.serviceName}
                </p>
                <button
                    onClick={(e) => {
                        e.stopPropagation()
                        onDelete(e)
                    }}>
                    Delete
                </button>
            </li>
        </>
    )
}

export default DeliveryTypeAdminCard