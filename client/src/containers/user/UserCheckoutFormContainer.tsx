import { useDispatch } from "react-redux"
import { AppDispatch } from "../../state/redux"
import { useNavigate } from "react-router-dom"
import React, { useCallback, useEffect, useMemo, useState } from "react"
import { addNotification } from "../../state/redux/slices/notificationSlice"
import { DeliveryType, Order } from "../../types"
import UserCheckoutForm from "../../components/user/UserCheckoutForm"
import useCart from "../../state/context/useCart"
import { addOrderService, fetchDeliveryTypesService } from "../../services"


const UserCheckoutFormContainer: React.FC = () => {
    const dispatch = useDispatch<AppDispatch>()
    const booksObjs = useMemo(() => ({} as Record<string, number>), []);
    const [price, setPrice] = useState<number>(0)
    const {cart, clearCart } = useCart();
    const navigate = useNavigate()
    const [deliveryTypes, setDeliveryTypes] = useState<DeliveryType[]>([])
    const [loading, setLoading] = useState<boolean>(true);
    const [pagination, setPagination] = useState({
        pageNumber: 1,
        pageSize: 10,
        totalCount: 0,
    })
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
                handleMessage('Order Placed successfully!', 'success');
                clearCart();
                handleNavigate('/');

            }
        },
        [handleMessage, handleNavigate,clearCart]
    )

   const paginationMemo = useMemo(() => ({...pagination}), [pagination]);

    const fetchDeliveryTypeList = useCallback(async () => {
        setLoading(true);
        try{
            const response = await fetchDeliveryTypesService(
                paginationMemo.pageNumber,
                paginationMemo.pageSize,
            );

            if(response.error)
                dispatch(
                    addNotification({
                        message: response.error,
                        type:'error',
                    }),
                );

            if(response && response.data) {
                const paginatedData = response.data;

                setDeliveryTypes(paginatedData.items);
                setPagination({
                    pageNumber: paginatedData.pageNumber,
                    pageSize: paginatedData.pageSize,
                    totalCount: paginatedData.totalCount
                })
            }else throw new Error('invalid response structure');
        }catch(error){
            dispatch(
                addNotification({
                    message: error instanceof Error ? error.message : String(error),
                    type: 'error'
                })
            )
            setDeliveryTypes([])
        }
        setLoading(false);
    }, [paginationMemo, dispatch])

    useEffect(() => {
        let newPrice = 0;
        for (const book of cart) {
            booksObjs[book.bookId] = book.amount;
            newPrice += book.amount * book.price
        }
        setPrice(newPrice);

        fetchDeliveryTypeList()
        // eslint-disable-next-line react-hooks/exhaustive-deps
    },[])


    return (
        <UserCheckoutForm
            price={price}
            books={booksObjs}
            deliveryTypes={deliveryTypes}
            onAddOrder={handleAddOrder}
            loading={loading}
        />
    )

}

export default UserCheckoutFormContainer