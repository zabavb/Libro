import { useState } from 'react';
import '@/assets/styles/components/search-bar.css';
import {icons} from '@/lib/icons'
interface SearchProps {
  searchTerm: string;
  onSearchTermChange: (searchTerm: string) => void;
}

const Search: React.FC<SearchProps> = ({ searchTerm, onSearchTermChange }) => {
  const [inputValue, setInputValue] = useState(searchTerm);

  const handleSearch = (event: React.KeyboardEvent<HTMLInputElement>) => {
    if (event.key === 'Enter') onSearchTermChange(inputValue);
  };

  return (
    <label htmlFor='search' className='search-container'>
      <label htmlFor='search'>
        <img src={icons.bMagnifyingGlass}/>
      </label>
      <input
        id='search'
        className='search'
        type='text'
        placeholder='Search'
        value={inputValue}
        onChange={(e) => setInputValue(e.target.value)}
        onKeyDown={(e) => handleSearch(e)}
      />
    </label>
  );
};

export default Search;
