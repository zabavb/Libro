import React from "react";
import { SubCategory } from "../../../types";
import '@/assets/styles/components/book/sub-category-list.css'
import {icons} from '@/lib/icons'
interface SubCategoryListProps {
    subCategories?: SubCategory[];
    loading: boolean;
    onDelete: (e: React.MouseEvent,id:string) => void
}

const SubCategoryList: React.FC<SubCategoryListProps> = ({
    subCategories = [],
    loading,
    onDelete,
}) => {
    if (loading) return <p>Loading...</p>
    return (
        <div className="mt-5">
            {subCategories.length > 0 ? (
                <div className="flex flex-col gap-2.5">
                    {subCategories.map((subCategory) => (
                        <div className="flex justify-between items-center">
                            <p className="font-semibold">{subCategory.name.toUpperCase()}</p>
                            <div className='flex gap-2'>
                                <button onClick={(e) => onDelete(e,subCategory.subCategoryId)} className='p-2.5 border-[1px] border-[#FF642E] rounded-lg'><img src={icons.oTrash} /></button>
                            </div>
                        </div>
                    ))}
                </div>
            ) : (
                <p>No subcategories found.</p>
            )}

        </div>
    )


}

export default SubCategoryList;