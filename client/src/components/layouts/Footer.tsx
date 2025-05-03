import React from "react";
import '@/assets/styles/layout/_footer.css'
import libroLogo from '@/assets/logoLibro.svg'
import { icons } from '@/lib/icons'
import visaUrl from '@/assets/Visa.svg'
import masterCardUrl from '@/assets/MasterCard.svg'
const Footer: React.FC = () => {
  return (
    <footer className="footer-container">
      <div className="footer-content">
        <div className="footer-socials">
          <img src={libroLogo} className="footer-logo" />
          <div className="flex gap-5">
            <img src={icons.telegram} />
            <img src={icons.instagram} />
            <img src={icons.facebook} />
          </div>
        </div>
        <div className="footer-info">
          <p className="font-bold">0-800-385-4**</p>
          <p className="font-bold">Email: libro.support@gmail.com</p>
          <p className="opacity-50">Mn-St 09:00-21:00</p>
        </div>
        <div className="footer-info">
          <p className="font-bold">For customers</p>
          <a href="#"  className="opacity-50">Delivery & payment</a>
          <a href="#"  className="opacity-50">Product return</a>
          <a href="#"  className="opacity-50">Loyalty program</a>
          <a href="#"  className="opacity-50">Contacts</a>
        </div>
        <div className="footer-info">
          <p className="font-bold">Company</p>
          <a href="#"  className="opacity-50">About us</a>
          <a href="#"  className="opacity-50">Franchise</a>
          <a href="#"  className="opacity-50">Partnership</a>
          <a href="#"  className="opacity-50">Job offerings</a>
        </div>
        <div className="flex flex-col gap-[32px]">
        <img
            src={masterCardUrl}
            alt="Mastercard"
            className="payment-icon"
          />
          <img
            src={visaUrl}
            alt="Visa"
            className="payment-icon"
          />
        </div>
      </div>
      <p className="copyright">© Libro 2024-2025. All rights reserved.</p>
    </footer>
  );
};

export default Footer;

{/* <div className="max-w-7xl mx-auto grid grid-cols-1 md:grid-cols-5 gap-8">
        <div>
          <img src={libroLogo} className="invert w-[190px] h-[75px]"/>
          <div className="flex space-x-4">
            <a href="#" className="text-gray-600 hover:text-gray-800">
            </a>
            <a href="#" className="text-gray-600 hover:text-gray-800">
            </a>
            <a href="#" className="text-gray-600 hover:text-gray-800">
            </a>
          </div>
        </div>

        <div>
          <p className="text-sm">0-800-385-4**</p>
          <p className="text-sm">Email: libro.support@gmail.com</p>
          <p className="text-sm mt-2">Mon–Sat 09:00–21:00</p>
        </div>

        <div>
          <h3 className="text-sm font-semibold text-gray-900 mb-2">Shopping & Payment</h3>
          <ul className="text-sm space-y-1">
            <li><a href="#" className="hover:text-gray-900">Returns</a></li>
            <li><a href="#" className="hover:text-gray-900">Loyalty Program</a></li>
            <li><a href="#" className="hover:text-gray-900">Contacts</a></li>
          </ul>
        </div>

        <div>
          <h3 className="text-sm font-semibold text-gray-900 mb-2">Company</h3>
          <ul className="text-sm space-y-1">
            <li><a href="#" className="hover:text-gray-900">About Us</a></li>
            <li><a href="#" className="hover:text-gray-900">Official</a></li>
            <li><a href="#" className="hover:text-gray-900">Service</a></li>
            <li><a href="#" className="hover:text-gray-900">Careers</a></li>
          </ul>
        </div>

        <div>
          <div className="flex space-x-2">
            <img
              src="https://upload.wikimedia.org/wikipedia/commons/2/2a/Mastercard-logo.svg"
              alt="Mastercard"
              className="h-6"
            />
            <img
              src="https://upload.wikimedia.org/wikipedia/commons/5/5e/Visa_Inc._logo.svg"
              alt="Visa"
              className="h-6"
            />
          </div>
        </div>
      </div>

      <div className="text-center text-sm text-gray-500 mt-6">
        © LIBRO 2024-2025. All rights reserved.
      </div> 
      */}