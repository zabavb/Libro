import { useNavigate } from "react-router-dom"; 
import { useState } from "react";

import CatalogMenu from "./CatalogMenu";

import userIcon from '@/assets/icons/headerUser.svg'
import heartIcon from '@/assets/icons/headerHeart.svg'
import magnifyingGlass from '@/assets/icons/magnifyingGlass.svg'
import CartPanel from "./CartPanel";

export default function Header() {
  const navigate = useNavigate(); 

  const [isProfileOpen, setIsProfileOpen] = useState<boolean>(false);

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
        <img src={magnifyingGlass} className='header__icon'/>
      </div>

      {/* Header__right */}
      <div className='header__right'>
        <CartPanel/>
        <img src={heartIcon}/>
        <img src={userIcon}
                  className='header__action-icon cursor-pointer'
                  onClick={() => navigate('/profile')}/>
        <div className='header__lang'>
          <span className='header__lang--active'>UA</span>
          <span>ENG</span>
        </div>
      </div>
    </header>
  );
}