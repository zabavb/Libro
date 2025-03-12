import { useDispatch } from "react-redux"
import { addDeliveryType, AppDispatch, editDeliveryType, RootState } from "../../state/redux"
import { useSelector } from "react-redux"
import { useNavigate } from "react-router-dom"
import { DeliveryType } from "../../types"
import { useEffect } from "react"
import { addNotification } from "../../state/redux/slices/notificationSlice"
import { resetDeliveryTypeOperationStatus } from "../../state/redux/slices/deliveryTypeSlice"
import DeliveryTypeForm from "../../components/order/admin/DeliveryTypeForm"

interface DeliveryTypeFormContainerProps{
    deliveryTypeId: string
}

const DeliveryTypeFormContainer: React.FC<DeliveryTypeFormContainerProps> = ({deliveryTypeId}) => {
    const dispatch = useDispatch<AppDispatch>()
    const {data: deliveryTypes, operationStatus, error} = useSelector((state: RootState) => state.deliveryTypes)
    const navigate = useNavigate()

    const existingDeliveryType = deliveryTypes.find((deliveryType) => deliveryType.id == deliveryTypeId) ?? undefined


    const handleAddDeliveryType = (deliveryType: DeliveryType) => {
        console.log("Adding Delivery Type:", deliveryType)
        dispatch(addDeliveryType(deliveryType))
    }

    const handleEditDeliveryType = (id: string, deliveryType: DeliveryType) => {
        console.log("Editing Delivery Type:", id, deliveryType)
        dispatch(editDeliveryType({id,deliveryType}))
    }

    useEffect(() => {
        if(operationStatus === "success"){
            dispatch(
                addNotification({
                    message: existingDeliveryType ? "Delivery Type updated successfully!" :  "Delivery Type created successfully!",
                    type: "success",
                })
            )
            dispatch(resetDeliveryTypeOperationStatus())
            navigate("/admin/deliverytypes")
        } else if (operationStatus === "error"){
            dispatch(
                addNotification({
                    message: error,
                    type: "error",
                })
            )
            dispatch(resetDeliveryTypeOperationStatus())
        }

    }, [operationStatus, existingDeliveryType, error, dispatch, navigate])

    return (
        <DeliveryTypeForm
            existingDeliveryType={existingDeliveryType}
            onAddDeliveryType={handleAddDeliveryType}
            onEditDeliveryType={handleEditDeliveryType}
        />
    )
}

export default DeliveryTypeFormContainer