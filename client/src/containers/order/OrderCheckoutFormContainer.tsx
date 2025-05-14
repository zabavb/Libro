import { useDispatch } from "react-redux"
import { AppDispatch } from "../../state/redux"
import React, { useCallback, useEffect, useMemo, useState } from "react"
import { addNotification } from "../../state/redux/slices/notificationSlice"
import { DeliveryType, User } from "../../types"
import OrderCheckoutForm from "../../components/order/OrderCheckoutForm"
import { useCart } from "../../state/context/CartContext"
import { fetchDeliveryTypesService } from "../../services"
import { getUserFromStorage } from "@/utils/storage"
import { CartItem } from "@/types/types/cart/CartItem"


const OrderCheckoutFormContainer: React.FC = () => {
    const dispatch = useDispatch<AppDispatch>()
    const booksObjs = useMemo(() => ({} as Record<string, number>), []);
    const [price, setPrice] = useState<number>(0)
    const {getTotalPrice, cart, clearItem, addItem, removeItem } = useCart();
    const [deliveryTypes, setDeliveryTypes] = useState<DeliveryType[]>([])
    const [loading, setLoading] = useState<boolean>(true);
    const [pagination, setPagination] = useState({
        pageNumber: 1,
        pageSize: 10,
        totalCount: 0,
    })

    const user: User | null = getUserFromStorage();

    const handleAdd = (item: CartItem) => {
        addItem({ bookId: item.bookId, amount: 1, name: item.name, price:item.price});
    };
    
    const handleRemove = (item: CartItem) => {
        removeItem({ bookId: item.bookId, amount: 1, name: item.name, price:item.price});
    };

    const handleClear = (bookId: string) => {
        clearItem(bookId)
    }    

    // const handleMessage = useCallback(
    //     (message: string, type: 'success' | 'error') => {
    //     dispatch(addNotification({ message, type }));
    //     },
    //     [dispatch],
    // );

    // const handleNavigate = useCallback(
    //     (route: string) => navigate(route),
    //     [navigate],
    // );

    // const handleAddOrder = useCallback(
    //     async(order: Order) => {
    //         const response = await addOrderService(order);

    //         if (response.error) handleMessage(response.error, 'error');
    //         else {
    //             handleMessage('Order Placed successfully!', 'success');
    //             clearCart();
    //             handleNavigate('/');

    //         }
    //     },
    //     [handleMessage, handleNavigate,clearCart]
    // )

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

                setDeliveryTypes(paginatedData.items.sort((a,b) => a.serviceName.localeCompare(b.serviceName)));
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
        for (const book of cart) {
            booksObjs[book.bookId] = book.amount;
        }

        fetchDeliveryTypeList()
        // eslint-disable-next-line react-hooks/exhaustive-deps
    },[])

    useEffect(() => {
        setPrice(getTotalPrice());
    },[cart, addItem, removeItem, clearItem, getTotalPrice])


    return (
        <OrderCheckoutForm
            price={price}
            cart={cart}
            books={booksObjs}
            deliveryTypes={deliveryTypes}
            loading={loading}
            user={user}
            onAdd={handleAdd}
            onRemove={handleRemove}
            onItemClear={handleClear}
        />
    )

}

export default OrderCheckoutFormContainer