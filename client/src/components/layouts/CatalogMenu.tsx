import React, { useEffect, useState } from "react";
import "@/assets/styles/layout/catalog-menu.css";
import { icons } from "@/lib/icons";
import { fetchCategoriesService } from "@/services/categoryService";
import { fetchSubCategoriesService } from "@/services/subCategoryService";
import { Category, SubCategory } from "@/types";
import { useNavigate } from "react-router-dom";

interface CatalogMenuProps {
  isOpen: boolean;
  setIsOpen: (isOpen: boolean) => void;
}

const CatalogMenu: React.FC<CatalogMenuProps> = ({ isOpen, setIsOpen }) => {
  const [categories, setCategories] = useState<Category[]>([]);
  const [subCategories, setSubCategories] = useState<SubCategory[]>([]);
  const [activeCategoryId, setActiveCategoryId] = useState<string | null>(null);
  const navigate = useNavigate();


  useEffect(() => {
    const loadCategories = async () => {
      const res = await fetchCategoriesService(1, 100);
      if (res.data) setCategories(res.data.items);
    };
    if (isOpen) loadCategories();
  }, [isOpen]);

  const handleCategoryClick = async (categoryId: string) => {
    setActiveCategoryId(categoryId);
    const res = await fetchSubCategoriesService(1, 100, undefined, { categoryId });
    if (res.data && res.data.items.length > 0) {
      setSubCategories(res.data.items);
    } else {
      setSubCategories([]);
      setIsOpen(false);
      navigate(`/catalog?category=${categoryId}`);
    }
  };
  const handleSubCategoryClick = (subCategoryId: string) => {
    setIsOpen(false);
    navigate(`/catalog?subcategory=${subCategoryId}&category=${activeCategoryId}`);
 };


  

  return (
    <>
      <div
        className={`dim ${isOpen ? 'visible' : ''}`}
        onClick={() => setIsOpen(false)}
        aria-hidden={!isOpen}
      />

      <div className={`catalog-menu ${isOpen ? 'visible' : ''}`} aria-hidden={!isOpen} aria-modal={true}>
        <img
          src={icons.gMenuClose}
          alt="close"
          onClick={() => setIsOpen(false)}
          className="cursor-pointer absolute top-4 right-4 z-10 w-6 h-6"
        />

        <div className="flex w-full overflow-hidden rounded-[30px] min-h-[300px] shadow-lg">
          <div className="w-1/2 bg-[#F4F0E5] text-[#1A1D23] p-6 rounded-l-[30px] pt-10">
            <ul className="flex flex-col gap-2">
              {categories.map((cat) => (
                <li
                    key={cat.categoryId}
                    onClick={() => handleCategoryClick(cat.categoryId)}
                    className={`cursor-pointer flex justify-between items-center text-base font-semibold transition-colors ${
                        activeCategoryId === cat.categoryId ? 'text-[#FF642E]' : ''
                    }`}
                    >
                    {cat.name}
                    <img src={icons.pointer} alt="pointer" />
                </li>

              ))}
            </ul>
          </div>

            <div className="w-1/2 bg-[#1A1D23] text-white p-6 rounded-r-[30px] pt-10">
                {subCategories.length > 0 ? (
                    subCategories.map((sub) => (
                    <div
                        key={sub.subCategoryId}
                        onClick={() => handleSubCategoryClick(sub.subCategoryId)}
                        className="flex justify-between items-center text-base font-semibold cursor-pointer hover:text-[#FF642E] transition-colors"
                    >
                        {sub.name}
                        <img src={icons.pointerLight} alt="pointer" />
                    </div>
                    ))
                ) : (
                    <div className="italic text-gray-400">No subcategories</div>
                )}
            </div>
        </div>
      </div>
    </>
  );
};

export default CatalogMenu;
