import { useDispatch } from "react-redux"
import { AppDispatch } from "../../state/redux"
import { useNavigate } from "react-router-dom"
import { DeliveryType, ServiceResponse } from "../../types"
import { useCallback, useEffect, useState } from "react"
import { addNotification } from "../../state/redux/slices/notificationSlice"
import DeliveryTypeForm from "../../components/order/admin/DeliveryTypeForm"
import { addDeliveryTypeService, editDeliveryTypeService, fetchDeliveryTypeByIdService } from "../../services"

interface DeliveryTypeFormContainerProps{
    deliveryTypeId: string
}

const DeliveryTypeFormContainer: React.FC<DeliveryTypeFormContainerProps> = ({deliveryTypeId}) => {
    const dispatch = useDispatch<AppDispatch>()
    const navigate = useNavigate()

    // const existingDeliveryType = deliveryTypes.find((deliveryType) => deliveryType.id == deliveryTypeId) ?? undefined
    const [serviceResponse, setServiceResponse] = useState<
    ServiceResponse<DeliveryType>
    >({
        data:null,
        loading: !!deliveryTypeId,
        error: null
    })

    useEffect(() => {

        (async () => {
            const response = await fetchDeliveryTypeByIdService(deliveryTypeId);
            setServiceResponse(response);

            if(response.error)
                dispatch(addNotification({message:response.error, type:'error'}))
        })();
    }, [deliveryTypeId, dispatch])

    const handleMessage = useCallback(
        (message: string, type: 'success' | 'error') => {
            dispatch(addNotification({message,type}));
        },
        [dispatch],
    );

    const handleNavigate = useCallback(
        (route: string) => navigate(route),
        [navigate],
    );

    const handleAddDeliveryType = useCallback (
        async(deliveryType: DeliveryType) => {
            const response = await addDeliveryTypeService(deliveryType);

            if(response.error) handleMessage(response.error,'error');
            else {
                handleMessage('Delivery type created successfully!', 'success')
                handleNavigate('/admin/deliveryTypes');
            }
        },
        [handleMessage, handleNavigate]
    )

    const handleEditDeliveryType = useCallback(
        async (existingDelivery: DeliveryType) => {
            if (!deliveryTypeId) return;

            const response = await editDeliveryTypeService(deliveryTypeId, existingDelivery);
            if (response.error) handleMessage(response.error, 'error');
            else {
                handleMessage('Delivery type updated successfully!', 'success');
                handleNavigate('/admin/deliveryTypes');
            }
        },
        [deliveryTypeId, handleMessage, handleNavigate]
    )

    return (
        <DeliveryTypeForm
            existingDeliveryType={serviceResponse.data ?? undefined}
            onAddDeliveryType={handleAddDeliveryType}
            onEditDeliveryType={handleEditDeliveryType}
            loading={serviceResponse.loading}
        />
    )
}

export default DeliveryTypeFormContainer