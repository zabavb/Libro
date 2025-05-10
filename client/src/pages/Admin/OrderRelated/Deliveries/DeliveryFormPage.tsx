import { useParams } from "react-router-dom"
import DeliveryTypeFormContainer from "../../../../containers/order/DeliveryTypeFormContainer"

const DeliveryTypeFormPage = () => {
    const { deliveryTypeId } = useParams<{deliveryTypeId: string}>()

    return (
        <div>
            <main>
                <DeliveryTypeFormContainer deliveryTypeId={deliveryTypeId ?? ""}/>
            </main>
        </div>
    )
}

export default DeliveryTypeFormPage