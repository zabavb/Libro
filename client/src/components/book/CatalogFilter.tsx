import "@/assets/styles/components/book/catalog-sort.css";
import "@/assets/styles/components/book/catalog-filter.css";
import { BookFilter } from "@/types/filters/BookFilter";
import RangeSlider from "../common/RangeSlider";
import { useEffect, useState } from "react";
import DropdownWrapper from "../common/DropdownWrapper";
import { Language } from "@/types/subTypes/book/Language";
import CategoryFilters from "../common/CategoryFilters";
import PublisherFilters from "../common/PublisherFilters";
import AuthorFilters from "../common/AuthorFilters";

interface CatalogFilterProps {
    onFilterChange: (field: BookFilter) => void;
    filters: BookFilter;
    isAudioOnly?: boolean;
}

const CatalogFilter: React.FC<CatalogFilterProps> = ({ onFilterChange, filters, isAudioOnly = false }) => {
    const [toPrice, setMaxPrice] = useState<boolean>(true);
    const [priceRange, setPriceRange] = useState<number>(0);

    const applyPriceFilter = () => {
        if (toPrice) {
            // eslint-disable-next-line @typescript-eslint/no-unused-vars
            const { minPrice: _minPrice, ...rest } = filters;
            onFilterChange({ ...rest, maxPrice: priceRange });
        } else {
            // eslint-disable-next-line @typescript-eslint/no-unused-vars
            const { maxPrice: _priceTo, ...rest } = filters;
            onFilterChange({ ...rest, minPrice: priceRange });
        }
    };

    const applyFilter = (option: keyof BookFilter, value: string | boolean) => {
        const updatedFilter: BookFilter = {
            ...filters,
            [option]: value,
        };

        if (option === "categoryId") {
        updatedFilter.subcategoryId = undefined;
        }

        onFilterChange(updatedFilter);
    }

    useEffect(() => {
        if (isAudioOnly)
            applyFilter("hasAudio", true);
    }, [])

    const setType = (option?: keyof BookFilter) => {
        let updatedFilter: BookFilter = filters;
        if (option === "hasAudio") {
            updatedFilter = { ...filters, hasAudio: true, hasDigital: false }
        }
        else if (option === "hasDigital") {
            updatedFilter = { ...filters, hasAudio: false, hasDigital: true }
        }
        else if (option === undefined) {
            updatedFilter = { ...filters, hasAudio: false, hasDigital: false }
        }
        onFilterChange(updatedFilter);
    }

    return (
        <div className={`filter-panel-container ${isAudioOnly ? "audio-only" : ""}`}>
            <p>Filters</p>

            <CategoryFilters
                filters={filters}
                onSelect={applyFilter} />

            {!isAudioOnly && (
                <>
                    <div className="filter-container">
                        <DropdownWrapper triggerLabel="Book type">
                            <div className="flex flex-col">
                                {/* <p
                                    className={`transition-colors duration-100 hover:text-[#FF642E] cursor-pointer ${(!filters.hasAudio && !filters.hasDigital) && "text-[#FF642E]"}`}
                                    onClick={() => setType()}>
                                    Physical
                                </p> */}
                                <p
                                    className={`transition-colors duration-100 hover:text-[#FF642E] cursor-pointer ${filters.hasDigital && "text-[#FF642E]"}`}
                                    onClick={() => setType("hasDigital")}>
                                    Digital
                                </p>
                                <p
                                    className={`transition-colors duration-100 hover:text-[#FF642E] cursor-pointer ${filters.hasAudio && "text-[#FF642E]"}`}
                                    onClick={() => setType("hasAudio")}>
                                    Audio
                                </p>
                            </div>
                        </DropdownWrapper>
                    </div>

                    <PublisherFilters
                        filters={filters}
                        onSelect={applyFilter} />

                    <div className="filter-container">
                        <DropdownWrapper triggerLabel="Availability">
                            <p className={`cursor-pointer transition-colors duration-100 hover:text-[#FF642E] ${filters.available && "text-[#FF642E]"}`} onClick={() => onFilterChange({ ...filters, available: true })}>Available</p>
                            <p className={`cursor-pointer transition-colors duration-100 hover:text-[#FF642E] ${!filters.available && "text-[#FF642E]"}`} onClick={() => onFilterChange({ ...filters, available: false })}>Not Available</p>
                        </DropdownWrapper>
                    </div>
                </>
            )}

            <div className="filter-container">
                <DropdownWrapper triggerLabel="Language">
                    {Object.values(Language).map((value) => (
                        <p
                            key={value}
                            className={`cursor-pointer transition-colors duration-100 hover:text-[#FF642E] ${filters.language == value && "text-[#FF642E]"}`}
                            onClick={() => onFilterChange({ ...filters, language: value as Language })}
                        >
                            {value}
                        </p>
                    ))}
                </DropdownWrapper>
            </div>

            <div className="filter-container">
                <AuthorFilters
                    filters={filters}
                    onSelect={applyFilter} />
                {/* <DropdownWrapper triggerLabel="Author">
                    <AuthorList onFilterChange={onFilterChange} filters={filters} />
                </DropdownWrapper> */}
            </div>

            <div>
                Price
                <div>
                    <RangeSlider min={0} max={9999} value={priceRange} onChange={setPriceRange} />
                </div>
                <div className="flex flex-col gap-4">
                    <div className="flex gap-2">
                        <button
                            className={`price-btn ${!toPrice ? "price-btn-active" : ""}`}
                            onClick={() => setMaxPrice(false)}>
                            from
                        </button>
                        <button
                            className={`price-btn ${toPrice ? "price-btn-active" : ""}`}
                            onClick={() => setMaxPrice(true)}>
                            to
                        </button>
                    </div>
                    <button className="apply-btn" onClick={applyPriceFilter}>
                        Apply
                    </button>
                </div>
            </div>
        </div>
    );
};

export default CatalogFilter;
