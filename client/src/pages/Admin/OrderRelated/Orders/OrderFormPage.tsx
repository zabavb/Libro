import { useParams } from "react-router-dom"
import OrderFormContainer from "../../../../containers/order/OrderFormContainer"


const OrderFormPage = () => {
    const { orderId } = useParams<{orderId: string}>()

    return (
        <div>
            <main>
                <OrderFormContainer orderId={orderId ?? ""} />
            </main>
        </div>
    )
}

export default OrderFormPage