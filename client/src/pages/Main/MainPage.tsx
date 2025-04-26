import { useNavigate } from "react-router-dom";
import "../../index.css";
import Header from "@/components/layouts/Header";
import Footer from "@/components/layouts/Footer";
import { CarouselOffers } from "@/components/common/CarouselOffers";

const MainPage: React.FC = () => {
  const navigate = useNavigate();
  const { logout } = useAuth();

  return (
    <div className="flex flex-col min-h-screen">
      <Header />
      <main className="flex-1">
        <div className="px-10 py-10">
          <CarouselOffers />
        </div>

        <div className="max-w-5xl mx-auto px-4 py-6 space-y-6">
         
        </div>
      </main>
      <Footer />
    </div>
  );
};

export default MainPage;
