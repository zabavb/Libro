import React from "react";
import CatalogMenu from "./CatalogMenu";

const Header: React.FC = () => {
  return (
    <header className="bg-gray-900 text-white p-4 flex items-center space-x-6">
      <div className="flex items-center space-x-4">
        <h1 className="text-xl font-bold">LIBRO</h1>
        <CatalogMenu />      
      </div>
    </header>
  );
};

export default Header;
