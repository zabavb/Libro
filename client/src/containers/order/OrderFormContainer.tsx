import { useDispatch } from "react-redux"
import { AppDispatch } from "../../state/redux"
import { Order, ServiceResponse } from "../../types"
import React, { useCallback, useEffect, useState } from "react"
import OrderForm from "../../components/order/admin/OrderForm"
import { addNotification } from "../../state/redux/slices/notificationSlice"
import { editOrderService, fetchOrderByIdService } from "../../services"



interface OrderFormContainerProps {
	orderId: string
}

const OrderFormContainer: React.FC<OrderFormContainerProps> = ({ orderId }) => {
    const dispatch = useDispatch<AppDispatch>()

    const [serviceResponse, setServiceResponse] = useState<
        ServiceResponse<Order>
        >({
            data:null,
            loading: !!orderId,
            error: null,
        })

    useEffect(() => {
        if (!orderId) return;

        (async () => {
            const response = await fetchOrderByIdService(orderId);
            setServiceResponse(response);

            if(response.error)
                dispatch(addNotification({message:response.error, type: 'error'}));
            console.log(response);
        })();
    }, [orderId, dispatch]);

    const handleMessage = useCallback(
        (message: string, type: 'success' | 'error') => {
        dispatch(addNotification({ message, type }));
        },
        [dispatch],
    );

    const handleEditOrder = useCallback(
        async (existingOrder: Order) => {
            if (!orderId) return;

            const response = await editOrderService(orderId, existingOrder);
            if (response.error) handleMessage(response.error, 'error');
            else handleMessage('Order updated successfully!', 'success');
        },
        [orderId, handleMessage]
    )

    return (
        <OrderForm
            existingOrder={serviceResponse.data ?? undefined}
            onEditOrder={handleEditOrder}
            loading={serviceResponse.loading}
        />
    )

}

export default OrderFormContainer
