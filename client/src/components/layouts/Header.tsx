import { useLocation, useNavigate } from "react-router-dom";
import CatalogMenu from "./CatalogMenu";
import CartPanel from "./CartPanel";
import libroLogo from '@/assets/logoLibro.svg'
import UserPanel from "./UserPanel";
import { icons } from '@/lib/icons'
import { useState } from "react";
import AuthPanelContainer from "@/containers/auth/AuthPanelContainer";
import '@/assets/styles/layout/_header.css'


export default function Header() {
  const navigate = useNavigate();

  const location = useLocation();
  const currentPath = location.pathname;

  const [isAuthOpen, setIsAuthOpen] = useState<boolean>(false);
  const toggleAuth = () => setIsAuthOpen(!isAuthOpen);
  const isActive = (path: string) => currentPath === path;

  return (
    <header className='flex flex-col gap-2.5'>
      {isActive("/") ? (      
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
      </div>)
      : ""
      }
      <AuthPanelContainer isOpen={isAuthOpen} setIsAuthOpen={setIsAuthOpen}/>
      <div className="flex gap-[62px] px-[40px] py-6 items-center">
        <div>
          <img src={libroLogo} className="invert w-[108px] h-[42px] cursor-pointer" onClick={() => navigate('/')}/>
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
            <img src={icons.bMagnifyingGlass} className='header__icon' />
          </div>
          <div className="header__nav">
            <CartPanel />
            <img src={icons.bHeart} />
            <UserPanel onLoginOpen={toggleAuth}/>
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