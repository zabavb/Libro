import Footer from "@/components/layouts/Footer";
import Header from "@/components/layouts/Header";
import {icons} from '@/lib/icons'
import "@/assets/styles/components/order/order-success.css"
import { Navigate, useLocation } from "react-router-dom";
const OrderSuccessPage = () => {
    const location = useLocation();

    const cameFromRedirect = location.state?.fromRedirect;
    console.log(location.state?.fromRedirect)

    if (!cameFromRedirect) {
        console.log("REDIRECT")
      return <Navigate to="/" replace />;
    }
    return (
        <div className="flex flex-col min-h-screen">
            <Header />
            <main className="flex-1 grid grid-rows-[1fr_1fr_1fr] pt-14">
                <div>
                    <p className="order-success">
                        Order has been successfully placed.
                    </p>
                </div>
                <div className="flex items-end justify-center">
                    <p className="text-center">
                        Wait for delivery, if necessary we will contact you<br />
                        Payment method: Cash or card: Upon receipt
                    </p>
                </div>
                <div className="p-16">
                    <div className="bonus-club">
                        <h1 className="text-[44px] font-bold">Libro Bonus Club</h1>
                        <p>Get bonuses for every purchase and use them for discounts on your favorite books!</p>
                        <a href="#" className="more-info">
                                More details
                                <img src={icons.bArrowRight}/>
                        </a>
                    </div>
                </div>
            </main>
            <Footer />
        </div>

    );
};

export default OrderSuccessPage