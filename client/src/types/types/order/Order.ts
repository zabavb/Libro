export interface Order {
    id: string
    userId: string
    books: Record<string,number>
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