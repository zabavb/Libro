import React, { useState } from 'react';
import '@/assets/styles/base/range-slider.css'
import { BookFilter } from '@/types/filters/BookFilter';

interface AuthorListProps {
    onFilterChange: (field: BookFilter) => void
    filters: BookFilter
}


const AuthorList: React.FC<AuthorListProps> = ({filters, onFilterChange}) => {
    const [searchTerm, setSearchTerm] = useState<string>();
    const [authors, setAuthors] = useState<string[]>(["Maria Koval"]);

    return (
        <div>
            <div>
                <input placeholder='Author search'/>
            </div>
            {authors.map((author) => (
                // TEMPORARY IMPLEMENTATION
                <p className='cursor-pointer' onClick={() => {onFilterChange({...filters, author:"DD45594C-CA70-4272-8485-BD5877D85A1E"})}}>{author}</p>
            ))}
        </div>
    );
};

export default AuthorList;
