import { useNavigate, useParams } from "react-router-dom"
import OrderFormContainer from "../../../../containers/order/OrderFormContainer"


const OrderFormPage = () => {
    const { orderId } = useParams<{orderId: string}>()
    const navigate = useNavigate()

    const handleGoBack = () => {
        navigate("/admin/orders")
    }

    return (
        <div>
            <header>
                <h1>{orderId ? "Edit Order" : "Add Order"}</h1>
                <button onClick={handleGoBack}>Back to Order List</button>
            </header>
            <main>
                <OrderFormContainer orderId={orderId ?? ""} />
            </main>
        </div>
    )
}

export default OrderFormPage