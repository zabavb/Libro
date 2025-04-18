import { useDispatch } from "react-redux"
import { AppDispatch } from "../../state/redux"
import { Order, ServiceResponse } from "../../types"
import React, { useEffect, useState } from "react"
import { addNotification } from "../../state/redux/slices/notificationSlice"
import { fetchOrderByIdService } from "../../services"
import OrderDetails from "@/components/order/OrderDetails"



interface OrderDetailsContainerProps {
	orderId: string
}

const OrderDetailsContainer: React.FC<OrderDetailsContainerProps> = ({ orderId }) => {
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
        })();
    }, [orderId, dispatch]);

    return (
        <OrderDetails
            existingOrder={serviceResponse.data ?? undefined}
            loading={serviceResponse.loading}
        />
    )

}

export default OrderDetailsContainer
