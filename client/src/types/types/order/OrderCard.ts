export interface OrderCard{
    orderId: string
    region: string
    city: string
    address: string
    fullPrice: number
    orderDate: Date
    deliveryDate: Date
    status: number 

    books: BookFetchWrapper;
    delivery: DeliveryFetchWrapper;
}

interface BookFetchWrapper {
    items: BookDetailsSnippet[];
    isFailedToFetch: boolean;
}

interface BookDetailsSnippet {
    bookId: string;
    title: string;
}

interface DeliveryFetchWrapper {
    item: DeliveryDetailSnippet;
    isFailedToFetch: boolean;
}

interface DeliveryDetailSnippet{
    deliveryId: string;
    serviceName: string;
}