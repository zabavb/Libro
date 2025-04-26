import { useNavigate } from 'react-router-dom';
import doodleUrl from "@/assets/notFound.svg"
import "@/assets/styles/layout/not-found.css"
const NotFoundPage = () => {
  const navigate = useNavigate();

  return (
    <div className='error-page-container'>
      <div className='error-container'>
        <h1 className='error'>
          <img className='error-image' src={doodleUrl}/>
          404
        </h1>
      </div>
      <p className='error-sub'>Sorry, the page was not found.</p>
      <div>
        <button className='error-nav-btn' onClick={() => navigate('/')}>Return to the main page</button>
      </div>
    </div>
  );
};

export default NotFoundPage;
