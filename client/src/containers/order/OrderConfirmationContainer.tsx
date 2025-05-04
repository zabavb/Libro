import OrderConfirmation from "@/components/order/OrderConfirmation"
import { addOrderService } from "@/services"
import useCart from "@/state/context/useCart"
import { Order, User } from "@/types"
import { useCallback, useEffect, useState } from "react"
import { useNavigate } from "react-router-dom"

interface OrderConfirmationContainerProps {
    order: Order
    user: User
}

const OrderConfirmationContainer: React.FC<OrderConfirmationContainerProps> = ({order, user}) => {
    const navigate = useNavigate();    
    const [total, setTotal] = useState<number>(0);
    const {clearCart, cart, getTotalPrice} = useCart();
    const handleNavigate = useCallback(
        (route: string) => navigate(route, {state: {fromRedirect: true}}),
        [navigate],
    );

    const handleConfirmOrder = useCallback(
        async(order: Order) => {
            const response = await addOrderService(order);
            if (!response.error) {
                handleNavigate('/checkout/success');
                clearCart();
            }
        },
        [handleNavigate, clearCart]
    )

    useEffect(() => {
        setTotal(getTotalPrice())
    },[cart, getTotalPrice])    


    return (
        <div>
            <OrderConfirmation
            onConfirm={handleConfirmOrder}
            order={order}
            user={user}
            cart={cart}
            total={total}/>
        </div>
    )
}

export default OrderConfirmationContainer