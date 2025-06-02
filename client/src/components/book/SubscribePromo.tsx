import React from "react";
import emailIcon from '@/assets/icons/emailIcom.svg';
import "@/assets/styles/components/book/subscribe-promo.css"; 

const SubscribePromo: React.FC = () => {
  return (
    <div className="subscribe-promo-root">
      <div>
        <div className="subscribe-promo-title">
          <span className="subscribe-promo-discount">
            -15%
          </span>
          off your first purchase<br />for newsletter signup
        </div>
        <p className="subscribe-promo-desc">
          Join our community to get updates on the latest promotions and products.
        </p>
      </div>

      <div className="subscribe-promo-form">
        <div className="subscribe-promo-icon-wrap">
          <img src={emailIcon} alt="Email icon" className="subscribe-promo-icon" />
        </div>
        <input
          type="email"
          placeholder="Enter your email address"
          className="subscribe-promo-input"
        />
        <button className="subscribe-promo-btn">
          Subscribe
        </button>
      </div>
    </div>
  );
};

export default SubscribePromo;