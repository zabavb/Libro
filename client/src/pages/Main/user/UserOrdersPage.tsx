import Footer from '@/components/layouts/Footer';
import Header from '@/components/layouts/Header';
import UserOrdersContainer from '@/containers/user/UserOrdersListContainer';

const UserOrdersPage = () => {
  return (
    <div className="flex flex-col min-h-screen">
      <Header />
      <main className="flex-1 overflow-auto">
        <UserOrdersContainer />
      </main>
      <Footer />
    </div>
  );
};

export default UserOrdersPage;
