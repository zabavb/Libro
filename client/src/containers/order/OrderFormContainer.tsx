import { useDispatch } from "react-redux"
import { AppDispatch, fetchBooks, RootState } from "../../state/redux"
import { useSelector } from "react-redux"
import { useNavigate } from "react-router-dom"
import { Order, ServiceResponse } from "../../types"
import React, { useCallback, useEffect, useState } from "react"
import OrderForm from "../../components/order/admin/OrderForm"
import { addNotification } from "../../state/redux/slices/notificationSlice"
import { addOrderService, editOrderService, fetchOrderByIdService } from "../../services"



interface OrderFormContainerProps {
	orderId: string
}

const OrderFormContainer: React.FC<OrderFormContainerProps> = ({ orderId }) => {
    const dispatch = useDispatch<AppDispatch>()
    const navigate = useNavigate()
    const [page, setPage] = useState(1)
    // old tmp
    const {data: books} = useSelector((state: RootState) => state.books) 
    const { pageSize, totalCount } = useSelector(
        (state: RootState) => state.books.pagination
      );
    const [serviceResponse, setServiceResponse] = useState<
        ServiceResponse<Order>
        >({
            data:null,
            loading: !!orderId,
            error: null,
        })

    useEffect(() => {
        if(books.length === 0){
            dispatch(fetchBooks({pageNumber:page,pageSize:pageSize}))
        }
        if (!orderId) return;

        (async () => {
            const response = await fetchOrderByIdService(orderId);
            setServiceResponse(response);

            if(response.error)
                dispatch(addNotification({message:response.error, type: 'error'}));
        })();
    }, [orderId, dispatch, books, page, pageSize]);

    const handleMessage = useCallback(
        (message: string, type: 'success' | 'error') => {
        dispatch(addNotification({ message, type }));
        },
        [dispatch],
    );

    const handleNavigate = useCallback(
        (route: string) => navigate(route),
        [navigate],
    );

    const handleAddOrder = useCallback(
        async(order: Order) => {
            const response = await addOrderService(order);

            if (response.error) handleMessage(response.error, 'error');
            else {
                handleMessage('Order created successfully!', 'success');
                handleNavigate('/admin/orders');
            }
        },
        [handleMessage, handleNavigate]
    )

    const handlePageChange = (page: number) =>{
        if(page > 0 && page <= totalCount / pageSize){
            setPage(page)
            dispatch(fetchBooks({pageNumber:page,pageSize:pageSize}))
        }
    }

    const handleEditOrder = useCallback(
        async (existingOrder: Order) => {
            if (!orderId) return;

            const response = await editOrderService(orderId, existingOrder);
            if (response.error) handleMessage(response.error, 'error');
            else handleMessage('Order updated successfully!', 'success');
        },
        [orderId, handleMessage]
    )

    return (
        <OrderForm
            page={page}
            books={books ?? []}
            existingOrder={serviceResponse.data ?? undefined}
            onEditOrder={handleEditOrder}
            onAddOrder={handleAddOrder}
            onPageChange={handlePageChange}
            loading={serviceResponse.loading}
        />
    )

}

export default OrderFormContainer
