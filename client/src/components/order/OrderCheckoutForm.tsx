import { useForm } from "react-hook-form";
import { DeliveryType, Order, Status, User } from "../../types";
import { zodResolver } from "@hookform/resolvers/zod";
import React, { useEffect, useState } from "react";
import { statusEnumToNumber } from "../../api/adapters/orderAdapters";
import { OrderFormData, orderSchema } from "../../utils";
import { useNavigate } from "react-router-dom";
import "@/assets/styles/components/order/checkout-form.css"
import { CartItem } from "@/types/types/cart/CartItem";
import CheckoutListing from "../common/CheckoutListing";
import CartCheckoutCard from "../common/CartCheckoutCard";
import { icons } from '@/lib/icons'

interface OrderCheckoutFormProps {
    books: Record<string, number>
    deliveryTypes?: DeliveryType[]
    price: number
    cart: CartItem[]
    loading: boolean
    user: User | null
    onAdd: (item: CartItem) => void;
    onRemove: (item: CartItem) => void;
    onItemClear: (bookId: string) => void;
}

const OrderCheckoutForm: React.FC<OrderCheckoutFormProps> = ({ onAdd, onItemClear, onRemove, cart, books, price, deliveryTypes, loading, user }) => {
    const [userId, setUserId] = useState<string>("");
    const [edit, setEdit] = useState<boolean>(false);
    const [paymentMethod, setPaymentMethod] = useState<string>("");
    const navigate = useNavigate()

    const toggleEdit = () => { setEdit(!edit); }

    const {
        register,
        handleSubmit,
        setValue,
        formState: { errors },
        watch
    } = useForm<OrderFormData>({
        resolver: zodResolver(orderSchema),
        defaultValues:
        {
            userId: "",
            books: {},
            address: "",
            region: "",
            city: "",
            orderDate: new Date().toString(),
            deliveryDate: new Date(Date.now() + 7 * 24 * 60 * 60 * 1000).toString(),
            price: price,
            deliveryPrice: 0,
            deliveryTypeId: "",
            status: Status.PENDING
        },
    })

    const deliveryTypeId = watch('deliveryTypeId');

    useEffect(() => {
        const json = localStorage.getItem('user');
        if (json != null) {
            const user = JSON.parse(json)
            setUserId(user.id)
            setValue("userId", user.id);
        }
        else {
            navigate('/')
        }
    }, [navigate, loading, setValue, userId])

    const onSubmit = (data: OrderFormData) => {

        const order: Order = {
            id: "00000000-0000-0000-0000-000000000000",
            userId: userId,
            books: books,
            region: data.region,
            city: data.city,
            address: data.address,
            orderDate: new Date(data.orderDate),
            deliveryDate: new Date(data.deliveryDate),
            price: data.price,
            deliveryPrice: 0,
            deliveryTypeId: data.deliveryTypeId,
            status: statusEnumToNumber(data.status)
        }
        localStorage.setItem('orderCheckout', JSON.stringify(order));
        navigate('/checkout/confirm')
    }

    if (cart.length <= 0) navigate("/")
    if (loading) return <p>Loading...</p>
    return (
        <div className="form-container">
            <p className="text-[#F4F0E5] text-2xl font-semibold">Order confirmation</p>
            <form onSubmit={handleSubmit(onSubmit)}>
                <>
                    <input type="hidden" {...register("userId")} />
                    <p>{errors.userId?.message}</p>
                    <input type="hidden" {...register("orderDate")} />
                    <p>{errors.orderDate?.message}</p>
                    <input type="hidden" {...register("deliveryDate")} />
                    <p>{errors.deliveryDate?.message}</p>
                    <input type="hidden" {...register("price")} />
                    <p>{errors.price?.message}</p>
                    <input type="hidden" {...register("deliveryPrice")} />
                    <p>{errors.deliveryPrice?.message}</p>
                    <input type="hidden"{...register("status")} />
                    <p>{errors.status?.message}</p>
                </>
                <div className="flex gap-[16px]">
                    <div className="checkout-form">
                        <div className="checkout-section">
                            <h1 className="font-semibold text-xl">Contact details</h1>
                            <div className="checkout-content">
                                <div className="content-row">
                                    <div className="input-container">
                                        <p>Name</p>
                                        <p className="checkout-input">{user?.firstName}</p>
                                    </div>
                                    <div className="input-container">
                                        <p>Surname</p>
                                        <p className="checkout-input">{user?.lastName}</p>
                                    </div>
                                </div>
                                <div className="content-row">
                                    <div className="input-container">
                                        <p>Email</p>
                                        <p className="checkout-input">{user?.email}</p>
                                    </div>
                                    <div className="input-container">
                                        <p>Phone number</p>
                                        <p className="checkout-input">+{user?.phoneNumber}</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div className="checkout-section">
                            <h1 className="font-semibold text-xl">Delivery</h1>
                            <div className="checkout-content">
                                <div className="content-row">
                                    <div className="input-container">
                                        <p>Region</p>
                                        <input
                                            className="checkout-input"
                                            {...register("region")} />
                                        <p>{errors.region?.message}</p>
                                    </div>
                                    <div className="input-container">
                                        <p>City</p>
                                        <input
                                            className="checkout-input"
                                            {...register("city")} />
                                        <p>{errors.city?.message}</p>
                                    </div>
                                    <div className="input-container">
                                        <p>Address</p>
                                        <input
                                            className="checkout-input"
                                            {...register("address")} />
                                        <p>{errors.address?.message}</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div className="checkout-section">
                            <h1 className="font-semibold text-xl">Delivery method</h1>
                            {deliveryTypes?.map((delivery) => (
                                <label htmlFor={delivery.id}>
                                    <div className={`radio-group ${deliveryTypeId === delivery.id ? "radio-selected" : ""}`}>
                                        <div className="radio-container">
                                            <input
                                                type="radio" id={delivery.id}
                                                value={delivery.id}
                                                {...register("deliveryTypeId")} />
                                        </div>
                                        {delivery.serviceName}
                                    </div>
                                </label>
                            ))}
                            <p className="">{errors.deliveryTypeId?.message}</p>
                        </div>
                        <div className="checkout-section">
                            <div className="checkout-content">
                                <div className="content-row">
                                    <div className="input-container">
                                        <p>Branch number</p>
                                        <p className="checkout-input">BRANCH_TMP</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div className="checkout-section">
                            <h1 className="font-semibold text-xl">Payment method</h1>
                            {/* THIS PAYMENTMETHOD IMPLEMENTATION IS TEMPORARY, FOR STYLES TO FUNCTION */}
                            <label htmlFor="card" onClick={() => setPaymentMethod("card")}>
                                <div className={`radio-group ${paymentMethod === "card" ? "radio-selected" : ""}`}>
                                    <div className="radio-container">
                                        <input type="radio" id="card" name="payment_method" value="CARD" />
                                    </div>
                                    <img src={icons.wCredit} className="invert" />
                                    By card
                                </div>
                            </label>
                            <label htmlFor="cash" onClick={() => setPaymentMethod("cash")}>
                                <div className={`radio-group ${paymentMethod === "cash" ? "radio-selected" : ""}`}>
                                    <div className="radio-container">
                                        <input type="radio" id="cash" name="payment_method" value="CASH" />
                                    </div>
                                    <img src={icons.bMoney} />
                                    Cash on delivery
                                </div>
                            </label>
                        </div>
                        <div className="checkout-section">
                            <h1 className="font-semibold text-xl">Leave a message</h1>
                            <textarea className="checkout-comment" rows={5} />
                        </div>
                    </div>

                    {/* ===== SIDE CHECKOUT PANEL =====  */}

                    <div className="checkout-side">
                        <div className="cart-section">
                            <div className="flex flex-col gap-[23px]">
                                <div className="flex justify-between">
                                    <h1 className="font-semibold text-xl">{cart.length} items in cart</h1>
                                    <div className={`edit-btn ${edit ? "active" : ""}`} onClick={toggleEdit}>
                                        <img src={icons.bPen} className={`transition-all duration-500 ${edit ? "invert" : ""}`} />
                                    </div>
                                </div>
                                {cart?.map((item) => (
                                    <CartCheckoutCard item={item}
                                        onAdd={onAdd}
                                        onItemClear={onItemClear}
                                        onRemove={onRemove}
                                        edit={edit} />
                                ))}
                            </div>
                            <p className="text-[#FF642E] font-semibold">Total {price.toFixed(2)} UAH</p>
                        </div>
                        <div className="checkout-section">
                            <div className="flex flex-col gap-2.5">
                                <div className="flex flex-col gap-[19px]">
                                    <div className="flex justify-between">
                                        <p>To be paid</p>
                                        <p>{price.toFixed(2)} UAH</p>
                                    </div>
                                    {cart?.map((item) => (
                                        <CheckoutListing item={item} />
                                    ))}
                                    <div className="flex justify-between text-gray">
                                        <p>Delivery</p>
                                        <p>0 UAH</p>
                                    </div>
                                    <hr />
                                </div>
                                <p>
                                    By submitting an order, I confirm
                                    that I have read and agree to the <a className="text-blue-900">Terms of Use</a>
                                </p>
                                <hr />
                                <div className="flex flex-col gap-[22px]">
                                    <div className="flex gap-2.5">
                                        <input type="checkbox" id="call_check" />
                                        <label htmlFor="call_check">I have a question, call me back.</label>
                                    </div>
                                    <div className="flex flex-col gap-6">
                                        <button className="confirm-btn">Submit order</button>
                                    </div>
                                    <a href="/catalog" className="text-lg font-semibold text-[#FF642E] text-center">Continue shopping</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </form>
        </div>
    )
}

export default OrderCheckoutForm