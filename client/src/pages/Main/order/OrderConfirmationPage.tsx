import Footer from "@/components/layouts/Footer";
import Header from "@/components/layouts/Header";
import OrderConfirmationContainer from "@/containers/order/OrderConfirmationContainer";
import { Order, User } from "@/types";

const OrderConfirmationPage = () => {
    const order: Order = JSON.parse(localStorage.getItem('orderCheckout') || '{}');
    const user: User = JSON.parse(localStorage.getItem('user') || '{}');
    return (
        <div className="flex flex-col min-h-screen">
            <Header />
            <main className="flex-1 overflow-auto">
                <OrderConfirmationContainer order={order} user={user}/>
            </main>
            <Footer />
        </div>
    );
};

export default OrderConfirmationPage