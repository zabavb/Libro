import { Status } from "@/types/subTypes/order/Status";

export interface OrderWithUserName
{
    orderUiId: string;
    price: number;
    status: Status;
    firstName: string;
    lastName: string;
}