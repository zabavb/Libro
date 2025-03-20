import { useNavigate } from "react-router-dom";
import "../../index.css"

const MainPage: React.FC = () => {
  const navigate = useNavigate();

  return (
    <div>
      <h1>Main page</h1>
      <div>
        <button onClick={() => navigate("/admin")}>Admin dashboard</button>
      </div>
      <div>
        <button onClick={() => navigate("/basket")}>Basket</button>
      </div>
    </div>
  );
}

export default MainPage