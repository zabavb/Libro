import { StatusView } from "@/types/subTypes/order/StatusView";
import { Order, Status, OrderView  } from "../../types";
import { dateToString } from "./commonAdapters";

export const statusNumberToEnum = (statusNumber: number): Status =>{
    const statusMap: { [key: number]: Status } =
    {
        0: Status.PENDING,
        1: Status.PROCESSING,
        2: Status.TRANSIT,
        3: Status.PAYMENT,
        4: Status.COMPLETED
    } 

    return statusMap[statusNumber] ?? Status.PENDING
}

export const statusEnumToNumber = (status: Status): number => {
    const statusMap: { [key in Status ]: number } = {
        [Status.PENDING]: 0,
        [Status.PROCESSING]: 1,
        [Status.TRANSIT]: 2,
        [Status.PAYMENT]: 3,
        [Status.COMPLETED]: 4,
    }

    return statusMap[status] ?? 0
}

export const statusEnumToStatusView = (status: Status): StatusView => {
  const roleMap: { [key in Status]: StatusView } = {
        [Status.PENDING]: StatusView.PENDING,
        [Status.PROCESSING]: StatusView.PROCESSING,
        [Status.TRANSIT]: StatusView.TRANSIT,
        [Status.PAYMENT]: StatusView.PAYMENT,
        [Status.COMPLETED]: StatusView.COMPLETED,
  };

  return roleMap[status] ?? StatusView.PENDING;
};


export const OrderToOrderView = (response: Order): OrderView => {
    return {
        id: response.id,
        books: response.books,
        region: response.region,
        city: response.city,
        address: response.address,
        price: response.price,
        deliveryPrice: response.deliveryPrice,
        orderDate: dateToString(response.orderDate),
        deliveryDate: dateToString(response.deliveryDate),
        status: statusNumberToEnum(response.status),
        deliveryType: response.deliveryTypeId.toString() // Temporary solution
    }
}

