import { Status } from "../../subTypes/Status"
import { OrderedBook } from "./OrderedBook";

export interface OrderView {
    id: string
    // userId: string There's no need for userId because data is already fetched based on it(?)
    //books: Record<string,number>
    orderedBooks: OrderedBook[];
    region: string
    city: string
    address: string
    price: number
    deliveryPrice: number
    orderDate: string
    deliveryDate: string
    status: Status
    deliveryType: string // To be implemented with DeliveryRepository
}