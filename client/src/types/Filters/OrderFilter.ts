import { Status } from "../subTypes/Status"

export interface OrderFilter{
    orderDateStart?: string
    orderDateEnd?: string
    deliveryDateStart?: string
    deliveryDateEnd?: string
    status?: Status
    deliveryId?: string
}