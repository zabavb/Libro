import { Status  } from "../../types";

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

