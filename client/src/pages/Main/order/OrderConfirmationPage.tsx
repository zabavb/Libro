import Footer from "@/components/layouts/Footer";
import Header from "@/components/layouts/Header";
import OrderConfirmationContainer from "@/containers/order/OrderConfirmationContainer";
import { useLocation } from "react-router-dom";

const OrderConfirmationPage = () => {
    const { state } = useLocation();
    const { order, user } = state || {};
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