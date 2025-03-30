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
            <button onClick={() => onSortChange("title")}> 
                Title {sort.title === true ? "↑" : sort.title === false ? "↓" : ""} 
            </button>
            <button onClick={() => onSortChange("price")}> 
                Price {sort.price === true ? "↑" : sort.price === false ? "↓" : ""} 
            </button>
            <button onClick={() => onSortChange("year")}> 
                Year {sort.year === true ? "↑" : sort.year === false ? "↓" : ""} 
            </button>
            <button onClick={() => onSortChange("feedBack")}> 
                Reviews {sort.feedBack === true ? "↑" : sort.feedBack === false ? "↓" : ""} 
            </button>
            <button onClick={() => onSortChange("biggestDiscount")}>
                Biggest Discount {sort.biggestDiscount ? "↑" : ""}
            </button>
        </div>
    )
}

export default BookSort;
