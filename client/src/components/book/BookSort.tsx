import type { BookSort } from "../../types";

interface BookSortProps {
    onSortChange: (field: keyof BookSort) => void
    sort: BookSort
}

const BookSort: React.FC<BookSortProps> = ({ onSortChange, sort }) => {
    return (
        <div>
            <button onClick={() => onSortChange("newest")}> 
                Newest {sort.newest === true ? "↑" : sort.newest === false ? "↓" : ""} 
            </button>
            <button onClick={() => onSortChange("alphabetical")}> 
                Title {sort.alphabetical === true ? "↑" : sort.alphabetical === false ? "↓" : ""} 
            </button>
            <button onClick={() => onSortChange("price")}> 
                Price {sort.price === true ? "↑" : sort.price === false ? "↓" : ""} 
            </button>
            <button onClick={() => onSortChange("feedBackCount")}> 
                Reviews {sort.feedBackCount === true ? "↑" : sort.feedBackCount === false ? "↓" : ""} 
            </button>
        </div>
    )
}

export default BookSort;
