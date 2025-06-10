import React from "react";
import '@/assets/styles/components/book/library-filters.css'
interface LibraryFiltersProps {
    onTypeChange: (isAudio: boolean) => void
    isAudio: boolean
}

const LibraryFilters: React.FC<LibraryFiltersProps> = ({ onTypeChange, isAudio}) => {

    const handleFilterChange = (isAudio: boolean) => {
        onTypeChange(isAudio)
    }

    return (
        <div className="library-filter-container">
            <div className="flex gap-5 cursor-pointer"
                tabIndex={0}
                role="button"
                onClick={() => handleFilterChange(true)}
                onKeyDown={(e) => {
                    if (e.key === 'Enter' || e.key === ' ') {
                    e.preventDefault();
                    handleFilterChange(true);
                    }
                }}>
                <div className={`w-1 rounded-r-[10px] ${isAudio ? 'bg-[#FF642E]' : 'bg-transparent'}`}></div>
                <p className={`${isAudio ? 'text-[#FF642E]' : 'text-[#1A1D23]'}`}>Audio book</p>
            </div>
            <div className="flex gap-5 cursor-pointer"
                tabIndex={0}
                role="button"
                onClick={() => handleFilterChange(false)}
                onKeyDown={(e) => {
                    if (e.key === 'Enter' || e.key === ' ') {
                    e.preventDefault();
                    handleFilterChange(false);
                    }
                }}>
                <div className={`w-1 rounded-r-[10px] ${!isAudio ? 'bg-[#FF642E]' : 'bg-transparent'}`}></div>
                <p className={`${!isAudio ? 'text-[#FF642E]' : 'text-[#1A1D23]'}`}>Digital book</p>
            </div>
        </div>
    )
}

export default LibraryFilters