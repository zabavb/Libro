import React from "react";
import RightArrow from '@/assets/icons/rightArrow.svg';
import "@/assets/styles/components/book/libro-bonus-club.css";

const LibroBonusClub: React.FC = () => {
  return (
    <div className="libro-bonus-root">
      <div>
        <h3 className="libro-bonus-title">
          Libro Bonus Club
        </h3>
        <p className="libro-bonus-desc">
          Earn bonuses for every purchase and use them for discounts on your favorite books!
        </p>
      </div>
      <div className="libro-bonus-btn-wrap">
        <button className="libro-bonus-btn">
          Learn more
          <img src={RightArrow} alt="Arrow right" className="libro-bonus-arrow" />
        </button>
      </div>
    </div>
  );
};

export default LibroBonusClub;