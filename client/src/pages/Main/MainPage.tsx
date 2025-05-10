import { useNavigate } from "react-router-dom";
import "../../index.css";
import Header from "@/components/layouts/Header";
import Footer from "@/components/layouts/Footer";
import { CarouselOffers } from "@/components/common/CarouselOffers";
import { useAuth } from "../../state/context";

const MainPage: React.FC = () => {
  const navigate = useNavigate();
  const { logout } = useAuth();

  return (
    <div>
      <h1>Main page</h1>
      <div>
        <button onClick={() => navigate("/admin")}>Admin dashboard</button>
      </div>
      <div>
        <button onClick={() => navigate("/cart")}>Cart</button>
      </div>
      <div>
        <button onClick={() => navigate('/catalog')}>Catalog</button>
      </div>
      <div>
        <button onClick={() => navigate('/liked')}>Liked Books</button>
      </div>

    </div>
  );
};

export default MainPage;
