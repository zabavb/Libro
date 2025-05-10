import { zodResolver } from "@hookform/resolvers/zod";
import { DeliveryType } from "../../../types";
import { DeliveryTypeFormData, deliveryTypeSchema } from "../../../utils";
import { useEffect } from "react";
import { useForm } from "react-hook-form";
import "@/assets/styles/components/delivery/delivery-form.css"
import { useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";
interface DeliveryTypeFormProps {
    existingDeliveryType?: DeliveryType
    onAddDeliveryType: (deliveryType: DeliveryType) => Promise<void>;
    onEditDeliveryType: (existingDeliveryType: DeliveryType) => Promise<void>;
    loading: boolean;
}

const DeliveryTypeForm: React.FC<DeliveryTypeFormProps> = ({ existingDeliveryType, onAddDeliveryType, onEditDeliveryType }) => {
    const navigate = useNavigate();
    const {
        register,
        handleSubmit,
        setValue,
        formState: { errors }
    } = useForm<DeliveryTypeFormData>({
        resolver: zodResolver(deliveryTypeSchema),
        defaultValues:
        {
            serviceName: ""
        }
    })

    useEffect(() => {
        if (existingDeliveryType) {
            setValue("serviceName", existingDeliveryType.serviceName)
        }
    }, [existingDeliveryType, setValue])

    const onSubmit = (data: DeliveryTypeFormData) => {
        const deliveryType: DeliveryType = {
            id: existingDeliveryType ? existingDeliveryType.id : "00000000-0000-0000-0000-000000000000",
            serviceName: data.serviceName
        }

        if (existingDeliveryType) {
            onEditDeliveryType(deliveryType)
        }
        else {
            onAddDeliveryType(deliveryType)
        }
    }

    return (
        <div>
            <header className="header-container">
                <div className="flex h-10 gap-2.5">
                    <button className="cancel-button" onClick={() => navigate('/admin/delivery')}><FontAwesomeIcon icon={faArrowLeft} /></button>
                    <button className="update-button" type="submit">{existingDeliveryType ? "Update" : "Add"}</button>
                </div>

                <div className="profile-icon">
                    <div style={{ borderRadius: "50%", backgroundColor: "grey", height: "43px", width: "43px" }}></div>
                    <p className="profile-name">Name Surname</p>
                </div>
            </header>
            <form onSubmit={handleSubmit(onSubmit)}>
                <main className="main-container">
                    <div className="input-container">
                    <p className="input-title">Service Name</p>
                    <input
                        className="input-text" 
                        {...register("serviceName")}
                        placeholder="Service Name" />
                    <p>{errors.serviceName?.message}</p>
                    </div>
                </main>
            </form>
        </div>
    )
}

export default DeliveryTypeForm