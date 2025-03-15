import React, { useRef, useState } from "react"
import { Book } from "../../../types"

interface OrderFormBookSearchProps {
    page: number
    books?: Book[]
    onBookAdd: (bookId: string) => void
    onPageChange: (page: number) => void
}

const OrderFormBookSearch: React.FC<OrderFormBookSearchProps> = ({ page, books, onPageChange, onBookAdd }) => {
    const [search, setSearch] = useState<string>()
    const [searchFocus, setSearchFocus] = useState<boolean>()
    const timeoutRef = useRef<NodeJS.Timeout | null>()

    const handleBookIdSubmit = () => {
        if(search != undefined){
            handleBookAdd(search)
            setSearch("")
        }
    };

    const handleFocus = (value: boolean) => {
        if (value === false) {
            timeoutRef.current = setTimeout(() => {
                setSearchFocus(false)
            }, 100)
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
            <div style={{display:"flex"}}>
                <input
                    placeholder="Book Search"
                    onFocus={() => handleFocus(true)}
                    onBlur={() => handleFocus(false)}
                    onChange={(e) => {setSearch(e.target.value)}}
                    value={search}
                />
                <p style={{cursor:"pointer"}} onClick={handleBookIdSubmit}>Submit</p>
            </div>
            {searchFocus === true &&
                (
                    <div style={{ cursor: "pointer" }} onFocus={() => handleFocus(true)} onBlur={() => handleFocus(false)}> {/* Book selection container*/}
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