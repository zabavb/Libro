import { Order, User } from "@/types";
import "@/assets/styles/components/order/checkout-confirmation.css"
import { icons } from "@/lib/icons"
import { CartItem } from "@/types/types/cart/CartItem";
import CheckoutBookCard from "../common/CheckoutBookCard";
import { useNavigate } from "react-router-dom";
import { useEffect } from "react";

interface OrderConfirmationProps {
    order: Order | undefined
    user: User | undefined
    cart: CartItem[]
    onConfirm: (order: Order) => void;
    total: number
}


const OrderConfirmation: React.FC<OrderConfirmationProps> = ({ order, user, onConfirm, cart, total }) => {
    const navigate = useNavigate();

    useEffect(() => {
        if (!order || !user) navigate('/', {state:{ authOpen: true} })
    }, [])
    return (
        <div className="form-container">
            <p className="text-[#F4F0E5] text-2xl font-semibold">Placing an order</p>
            <div className="checkout-section">
                <div className="flex justify-between">
                    <div className="flex flex-col gap-3">
                        {cart.map((item) => (
                            <CheckoutBookCard item={item} />
                        ))}
                    </div>
                    <div className="price-container">
                        <h1 className="text-3xl">To be paid:</h1>
                        <h1 className="text-5xl">{total.toFixed(2)} UAH</h1>
                    </div>
                </div>
                <div className="flex flex-col justify-between gap-16">
                    <div className="flex gap-32">
                        <div className="display-col">
                            <h1 className="text-xl font-semibold">Contacts</h1>
                            <div className="flex gap-11">
                                <div className="display-row">
                                    <p>Name</p>
                                    <div className="input-display">
                                        <p>{user?.firstName}</p>
                                    </div>
                                </div>
                                <div className="display-row">
                                    <p className="input-label">Surname</p>
                                    <p className="input-display">{user?.lastName}</p>
                                </div>
                            </div>
                            <div className="flex gap-11">
                                <div className="display-row">
                                    <p>Email</p>
                                    <div className="input-display">
                                        <p>{user?.email}</p>
                                    </div>
                                </div>
                                <div className="display-row">
                                    <p className="input-label">Phone number</p>
                                    <p className="input-display">+{user?.phoneNumber}</p>
                                </div>
                            </div>
                        </div>
                        <div className="display-col">
                            <h1 className="text-xl font-semibold">Delivery</h1>
                            <div className="flex gap-11">
                                <div className="display-row">
                                    <p>Region</p>
                                    <div className="input-display">
                                        <p>{order?.region}</p>
                                    </div>
                                </div>
                                <div className="display-row">
                                    <p className="input-label">City</p>
                                    <p className="input-display">{order?.city}</p>
                                </div>
                            </div>
                            <div className="flex gap-11">
                                <div className="display-row">
                                    <p>Address</p>
                                    <div className="input-display">
                                        <p>{order?.address}</p>
                                    </div>
                                </div>
                                <div className="display-row">
                                    <p className="input-label">Branch</p>
                                    <p className="input-display">BRANCH_TMP</p>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="flex gap-32">
                        <div className="display-col">
                            <h1 className="text-xl font-semibold">Payment Method</h1>
                            <div className="flex gap-11">
                                <div className="flex flex-col gap-2 w-full">
                                    <div className="input-display">
                                        <img className="w-7 h-7" src={icons.bMoney} />
                                        <p>PAYMENT_TMP</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div className="display-col">
                            <h1 className="text-xl font-semibold">Message</h1>
                            <div className="flex gap-11">
                                <div className="flex flex-col gap-2 w-full">
                                    <div className="input-display">
                                        <p>-</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="flex flex-col gap-[22px]">
                    <div className="flex justify-center gap-5">
                        <input type="checkbox" id="call_check" />
                        <label htmlFor="call_check" className="font-semibold">I have a question, call me back.</label>
                    </div>
                    <div className="flex justify-center">
                        {order ? (
                            <button className="confirm-btn" onClick={() => onConfirm(order)}>
                                Confirm Order
                            </button>
                        ) : ""
                        }

                    </div>
                    <div className="flex justify-center">
                        <a className="text-[#FF642E] text-lg font-semibold"
                            href="/checkout">Cancel</a>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default OrderConfirmation