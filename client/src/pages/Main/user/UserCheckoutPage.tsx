
import Footer from '@/components/layouts/Footer';
import Header from '@/components/layouts/Header';
import UserCheckoutFormContainer from '@/containers/user/UserCheckoutFormContainer';

const UserCheckoutPage = () => {
    return (
        <div className="flex flex-col min-h-screen">
            <Header />
            <main className="flex-1 overflow-auto">
              <UserCheckoutFormContainer />
            </main>
            <Footer />
        </div>
        // <div>
        //     <main>
        //         {cart.map((item) => (
        //             <li key={item.bookId}>
        //                 {item.name} x ({item.amount})
        //             </li>
        //         ))}
        //         <UserCheckoutFormContainer />
        //     </main>
        // </div>
    );
};

export default UserCheckoutPage;
