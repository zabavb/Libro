import React from "react";
import { useNavigate } from "react-router-dom";
import { icons } from '@/lib/icons';
import closeIcon from '@/assets/icons/menuClose.svg';

interface AddToCartToastProps {
  itemCount: number;
  totalPrice: number;
  onClose: () => void;
}

const AddToCartToast: React.FC<AddToCartToastProps> = ({
  itemCount,
  totalPrice,
  onClose,
}) => {
  const navigate = useNavigate();

  return (
    <div style={{
      position: "fixed",
      top: "20px",
      right: "20px",
      width: "363px",
      height: "146px",
      backgroundColor: "#F4F0E5",
      border: "1px solid #1A1D23",
      borderRadius: "20px",
      padding: "20px",
      boxShadow: "0 2px 10px rgba(0,0,0,0.1)",
      zIndex: 1000
    }}>
      <div style={{ display: "flex", justifyContent: "space-between" }}>
        <div style={{ display: "flex", gap: "10px" }}>
          <div style={{
            backgroundColor: "#FF642E",
            width: "33px",
            height: "33px",
            borderRadius: "30px",
            display: "flex",
            alignItems: "center",
            justifyContent: "center"
          }}>
            <img src={icons.cartN} alt="Cart" style={{ width: 20, height: 20 }} />
          </div>
          <div>
            <div style={{
              fontWeight: 600,
              fontSize: "14px",
              color: "#1A1D23"
            }}>
              Item added to cart
            </div>
            <div style={{
              fontSize: "14px",
              color: "#929089",
              marginTop: "-2px"
            }}>
              You have {itemCount} item{itemCount === 1 ? '' : 's'} in your cart
              <br />
                Total amount: {totalPrice.toFixed(2)} UAH
            </div>
          </div>
        </div>
        <button onClick={onClose} style={{ background: "none", border: "none", padding: 0 }}>
          <img src={closeIcon} alt="Close" style={{ width: 24, height: 24 }} />
        </button>
      </div>
      <div
        onClick={() => {
          navigate("/checkout");
          onClose();
        }}
        style={{
          marginTop: "20px",
          backgroundColor: "#FF642E",
          borderRadius: "46px",
          height: "40px",
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          cursor: "pointer"
        }}
      >
        <span style={{
          color: "white",
          fontWeight: 600,
          fontSize: "14px"
        }}>
          Proceed to checkout
        </span>
      </div>
    </div>
  );
};

export default AddToCartToast;
