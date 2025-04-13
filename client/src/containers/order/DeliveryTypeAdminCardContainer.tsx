import { useDispatch } from "react-redux";
import { DeliveryType } from "../../types";
import { AppDispatch } from "../../state/redux";
import { useNavigate } from "react-router-dom";
import { addNotification } from "../../state/redux/slices/notificationSlice";
import DeliveryTypeAdminCard from "../../components/order/admin/DeliveryTypeAdminCard";
import { removeDeliveryTypeService } from "../../services";

interface DeliveryAdminCardContainerProps{
    deliveryType: DeliveryType
}

const DeliveryTypeAdminCardContainer: React.FC<DeliveryAdminCardContainerProps> = ({deliveryType}) => {
    const dispatch = useDispatch<AppDispatch>()
    const navigate = useNavigate()

    const handleDelete = async (e: React.MouseEvent) => {
        e.stopPropagation()
        const response = await removeDeliveryTypeService(deliveryType.id)
        dispatch(
              response.error
                ? addNotification({
                    message: response.error,
                    type: 'error',
                  })
                : addNotification({
                    message: 'Order successfully deleted.',
                    type: 'success',
                  }),
            );
    }

    const handleNavigate = () => {
        navigate(`/admin/delivery/${deliveryType.id}`)
    }

    return (
        <DeliveryTypeAdminCard 
            deliveryType={deliveryType}
            onNavigate={handleNavigate}
            onDelete={handleDelete}
            />
    )
}

export default DeliveryTypeAdminCardContainer