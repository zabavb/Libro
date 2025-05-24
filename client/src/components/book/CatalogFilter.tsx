import "@/assets/styles/components/book/catalog-sort.css";
import "@/assets/styles/components/book/catalog-filter.css";
import { BookFilter } from "@/types/filters/BookFilter";
import RangeSlider from "../common/RangeSlider";
import { useEffect, useState } from "react";
import DropdownWrapper from "../common/DropdownWrapper";
import AuthorList from "../common/AuthorList";
import { Language } from "@/types/subTypes/book/Language";
import CategoryFilters from "../common/CategoryFilters";
import PublisherFilters from "../common/PublisherFilters";

interface CatalogFilterProps {
    onFilterChange: (field: BookFilter) => void;
    filters: BookFilter;
    isAudioOnly?: boolean;
}

const CatalogFilter: React.FC<CatalogFilterProps> = ({ onFilterChange, filters, isAudioOnly = false }) => {
    const [toPrice, setMaxPrice] = useState<boolean>(true);
    const [priceRange, setPriceRange] = useState<number>(0);

    const applyPriceFilter = () => {
        console.log(priceRange)
        if (toPrice) {
            // eslint-disable-next-line @typescript-eslint/no-unused-vars
            const { minPrice: _minPrice, ...rest } = filters;
            onFilterChange({ ...rest, maxPrice: priceRange });
        } else {
            // eslint-disable-next-line @typescript-eslint/no-unused-vars
            const { maxPrice: _priceTo, ...rest } = filters;
            onFilterChange({ ...rest, minPrice: priceRange });
        }
        console.log(filters)
    };

    const applyFilter = (option: keyof BookFilter, value: string | boolean) => {
        const updatedFilter: BookFilter = {
            ...filters,
            [option]:value,
        };
        onFilterChange(updatedFilter);
    }

    useEffect(() => {
        if(isAudioOnly)
            applyFilter("hasAudio", true);
    },[])


    return (
        <div className={`filter-panel-container ${isAudioOnly ? "audio-only" : ""}`}>
            <p>Filters</p>

            <CategoryFilters
                onSelect={applyFilter} />

            {!isAudioOnly && (
                <>
                    <div className="filter-container">
                        <DropdownWrapper triggerLabel="Book type">
                            <div className="flex flex-col">
                                <p>Physical</p>
                                <p>Digital</p>
                                <p>Audio</p>
                            </div>
                        </DropdownWrapper>
                    </div>

                    <PublisherFilters
                        onSelect={applyFilter}/>

                    <div className="filter-container">
                        <DropdownWrapper triggerLabel="Availability">
                            <p className={`cursor-pointer ${filters.available && "text-[#FF642E]"}`} onClick={() => onFilterChange({ ...filters, available: true })}>Available</p>
                            <p className={`cursor-pointer ${!filters.available && "text-[#FF642E]"}`} onClick={() => onFilterChange({ ...filters, available: false })}>Not Available</p>
                        </DropdownWrapper>
                    </div>
                </>
            )}

            <div className="filter-container">
                <DropdownWrapper triggerLabel="Language">
                    {Object.values(Language).map((value) => (
                        <p
                            key={value}
                            className={`cursor-pointer ${filters.language == value && "text-[#FF642E]"}`}
                            onClick={() => onFilterChange({ ...filters, language: value as Language })}
                        >
                            {value}
                        </p>
                    ))}
                </DropdownWrapper>
            </div>

            <div className="filter-container">
                <DropdownWrapper triggerLabel="Author">
                    <AuthorList onFilterChange={onFilterChange} filters={filters} />
                </DropdownWrapper>
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
                            onClick={() => setMaxPrice(false)}
                        >
                            from
                        </button>
                        <button
                            className={`price-btn ${toPrice ? "price-btn-active" : ""}`}
                            onClick={() => setMaxPrice(true)}
                        >
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
