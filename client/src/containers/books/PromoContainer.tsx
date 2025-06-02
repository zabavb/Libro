import React from "react";
import SubscribePromo from "@/components/book/SubscribePromo";
import LibroBonusClub from "@/components/book/LibroBonusClub";

const PromoContainer: React.FC = () => {
  return (
    <div className="bg-[#1A1D23] py-10 px-4 md:px-16 ">
      <div className="flex flex-col md:flex-row gap-8">
        <div className="flex-1 bg-[#F4F0E5] rounded-3xl p-8">
          <SubscribePromo />
        </div>
        <div className="w-full md:w-[560px] bg-[#F4F0E5] rounded-3xl p-8 flex items-center">
          <LibroBonusClub />
        </div>
      </div>
    </div>
  );
};

export default PromoContainer;
