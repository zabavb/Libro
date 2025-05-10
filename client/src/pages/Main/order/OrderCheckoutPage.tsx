
import Footer from '@/components/layouts/Footer';
import Header from '@/components/layouts/Header';
import OrderCheckoutFormContainer from '@/containers/order/OrderCheckoutFormContainer';

const OrderCheckoutPage = () => {
    return (
        <div className="flex flex-col min-h-screen">
            <Header />
            <main className="flex-1 overflow-auto">
              <OrderCheckoutFormContainer />
            </main>
            <Footer />
        </div>
    );
};

export default OrderCheckoutPage;
