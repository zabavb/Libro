import { useNavigate } from "react-router-dom";
import "../../index.css"

const MainPage: React.FC = () => {
  const navigate = useNavigate();

  return (
    <>
      <h1>Main page</h1>
      <button onClick={() => navigate("/admin")}>Admin dushboard</button>
    </>
  );
}

export default MainPage