import { CoverType } from "@/types/subTypes/book/CoverType";
import { Status } from "@/types/subTypes/Order/Status";

export interface OrderDetails{
    orderId:string;
    price: number;
    created: Date;
    status: Status;
    orderBooks: BookOrderDetails[]
}

export interface BookOrderDetails{
    bookId: string;
    title: string;
    authorName: string;
    price: number;
    imageUrl: string;
    amount: number;

    // coverType: CoverType;
}