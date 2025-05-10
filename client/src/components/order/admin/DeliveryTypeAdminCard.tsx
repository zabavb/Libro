import { DeliveryType } from "../../../types";
import { icons } from '@/lib/icons'
interface DeliveryTypeAdminCardProps {
    deliveryType: DeliveryType
    onDelete: (e: React.MouseEvent) => void
    onNavigate: () => void 
}

const DeliveryTypeAdminCard: React.FC<DeliveryTypeAdminCardProps> = ({deliveryType, onDelete,onNavigate}) => {
    const deliveryUid = deliveryType.id.split('-')[4];
    return(
        <>
            <tr>
                <td>
                    {deliveryType.serviceName}
                </td>
                <td>
                    {deliveryUid}...
                </td>
                <td>
                    <div className='flex gap-2'>
                        <button onClick={onNavigate} className='p-2.5 bg-[#FF642E] rounded-lg'><img src={icons.wPen} /></button>
                        <button onClick={onDelete} className='p-2.5 border-[1px] border-[#FF642E] rounded-lg'><img src={icons.oTrash} /></button>
                    </div>
                </td>
            </tr>
        </>
    )
}

export default DeliveryTypeAdminCard