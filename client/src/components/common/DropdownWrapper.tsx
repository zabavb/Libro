import React, { useState, ReactNode } from "react";
import { icons } from '@/lib/icons'
import "@/assets/styles/components/common/dropdown-wrapper.css"

interface DropdownWrapperProps {
  triggerLabel: string;
  children: ReactNode;
  triggerClassName?: string
}

const DropdownWrapper: React.FC<DropdownWrapperProps> = ({ triggerLabel, children, triggerClassName }) => {
  const [isOpen, setIsOpen] = useState(false);

  return (
    <div className={`relative inline-block text-left`}>
      <div
        onClick={() => setIsOpen(!isOpen)}
        className={`dropdown-header ${triggerClassName}`}
      >
        {triggerLabel} <img className={`caret-scale ${isOpen ? "caret-open": ""}`} src={icons.wCaretDown}/>
      </div>

      <div className={`dropdown-container 
      ${isOpen ? "max-h-[500px]" : "max-h-0"}`}>
          {children}
        </div>

    </div>
  );
};

export default DropdownWrapper;