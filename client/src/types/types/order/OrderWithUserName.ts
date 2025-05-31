import { Status } from "@/types/subTypes/Order/Status";

export interface OrderWithUserName
{
    orderUiId: string;
    price: number;
    status: Status;
    firstName: string;
    lastName: string;
}