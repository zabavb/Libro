import { useNavigate } from 'react-router-dom';
import doodleUrl from "@/assets/accessDenied.svg"
import "@/assets/styles/layout/access-denied.css"
const ForbiddenPage: React.FC = () => {
  const navigate = useNavigate();

  return (
    <div className='denied-page-container'>
      <div className='denied-container'>
        <h1 className='denied'>Access<br/>Denied
        <img className='denied-image' src={doodleUrl}/>
        </h1>
      <button className='denied-nav-btn' onClick={() => navigate('/')}>Return to the main page</button>
      </div>
    </div>
  );
};

export default ForbiddenPage;
