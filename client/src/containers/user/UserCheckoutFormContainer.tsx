import { useDispatch } from "react-redux"
import { addOrder, AppDispatch, RootState } from "../../state/redux"
import { useSelector } from "react-redux"
import { useNavigate } from "react-router-dom"
import React, { useEffect, useMemo, useState } from "react"
import { resetOrderOperationStatus } from "../../state/redux/slices/orderSlice"
import { addNotification } from "../../state/redux/slices/notificationSlice"
import { fetchDeliveryTypes } from "../../state/redux/slices/deliveryTypeSlice"
import { Order } from "../../types"
import UserCheckoutForm from "../../components/user/UserCheckoutForm"
import useBasket from "../../state/context/useBasket"


const UserCheckoutFormContainer: React.FC = () => {
    const dispatch = useDispatch<AppDispatch>()
    const {operationStatus:operationStatus, error:error } = useSelector((state: RootState) => state.orders)
    const {data: deliveryTypes} = useSelector((state: RootState) => state.deliveryTypes)
    const {data: books} = useSelector((state: RootState) => state.books) 
    const booksObjs = useMemo(() => ({} as Record<string, number>), []);
    const [price, setPrice] = useState<number>(0)
    const {basket, clearBasket } = useBasket();
    const navigate = useNavigate()

    const handleAddOrder = (order: Order) => {
        console.log("Placing order:", order)
        dispatch(addOrder(order))
    }

    useEffect(() => {
        let newPrice = 0;
        for (const book of basket) {
            booksObjs[book.bookId] = book.amount;
            newPrice += book.amount * book.price
        }
        setPrice(newPrice);

        // eslint-disable-next-line react-hooks/exhaustive-deps
    },[])

    useEffect(() => {
        // Forcing delivery types to be loaded into Store if they weren't

        if(deliveryTypes.length === 0){
            dispatch(fetchDeliveryTypes({pageNumber:1,pageSize:10}))
        }
        if (operationStatus === "success"){
            dispatch(
                addNotification({
                    message: "Order placed successfully!",
                    type: "success"
                })
            )
            dispatch(resetOrderOperationStatus())
            clearBasket()
            navigate("/")
        } else if (operationStatus === "error") {
            dispatch(
                addNotification({
                    message: error,
                    type: "error",
                })
            )
            dispatch(resetOrderOperationStatus())
        }
    }, [operationStatus, error, dispatch, navigate, deliveryTypes, clearBasket, basket, booksObjs, price, books])

    return (
        <UserCheckoutForm
            price={price}
            books={booksObjs}
            deliveryTypes={deliveryTypes}
            onAddOrder={handleAddOrder}
        />
    )

}

export default UserCheckoutFormContainer
