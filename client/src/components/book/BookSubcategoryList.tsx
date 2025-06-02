import React from "react";
import {icons} from '@/lib/icons'
interface BookSubcategoryListProps {
    Subcategories: Record<string, string>;
    RemoveSubcategory: (subcategoryId: string) => void;
    isEnabled: boolean
}

const BookSubcategoryList: React.FC<BookSubcategoryListProps> = ({ Subcategories, isEnabled, RemoveSubcategory }) => {
    return (
        <div>
            <label className="text-[#1A1D23] text-sm">Subcategories:</label>
            <div className="flex gap-2">
                {Object.entries(Subcategories).map(([key, value]) => (
                    <div className="bg-[#DEDBD1] text-[#1A1D23] font-semibold p-2.5 rounded-lg relative">
                        {isEnabled && (
                        <img 
                            className="absolute top-1 right-1 cursor-pointer" 
                            src={icons.oCross}
                            onClick={() => RemoveSubcategory(key)}/>
                        )}

                        {value}
                    </div>
                ))}
            </div>
        </div>
    )
}

export default BookSubcategoryList;