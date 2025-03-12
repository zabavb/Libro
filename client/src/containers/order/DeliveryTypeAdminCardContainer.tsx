import { useDispatch } from "react-redux";
import { DeliveryType } from "../../types";
import { AppDispatch, removeDeliveryType, RootState } from "../../state/redux";
import { useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import { useEffect } from "react";
import { addNotification } from "../../state/redux/slices/notificationSlice";
import { resetDeliveryTypeOperationStatus } from "../../state/redux/slices/deliveryTypeSlice";
import DeliveryTypeAdminCard from "../../components/order/admin/DeliveryTypeAdminCard";

interface DeliveryAdminCardContainerProps{
    deliveryType: DeliveryType
}

const DeliveryTypeAdminCardContainer: React.FC<DeliveryAdminCardContainerProps> = ({deliveryType}) => {
    const dispatch = useDispatch<AppDispatch>()
    const {operationStatus, error} = useSelector((state: RootState) => state.deliveryTypes)
    const navigate = useNavigate()

    const handleDelete = (e: React.MouseEvent) => {
        e.stopPropagation()
        dispatch(removeDeliveryType(deliveryType.id))
    }

    const handleNavigate = () => {
        navigate(`/admin/deliverytypes/${deliveryType.id}`)
    }

    useEffect(() => {
        if (operationStatus === "success")
        {
            addNotification({
                message: "Delivery type removed successfully.",
                type: "success"
            })
            dispatch(resetDeliveryTypeOperationStatus())
        } else if(operationStatus === "error"){
            dispatch(
                addNotification({
                    message: error,
                    type:"error"
                })
            )
            dispatch(resetDeliveryTypeOperationStatus())
        }
    }, [operationStatus, error, dispatch])

    return (
        <DeliveryTypeAdminCard 
            deliveryType={deliveryType}
            onNavigate={handleNavigate}
            onDelete={handleDelete}
            />
    )
}

export default DeliveryTypeAdminCardContainer