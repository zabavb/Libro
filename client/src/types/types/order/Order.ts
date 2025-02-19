export interface Order {
    id: string
    userId: string
    bookIds: Array<string>
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