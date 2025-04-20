import React from "react";

const Footer: React.FC = () => {
  return (
    <footer className="bg-[rgb(244,240,229)] p-6 text-gray-700">
      <div className="max-w-7xl mx-auto grid grid-cols-1 md:grid-cols-5 gap-8">
        {/* Left Section: Logo and Social Media */}
        <div>
          <h2 className="text-xl font-bold text-gray-900 mb-4">LIBRO</h2>
          <div className="flex space-x-4">
            <a href="#" className="text-gray-600 hover:text-gray-800">
              {/* <PhoneIcon className="w-5 h-5" /> */}
            </a>
            <a href="#" className="text-gray-600 hover:text-gray-800">
              {/* <InstagramLogoIcon className="w-5 h-5" /> */}
            </a>
            <a href="#" className="text-gray-600 hover:text-gray-800">
              {/* <FacebookLogoIcon className="w-5 h-5" /> */}
            </a>
          </div>
        </div>

        {/* Contact Information */}
        <div>
          <p className="text-sm">0-800-385-4**</p>
          <p className="text-sm">Email: libro.support@gmail.com</p>
          <p className="text-sm mt-2">Mon–Sat 09:00–21:00</p>
        </div>

        {/* Shopping & Payment Section */}
        <div>
          <h3 className="text-sm font-semibold text-gray-900 mb-2">Shopping & Payment</h3>
          <ul className="text-sm space-y-1">
            <li><a href="#" className="hover:text-gray-900">Returns</a></li>
            <li><a href="#" className="hover:text-gray-900">Loyalty Program</a></li>
            <li><a href="#" className="hover:text-gray-900">Contacts</a></li>
          </ul>
        </div>

        {/* Company Section (Moved to a separate column) */}
        <div>
          <h3 className="text-sm font-semibold text-gray-900 mb-2">Company</h3>
          <ul className="text-sm space-y-1">
            <li><a href="#" className="hover:text-gray-900">About Us</a></li>
            <li><a href="#" className="hover:text-gray-900">Official</a></li>
            <li><a href="#" className="hover:text-gray-900">Service</a></li>
            <li><a href="#" className="hover:text-gray-900">Careers</a></li>
          </ul>
        </div>

        {/* Payment Methods */}
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

      {/* Copyright Notice */}
      <div className="text-center text-sm text-gray-500 mt-6">
        © LIBRO 2024-2025. All rights reserved.
      </div>
    </footer>
  );
};

export default Footer;