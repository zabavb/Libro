import { OrderedBook } from "./OrderedBook";

export interface Order {
    id: string
    userId: string
    //books: Record<string,number>
    orderedBooks: OrderedBook[];
    region: string
    city: string
    address: string
    price: number
    deliveryTypeId: string
    deliveryPrice: number
    orderDate: Date
    deliveryDate: Date
    status: number
}