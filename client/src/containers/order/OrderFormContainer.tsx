import { useDispatch } from "react-redux"
import { AppDispatch, editOrder, fetchBooks, RootState } from "../../state/redux"
import { useSelector } from "react-redux"
import { useNavigate } from "react-router-dom"
import { Order } from "../../types"
import React, { useEffect, useState } from "react"
import { resetOrderOperationStatus } from "../../state/redux/slices/orderSlice"
import OrderForm from "../../components/order/admin/OrderForm"
import { addNotification } from "../../state/redux/slices/notificationSlice"



interface OrderFormContainerProps {
	orderId: string
}

const OrderFormContainer: React.FC<OrderFormContainerProps> = ({ orderId }) => {
    const dispatch = useDispatch<AppDispatch>()
    const {data: orders, operationStatus, error } = useSelector((state: RootState) => state.orders)
    const {data: books} = useSelector((state: RootState) => state.books) 
    const [order, setOrder] = useState<Order>()
    const { pageSize, totalCount } = useSelector(
        (state: RootState) => state.books.pagination
      );
    
    const navigate = useNavigate()


    useEffect(() => {
        if (orders.length === 0) return;
        console.log(orders);
        const foundOrder = orders.find((order) => order.id == orderId)
        console.log(foundOrder)
        if(!foundOrder){
            navigate("/admin/orders")
        }
        else{
            setOrder(foundOrder)
        }
    },[orderId,orders,navigate])
    const [page, setPage] = useState(1)


    const handlePageChange = (page: number) =>{
        if(page > 0 && page <= totalCount / pageSize){
            setPage(page)
            dispatch(fetchBooks({pageNumber:page,pageSize:pageSize}))
        }
    }

    const handleEditOrder = (id: string, order: Order) => {
        console.log("Editing order:", id, order)
        dispatch(editOrder({id,order}))
    }

    useEffect(() => {
        // Forcing delivery types to be loaded into Store if they weren't

        if(books.length === 0){
            dispatch(fetchBooks({pageNumber:page,pageSize:pageSize}))
        }
        if (operationStatus === "success"){
            dispatch(
                addNotification({
                    message: "Order updated successfully!",
                    type: "success"
                })
            )
            dispatch(resetOrderOperationStatus())
            navigate("/admin/orders")
        } else if (operationStatus === "error") {
            dispatch(
                addNotification({
                    message: error,
                    type: "error",
                })
            )
            dispatch(resetOrderOperationStatus())
        }
    }, [operationStatus, error, dispatch, navigate, books, page, pageSize])

    return (
        <OrderForm
            page={page}
            books={books}
            existingOrder={order as Order}
            onEditOrder={handleEditOrder}
            onPageChange={handlePageChange}
        />
    )

}

export default OrderFormContainer
