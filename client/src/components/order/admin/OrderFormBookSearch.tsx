import React, { useRef, useState } from "react"
import { Book } from "../../../types"

interface OrderFormBookSearchProps {
    page: number
    books?: Book[]
    onBookAdd: (bookId: string) => void
    onPageChange: (page: number) => void
}

const OrderFormBookSearch: React.FC<OrderFormBookSearchProps> = ({ page, books, onPageChange, onBookAdd }) => {
    const [searchFocus, setSearchFocus] = useState<boolean>()
    const timeoutRef = useRef<NodeJS.Timeout | null>()

    const handleBookIdSubmit = (e: React.KeyboardEvent<HTMLInputElement>) => {
        if (e.key === "Enter") {
            alert("Submit")
        }
    };

    const handleFocus = (value: boolean) => {
        if (value === false) {
            timeoutRef.current = setTimeout(() => {
                setSearchFocus(false)
            }, 50)
        }
        else if (timeoutRef.current) {
            clearTimeout(timeoutRef.current)
            setSearchFocus(true)
        }
        else {
            setSearchFocus(true)
        }
    }

    const handleBookAdd = (id: string) => {
        onBookAdd(id)
        setSearchFocus(false)
    }

    return (
        <div> {/* Book select container*/}
            <input
                placeholder="Book Search"
                onKeyDown={handleBookIdSubmit}
                onFocus={() => handleFocus(true)}
                onBlur={() => handleFocus(false)}
            />
            {searchFocus === true &&
                (
                    <div style={{ cursor: "pointer" }} onFocus={() => handleFocus(true)}> {/* Book selection container*/}
                        <div>
                            {books?.map((book) => (
                                <div onClick={() => handleBookAdd(book.bookId)}>
                                    <p>{book.title}</p>
                                </div>
                            ))}
                        </div>
                        <hr />
                        <div style={{ display: "flex" }}>
                            <button type="button" onClick={() => onPageChange(page - 1)}>prev</button>
                            <p>{page}</p>
                            <button type="button" onClick={() => onPageChange(page + 1)}>next</button>
                        </div>
                    </div>
                )}
        </div>
    )
}

export default OrderFormBookSearch