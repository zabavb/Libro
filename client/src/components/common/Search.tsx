import { useState } from "react"

interface SearchProps {
	searchTerm: string
	onSearchTermChange: (searchTerm: string) => void
}

const Search: React.FC<SearchProps> = ({ searchTerm, onSearchTermChange }) => {
	const [inputValue, setInputValue] = useState(searchTerm)

	return (
		<div style={{alignContent:"center", margin:"0 20px"}}>
			<input
				type="text"
				placeholder="Search..."
				value={inputValue}
				onChange={(e) => setInputValue(e.target.value)}
			/>
			<button onClick={() => onSearchTermChange(inputValue)}>Search</button>
		</div>
	)
}

export default Search
