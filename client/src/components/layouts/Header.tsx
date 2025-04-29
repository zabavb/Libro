import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faCartShopping,
  faHeart,
  faUser,
  faMagnifyingGlass,
} from "@fortawesome/free-solid-svg-icons";
import { useNavigate } from "react-router-dom"; 
import CatalogMenu from "./CatalogMenu";

export default function Header() {
  const navigate = useNavigate(); 

  return (
    <header className='header'>
      {/* Header__left */}
      <div className='header__left'>
        <div className='header__logo'>LIBRO</div>
        <div className='header__catalog'>
          <CatalogMenu />
        </div>
      </div>

      {/* Header__search */}
      <div className='header__search'>
        <input
          type='text'
          placeholder='Search on website'
          className='header__input'
        />
        <FontAwesomeIcon icon={faMagnifyingGlass} className='header__icon' />
      </div>

      {/* Header__right */}
      <div className='header__right'>
        <FontAwesomeIcon
          icon={faCartShopping}
          className='header__action-icon cursor-pointer' // Додаємо cursor-pointer для кращого UX
          onClick={() => navigate('/cart')} // Навігація до /cart
        />
        <FontAwesomeIcon icon={faHeart} className='header__action-icon' />
        <FontAwesomeIcon
          icon={faUser}
          className='header__action-icon cursor-pointer'
          onClick={() => navigate('/profile')}
        />
        <FontAwesomeIcon
          icon={faUser}
          className='header__action-icon cursor-pointer' // Додаємо cursor-pointer
          onClick={() => navigate('/admin')} // Навігація до /admin
        />
        <div className='header__lang'>
          <span className='header__lang--active'>UA</span>
          <span>ENG</span>
        </div>
      </div>
    </header>
  );
}