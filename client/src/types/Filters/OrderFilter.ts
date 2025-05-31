import { Status } from "../subTypes/order/Status"

export interface OrderFilter{
    orderDateStart?: string | null;
    orderDateEnd?: string | null;
    deliveryDateStart?: string | null;
    deliveryDateEnd?: string | null;
    status?: Status | null;
    deliveryId?: string | null;
    userId?: string | null;
}