import OrderDetailsContainer from "@/containers/order/OrderDetailsContainer"
import { useParams } from "react-router-dom"


const OrderDetailsPage = () => {
    const { orderId } = useParams<{orderId: string}>()

    return (
        <div>
            <main>
                <OrderDetailsContainer orderId={orderId ?? ""} />
            </main>
        </div>
    )
}

export default OrderDetailsPage