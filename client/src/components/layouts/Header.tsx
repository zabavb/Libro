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
  const [isCatalogOpen, setIsCatalogOpen] = useState<boolean>(false);

  const toggleAuth = () => setIsAuthOpen(!isAuthOpen);
  const isActive = (path: string) => currentPath === path;

  return (
    <header className='flex flex-col gap-2.5'>
      {/* Top Bar */}
      {isActive("/") && (      
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
      )}

      {/* Panels */}
      <AuthPanelContainer isOpen={isAuthOpen} setIsAuthOpen={setIsAuthOpen} />
      <CatalogMenu isOpen={isCatalogOpen} setIsOpen={setIsCatalogOpen} />

      {/* Main Header */}
      <div className="flex gap-[62px] px-[40px] py-6 items-center">
        {/* Logo */}
        <div>
          <img
            src={libroLogo}
            className="invert w-[108px] h-[42px] cursor-pointer"
            onClick={() => navigate('/')}
          />
        </div>

        {/* Middle Section */}
        <div className="flex gap-[54px] items-center flex-1">
          {/* Catalog Button */}
          <div className='header__catalog'>
            <button
              onClick={() => setIsCatalogOpen(true)}
              className="flex items-center gap-2 text-lg font-medium"
            >
              <img src={icons.bMenu} alt="Menu" className="w-6 h-6" />
              Каталог
            </button>
          </div>

          {/* Search */}
          <div className='header__search'>
            <input
              type='text'
              placeholder='Search on website'
              className='header__input'
            />
            <img src={icons.bMagnifyingGlass} className='header__icon' />
          </div>

          {/* Nav */}
          <div className="header__nav">
            <CartPanel />
            <img
              src={icons.bHeart}
              className="cursor-pointer"
              onClick={() => navigate("/liked")}
            />
            <UserPanel onLoginOpen={toggleAuth} />
          </div>

          {/* Language Switch */}
          <div className='header__lang'>
            <span>UA</span>
            <span className='header__lang--active'>ENG</span>
          </div>
        </div>
      </div>
    </header>
  );
}
