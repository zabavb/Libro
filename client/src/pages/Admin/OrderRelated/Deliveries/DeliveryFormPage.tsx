import { useNavigate, useParams } from "react-router-dom"
import DeliveryTypeFormContainer from "../../../../containers/order/DeliveryTypeFormContainer"

const DeliveryTypeFormPage = () => {
    const { deliveryTypeId } = useParams<{deliveryTypeId: string}>()
    const navigate = useNavigate()

    const handleGoBack = () => {
        navigate("/admin/deliverytypes")
    }

    return (
        <div>
            <header>
                <h1>{deliveryTypeId == undefined ? "Add Delivery Type" : "Edit Delivery Type"}</h1>
                <button onClick={handleGoBack}>Back to Delivery Type List</button>
            </header>
            <main>
                <DeliveryTypeFormContainer deliveryTypeId={deliveryTypeId ?? ""}/>
            </main>
        </div>
    )
}

export default DeliveryTypeFormPage