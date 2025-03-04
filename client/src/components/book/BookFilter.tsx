import React from "react";
import type { BookFilter } from "../../types/Filters/BookFilter";
import { CoverType } from "../../types/subTypes/Book/CoverType";
import { Language } from "../../types/subTypes/Book/Language";

interface BookFilterProps {
    onFilterChange: (filters: BookFilter) => void;
    filters: BookFilter;
}

const BookFilter: React.FC<BookFilterProps> = ({ onFilterChange, filters }) => {
    return (
        <div>
            <h3>Фільтри</h3>
            <label>
                Автор:
                <input
                    type="text"
                    value={filters.author || ""}
                    onChange={(e) => onFilterChange({ ...filters, author: e.target.value })}
                />
            </label>
            <label>
                Видавництво:
                <input
                    type="text"
                    value={filters.publisher || ""}
                    onChange={(e) => onFilterChange({ ...filters, publisher: e.target.value })}
                />
            </label>
            <label>
                Ціна (від):
                <input
                    type="number"
                    value={filters.priceFrom || ""}
                    onChange={(e) => onFilterChange({ ...filters, priceFrom: Number(e.target.value) })}
                />
            </label>
            <label>
                Ціна (до):
                <input
                    type="number"
                    value={filters.priceTo || ""}
                    onChange={(e) => onFilterChange({ ...filters, priceTo: Number(e.target.value) })}
                />
            </label>
            <label>
                Рік (від):
                <input
                    type="number"
                    value={filters.yearFrom || ""}
                    onChange={(e) => onFilterChange({ ...filters, yearFrom: Number(e.target.value) })}
                />
            </label>
            <label>
                Рік (до):
                <input
                    type="number"
                    value={filters.yearTo || ""}
                    onChange={(e) => onFilterChange({ ...filters, yearTo: Number(e.target.value) })}
                />
            </label>
            <label>
                Мова:
                <select
                    value={filters.language || ""}
                    onChange={(e) => onFilterChange({ ...filters, language: e.target.value as Language })}
                >
                    <option value="">Усі</option>
                    {Object.values(Language).map((lang) => (
                        <option key={lang} value={lang}>{lang}</option>
                    ))}
                </select>
            </label>
            <label>
                Обкладинка:
                <select
                    value={filters.coverType || ""}
                    onChange={(e) => onFilterChange({ ...filters, coverType: e.target.value as CoverType })}
                >
                    <option value="">Усі</option>
                    {Object.values(CoverType).map((cover) => (
                        <option key={cover} value={cover}>{cover}</option>
                    ))}
                </select>
            </label>
            <label>
                Чи в наявності:
                <input
                    type="checkbox"
                    checked={filters.inStock || false}
                    onChange={(e) => onFilterChange({ ...filters, inStock: e.target.checked })}
                />
            </label>
            <label>
                Під категорія:
                <input
                    type="text"
                    value={filters.subcategory || ""}
                    onChange={(e) => onFilterChange({ ...filters, subcategory: e.target.value })}
                />
            </label>
        </div>
    );
};

export default BookFilter;
