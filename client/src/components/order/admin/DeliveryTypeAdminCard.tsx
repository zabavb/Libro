import { DeliveryType } from "../../../types";

interface DeliveryTypeAdminCardProps {
    deliveryType: DeliveryType
    onDelete: (e: React.MouseEvent) => void
    onNavigate: () => void 
}

const DeliveryTypeAdminCard: React.FC<DeliveryTypeAdminCardProps> = ({deliveryType, onDelete,onNavigate}) => {
    const deliveryUid = deliveryType.id.split('-')[4];
    return(
        <>
            <tr
                onClick={(e) => {
                    e.stopPropagation()
                    onNavigate()
                }}
                style={{textAlign:"center"}}>
                <td>
                    {deliveryType.serviceName}
                </td>
                <td>
                    {deliveryUid}
                </td>
                <td>
                    <button
                        onClick={(e) => {
                            e.stopPropagation()
                            onDelete(e)
                        }}>
                        <img width="25" height="25" src="https://img.icons8.com/ios-filled/50/trash.png" alt="delete"/>
                    </button>
                </td>
            </tr>
        </>
    )
}

export default DeliveryTypeAdminCard