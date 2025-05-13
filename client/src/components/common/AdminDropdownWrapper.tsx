import React, { useState, ReactNode } from "react";
import { icons } from '@/lib/icons'
import "@/assets/styles/components/common/dropdown-wrapper.css"
import '@/assets/styles/layout/admin-layout.css';

interface AdminDropdownWrapperProps {
  triggerLabel: string;
  children: ReactNode;
  iconUrl: string;
  isActive?: boolean;
}

const AdminDropdownWrapper: React.FC<AdminDropdownWrapperProps> = ({ triggerLabel, children, iconUrl, isActive }) => {
  const [isOpen, setIsOpen] = useState(false);

  return (
    <div className={`relative inline-block text-left flex-1`}>
      <div
        onClick={() => setIsOpen(!isOpen)}
        className="dropdown-header gap-2.5"
      >
        <img src={iconUrl} className={`${isActive && "invert"}`}/> {triggerLabel} <img className={`caret-scale ${isOpen && "caret-open"} ${isActive && 'invert'}`} src={icons.wCaretDown}/>
      </div>

      <div className={`dropdown-container 
      ${isOpen ? "max-h-[500px]" : "max-h-0"}`}>
          {children}
        </div>

    </div>
  );
};

export default AdminDropdownWrapper;