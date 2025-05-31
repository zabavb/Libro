import { useLocation, useNavigate } from "react-router-dom";
import CatalogMenu from "./CatalogMenu";
import CartPanel from "./CartPanel";
import libroLogo from '@/assets/logoLibro.svg'
import UserPanel from "./UserPanel";
import { icons } from '@/lib/icons'
import { useState } from "react";
import AuthPanelContainer from "@/containers/auth/AuthPanelContainer";

import '@/assets/styles/layout/_header.css'
import React from "react";

interface LocationState{
  authOpen?: boolean
}

export default function Header() {
  const navigate = useNavigate();
  const location = useLocation();
  const currentPath = location.pathname;
  const state = location.state !== null ? location.state as LocationState : {authOpen: false};
  const [isAuthOpen, setIsAuthOpen] = useState<boolean>(state.authOpen ?? false);
  const [isCatalogOpen, setIsCatalogOpen] = useState<boolean>(false);
  const [searchValue, setSearchValue] = useState('');

  const proposals = [
    { label: "Discounts", path: "/catalog?type=discounts" },
    { label: "Paper Books", path: "/catalog?type=paper" },
    { label: "Digital Books", path: "/catalog?type=digital" },
    { label: "Audio Books", path: "/catalog?type=audio" },
    { label: "Other", path: "/catalog?type=other" },
  ];

   const getActiveProposal = () => {
    const params = new URLSearchParams(location.search);
    const type = params.get("type");
    if (currentPath !== "/catalog") return "";
    return (
      proposals.find((p) => p.path.includes(`type=${type}`))?.label || ""
    );
  };
  const activeProposal = getActiveProposal();
  const toggleAuth = () => setIsAuthOpen(!isAuthOpen);
  const isActive = (path: string) => currentPath === path;

  const handleSearch = () => {
    if (searchValue.trim()) {
      navigate(`/catalog?search=${encodeURIComponent(searchValue.trim())}`);
    }
  };

  return (
    <header className='flex flex-col gap-2.5'>
      {/* Top Bar */}
      {isActive("/") && (
        <div className="header__proposals">
          {proposals.map((item, idx) => (
            <React.Fragment key={item.label}>
              <p
                className={`header__proposal-btn${activeProposal === item.label ? " header__proposal-btn--active" : ""}`}
                onClick={() => navigate(item.path)}
                style={{ cursor: "pointer" }}
              >
                {item.label}
              </p>
              {idx < proposals.length - 1 && <div className="line" />}
            </React.Fragment>
          ))}
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
              Book Catalog
            </button>
          </div>

          {/* Search */}
          <div className='header__search'>
            <input
              type='text'
              placeholder='Search on website'
              className='header__input'
              value={searchValue}
              onChange={(e) => setSearchValue(e.target.value)}
              onKeyDown={(e) => {
                if (e.key === 'Enter') handleSearch();
              }}
            />
            <img
              src={icons.bMagnifyingGlass}
              className='header__icon cursor-pointer'
              onClick={handleSearch}
            />
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
