import { Status } from "../subObjects/Status"

export interface OrderView {
    id: string
    // userId: string There's no need for userId because data is already fetched based on it(?)
    bookIds: Array<string>
    region: string
    city: string
    address: string
    price: number
    deliveryPrice: number
    orderDate: Date
    deliveryDate: Date
    status: Status
    deliveryType: string // To be implemented with DeliveryRepository
}