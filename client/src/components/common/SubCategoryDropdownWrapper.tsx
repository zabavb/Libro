import React, { ReactNode } from "react";
import { icons } from '@/lib/icons'
import "@/assets/styles/components/common/subCategoryDropdown.css"

interface SubCategoryDropdownWrapperProps {
  isOpen?: boolean;
  id: string
  onStateChange: (id:string) => void;
  triggerLabel: string;
  children: ReactNode;
  onDelete: (e: React.MouseEvent,id:string) => void
}

const SubCategoryDropdownWrapper: React.FC<SubCategoryDropdownWrapperProps> = ({ isOpen, id, onStateChange, triggerLabel, children,onDelete }) => {
  return (
    <div className='subcategory-dropdown-wrapper'>
      <div
        onClick={() => onStateChange(id)}
        className="subcategory-dropdown-header"
      >
       {triggerLabel}  
       <img src={icons.oTrash} 
       onClick={(e) => onDelete(e,id)}
       className='p-2.5 border-[1px] border-[#FF642E] rounded-lg'/>
      </div>

      <div 
      className='subcategory-dropdown-container max-h-[1000px]'>
          {isOpen && children}
          <div className="flex justify-center items-center cursor-pointer h-[40px]" onClick={() => onStateChange(id)}>
                <img className={`caret-scale invert ${isOpen ? "caret-open": ""}`} src={icons.wCaretDown}/>
          </div>
        </div>

    </div>
  );
};

export default SubCategoryDropdownWrapper;