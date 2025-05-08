import React from "react";

const LibroBonusClub: React.FC = () => {
  return (
    <div className="bg-[#f4f1ea] rounded-xl p-6 flex flex-col justify-between h-full">
      <div>
        <h3 className="text-black font-bold text-xl mb-2">Libro Bonus Club</h3>
        <p className="text-sm text-gray-700">
          Earn bonuses for every purchase and use them for discounts on your favorite books!
        </p>
      </div>
      <div className="mt-6">
        <button className="bg-black text-white rounded-lg px-6 py-2 hover:bg-gray-800 transition">
          Learn more
        </button>
      </div>
    </div>
  );
};

export default LibroBonusClub;