import "@/assets/styles/components/book/catalog-sort.css";
import "@/assets/styles/components/book/catalog-filter.css";
import { BookFilter } from "@/types/filters/BookFilter";
import RangeSlider from "../common/RangeSlider";
import { useState } from "react";
import DropdownWrapper from "../common/DropdownWrapper";
import AuthorList from "../common/AuthorList";
import { Language } from "@/types/subTypes/book/Language";

interface CatalogFilterProps {
    onFilterChange: (field: BookFilter) => void;
    filters: BookFilter;
    isAudioOnly?: boolean;
}

const CatalogFilter: React.FC<CatalogFilterProps> = ({ onFilterChange, filters, isAudioOnly = false }) => {
    const [priceTo, setPriceTo] = useState<boolean>(true);
    const [priceRange, setPriceRange] = useState<number>(0);

    const applyPriceFilter = () => {
        if (priceTo) {
            const { priceFrom: _priceFrom, ...rest } = filters;
            onFilterChange({ ...rest, priceTo: priceRange });
        } else {
            const { priceTo: _priceTo, ...rest } = filters;
            onFilterChange({ ...rest, priceFrom: priceRange });
        }
    };

    return (
        <div className={`filter-panel-container ${isAudioOnly ? "audio-only" : ""}`}>
            <p>Filters</p>

            <div className="filter-container">
                <DropdownWrapper triggerLabel="Category">
                    <button onClick={() => onFilterChange({ ...filters, subcategory: "" })}>
                        Psychology
                    </button>
                </DropdownWrapper>
            </div>

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

                    <div className="filter-container">
                        <DropdownWrapper triggerLabel="Publisher">
                            <button onClick={() => onFilterChange({ ...filters, publisher: "" })}>
                                Publisher B
                            </button>
                        </DropdownWrapper>
                    </div>

                    <div className="filter-container">
                        <DropdownWrapper triggerLabel="Availability">
                            <p onClick={() => onFilterChange({ ...filters, inStock: true })}>Available</p>
                            <p onClick={() => onFilterChange({ ...filters, inStock: false })}>Not Available</p>
                        </DropdownWrapper>
                    </div>
                </>
            )}

            <div className="filter-container">
                <DropdownWrapper triggerLabel="Language">
                    {Object.values(Language).map((value) => (
                        <p
                            key={value}
                            className="cursor-pointer"
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
                    <RangeSlider min={0} max={1500} value={priceRange} onChange={setPriceRange} />
                </div>
                <div className="flex flex-col gap-4">
                    <div className="flex gap-2">
                        <button
                            className={`price-btn ${!priceTo ? "price-btn-active" : ""}`}
                            onClick={() => setPriceTo(false)}
                        >
                            from
                        </button>
                        <button
                            className={`price-btn ${priceTo ? "price-btn-active" : ""}`}
                            onClick={() => setPriceTo(true)}
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
