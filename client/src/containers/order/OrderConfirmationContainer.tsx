import OrderConfirmation from "@/components/order/OrderConfirmation"
import { addOrderService } from "@/services"
import useCart from "@/state/context/useCart"
import { Order, User } from "@/types"
import { useCallback } from "react"
import { useNavigate } from "react-router-dom"

interface OrderConfirmationContainerProps {
    order: Order
    user: User
}

const OrderConfirmationContainer: React.FC<OrderConfirmationContainerProps> = ({order, user}) => {
    const navigate = useNavigate(); 
    const {clearCart} = useCart();
    // const handleMessage = useCallback(
    //     (message: string, type: 'success' | 'error') => {
    //     dispatch(addNotification({ message, type }));
    //     },
    //     [dispatch],
    // );

    const handleNavigate = useCallback(
        (route: string) => navigate(route),
        [navigate],
    );

    const handleConfirmOrder = useCallback(
        async(order: Order) => {
            const response = await addOrderService(order);
            console.log(response)
            if (!response.error) {
                clearCart();
                handleNavigate('/checkout/success');
            }
        },
        [handleNavigate, clearCart]
    )

    
    return (
        <div>
            <OrderConfirmation onConfirm={handleConfirmOrder} order={order} user={user}/>
        </div>
    )
}

export default OrderConfirmationContainer