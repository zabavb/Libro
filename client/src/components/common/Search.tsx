import { useState } from "react"
import "@/assets/styles/components/search-bar.css"
interface SearchProps {
	searchTerm: string
	onSearchTermChange: (searchTerm: string) => void
}



const Search: React.FC<SearchProps> = ({ searchTerm, onSearchTermChange }) => {
	const [inputValue, setInputValue] = useState(searchTerm)

	const handleSearch = (event: React.KeyboardEvent<HTMLInputElement>) => {
		if(event.key === "Enter"){
			onSearchTermChange(inputValue)
		}
	}

	return (
		<div className="search-container">
			<input
				className="search"
				type="text"
				placeholder="Search"
				value={inputValue}
				onChange={(e) => setInputValue(e.target.value)}
				onKeyDown={(e) => handleSearch(e)}
			/>
		</div>
	)
}

export default Search
