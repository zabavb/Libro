import React from 'react';
import type { BookFilter } from '@/types/filters/BookFilter';
import { CoverType } from '../../types/subTypes/book/CoverType';
import { Language } from '../../types/subTypes/book/Language';

interface BookFilterProps {
  onFilterChange: (filters: BookFilter) => void;
  filters: BookFilter;
}

const BookFilter: React.FC<BookFilterProps> = ({ onFilterChange, filters }) => {
  return (
    <div>
      <h3>Filters</h3>
      <label>
        Author:
        <input
          type='text'
          value={filters.author || ''}
          onChange={(e) =>
            onFilterChange({ ...filters, author: e.target.value })
          }
        />
      </label>
      <label>
        Publisher:
        <input
          type='text'
          value={filters.publisher || ''}
          onChange={(e) =>
            onFilterChange({ ...filters, publisher: e.target.value })
          }
        />
      </label>
      <label>
        Price (from):
        <input
          type='number'
          value={filters.priceFrom || ''}
          onChange={(e) =>
            onFilterChange({ ...filters, priceFrom: Number(e.target.value) })
          }
        />
      </label>
      <label>
        Price (to):
        <input
          type='number'
          value={filters.priceTo || ''}
          onChange={(e) =>
            onFilterChange({ ...filters, priceTo: Number(e.target.value) })
          }
        />
      </label>
      <label>
        Year (from):
        <input
          type='number'
          value={filters.yearFrom || ''}
          onChange={(e) =>
            onFilterChange({ ...filters, yearFrom: Number(e.target.value) })
          }
        />
      </label>
      <label>
        Year (to):
        <input
          type='number'
          value={filters.yearTo || ''}
          onChange={(e) =>
            onFilterChange({ ...filters, yearTo: Number(e.target.value) })
          }
        />
      </label>
      <label>
        Language:
        <select
          value={filters.language || ''}
          onChange={(e) =>
            onFilterChange({ ...filters, language: e.target.value as Language })
          }
        >
          <option value=''>All</option>
          {Object.values(Language).map((lang) => (
            <option key={lang} value={lang}>
              {lang}
            </option>
          ))}
        </select>
      </label>
      <label>
        Cover Type:
        <select
          value={filters.coverType || ''}
          onChange={(e) =>
            onFilterChange({
              ...filters,
              coverType: e.target.value as CoverType,
            })
          }
        >
          <option value=''>All</option>
          {Object.values(CoverType).map((cover) => (
            <option key={cover} value={cover}>
              {cover}
            </option>
          ))}
        </select>
      </label>
      <label>
        In Stock:
        <input
          type='checkbox'
          checked={filters.inStock || false}
          onChange={(e) =>
            onFilterChange({ ...filters, inStock: e.target.checked })
          }
        />
      </label>
      <label>
        Subcategory:
        <input
          type='text'
          value={filters.subcategory || ''}
          onChange={(e) =>
            onFilterChange({ ...filters, subcategory: e.target.value })
          }
        />
      </label>
    </div>
  );
};

export default BookFilter;
