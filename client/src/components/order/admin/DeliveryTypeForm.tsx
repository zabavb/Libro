import { zodResolver } from "@hookform/resolvers/zod";
import { DeliveryType } from "../../../types";
import { DeliveryTypeFormData, deliveryTypeSchema } from "../../../utils";
import { useEffect } from "react";
import { useForm } from "react-hook-form";

interface DeliveryTypeFormProps {
    existingDeliveryType?: DeliveryType
    onAddDeliveryType: (deliveryType :DeliveryType) => Promise<void>;
    onEditDeliveryType: (existingDeliveryType: DeliveryType)=> Promise<void>;
    loading: boolean;
}

const DeliveryTypeForm: React.FC<DeliveryTypeFormProps> = ({existingDeliveryType, onAddDeliveryType, onEditDeliveryType}) => {
    const {
        register,
        handleSubmit,
        setValue,
        formState: {errors}
    } = useForm<DeliveryTypeFormData>({
        resolver: zodResolver(deliveryTypeSchema),
        defaultValues:
        {
            serviceName: ""
        }
    })

    useEffect(() =>{
        if(existingDeliveryType){
            setValue("serviceName", existingDeliveryType.serviceName)
        }
    }, [existingDeliveryType, setValue])

    const onSubmit = (data: DeliveryTypeFormData) => {
        const deliveryType: DeliveryType = {
            id: existingDeliveryType ? existingDeliveryType.id  :  "00000000-0000-0000-0000-000000000000",
            serviceName: data.serviceName
        }

        if(existingDeliveryType){
            onEditDeliveryType(deliveryType)
        }
        else{
            onAddDeliveryType(deliveryType)
        }
    }

    return (
        <form onSubmit={handleSubmit(onSubmit)}>
            <input {...register("serviceName")} 
            placeholder="Service Name"/>
            <p>{errors.serviceName?.message}</p>

            <button type="submit">{existingDeliveryType ? "Update Delivery Type" : "Add Delivery Type"}</button>
        </form>
    )
}

export default DeliveryTypeForm