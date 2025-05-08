import React from "react";

const SubscribePromo: React.FC = () => {
  return (
    <div className="bg-[#f4f1ea] rounded-xl p-6 flex flex-col justify-between h-full">
      <div>
        <div className="text-black font-bold text-lg md:text-xl mb-2">
          <span className="inline-block bg-black text-white px-2 py-1 rounded-md mr-2">-15%</span>
          discount on your first purchase for subscribing to our newsletter
        </div>
        <p className="text-sm text-gray-700">
          Join our community to receive updates on the latest promotions and products
        </p>
      </div>
      <div className="flex flex-col sm:flex-row items-center gap-3 mt-4">
        <input
          type="email"
          placeholder="Enter your email address"
          className="border border-gray-400 rounded-lg px-4 py-2 w-full sm:w-64"
        />
        <button className="bg-black text-white rounded-lg px-6 py-2 hover:bg-gray-800 transition w-full sm:w-auto">
          Subscribe
        </button>
      </div>
    </div>
  );
};

export default SubscribePromo;