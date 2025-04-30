import { useNavigate } from "react-router-dom";
import CatalogMenu from "./CatalogMenu";
import userIcon from '@/assets/icons/headerUser.svg'
import heartIcon from '@/assets/icons/headerHeart.svg'
import magnifyingGlass from '@/assets/icons/magnifyingGlass.svg'
import CartPanel from "./CartPanel";
import libroLogo from '@/assets/logoLibro.svg'
export default function Header() {
  const navigate = useNavigate();

  return (
    <header className='flex flex-col gap-2.5'>
      <div className="header__proposals">
          <p>Discounts</p>
          <div className="line"/>
          <p>Paper Books</p>
          <div className="line"/>
          <p>Digital Books</p>
          <div className="line"/>
          <p>Audio Books</p>
          <div className="line"/>
          <p>Other</p>
      </div>
      <div className="flex gap-[62px] px-[40px] py-6 items-center">
        <div>
          <img src={libroLogo} className="invert w-[108px] h-[42px]" />
        </div>
        <div className="flex gap-[54px] items-center flex-1">
          <div className='header__catalog'>
            <CatalogMenu />
          </div>
          <div className='header__search'>
            <input
              type='text'
              placeholder='Search on website'
              className='header__input'
            />
            <img src={magnifyingGlass} className='header__icon' />
          </div>
          <div className="header__nav">
            <CartPanel />
            <img src={heartIcon} />
            <img src={userIcon}
              className='cursor-pointer'
              onClick={() => navigate('/profile')} />
          </div>
          <div className='header__lang'>
            <span>UA</span>
            <span className='header__lang--active'>ENG</span>
          </div>
        </div>
      </div>
    </header>
  );
}