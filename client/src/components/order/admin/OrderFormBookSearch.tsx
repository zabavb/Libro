import React, { useCallback, useEffect, useMemo, useRef, useState } from "react"
import { Book } from "../../../types"
import { addNotification } from "@/state/redux/slices/notificationSlice"
import { fetchBooksService } from "@/services/bookService"
import { useDispatch } from "react-redux"
import { AppDispatch } from "@/state/redux"
import "@/assets/styles/components/order-book-search.css"
interface OrderFormBookSearchProps {
    onBookAdd: (bookId: string) => void
}

const OrderFormBookSearch: React.FC<OrderFormBookSearchProps> = ({ onBookAdd }) => {
    const [searchFocus, setSearchFocus] = useState<boolean>()
    const timeoutRef = useRef<NodeJS.Timeout | null>()
    const dispatch = useDispatch<AppDispatch>();
    const [books, setBooks] = useState<Book[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [searchTerm, setSearchTerm] = useState<string>('');
    //const [filters, setFilters] = useState<BookFilter>({});
    //const [sort, setSort] = useState<BookSort>({});
    const [pagination, setPagination] = useState({
        pageNumber: 1,
        pageSize: 10,
        totalCount: 0,
    })

    const paginationMemo = useMemo(() => ({ ...pagination }), [pagination]);

    const fetchBookList = useCallback(async () => {
        setLoading(true);
        try {
            const response = await fetchBooksService(
                paginationMemo.pageNumber,
                paginationMemo.pageSize,
                searchTerm,
            );

            if (response.error)
                dispatch(
                    addNotification({
                        message: response.error,
                        type: 'error'
                    })
                );

            if (response && response.data) {
                const paginatedData = response.data;

                setBooks(paginatedData.items);
                setPagination({
                    pageNumber: paginatedData.pageNumber,
                    pageSize: paginatedData.pageSize,
                    totalCount: paginatedData.totalCount,
                });
            }
            else throw new Error('invalid response structure');
        } catch (error) {
            dispatch(
                addNotification({
                    message: error instanceof Error ? error.message : String(error),
                    type: 'error'
                })
            )
            setBooks([])
        }
        setLoading(false);
    }, [paginationMemo, searchTerm, dispatch])

    useEffect(() => {
        fetchBookList()
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [pagination.pageNumber, pagination.pageSize])

    const handleBookIdSubmit = (event: React.KeyboardEvent<HTMLInputElement>) => {
        if (event.key === "Enter" && searchTerm != undefined) {
            handleBookAdd(searchTerm)
            setSearchTerm("")
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

    const onPageChange = (page: number) => {
        // Limiting the page value
        page = Math.min(page, Math.ceil(pagination.totalCount / pagination.pageSize));
        page = Math.max(1, page);

        setPagination((prev) => ({
            ...prev,
            pageNumber: page,
        }));
    }

    if (loading) return <div>loading</div>

    return (
        <div> {/* Book select container*/}
            <div className="flex w-1/2">
                <input
                    className="input-text w-full"
                    placeholder="Book Search"
                    onFocus={() => handleFocus(true)}
                    onBlur={() => handleFocus(false)}
                    onChange={(e) => { setSearchTerm(e.target.value) }}
                    onKeyDown={(e) => { handleBookIdSubmit(e) }}
                    value={searchTerm}
                />
            </div>
            <div className="relative">
                {searchFocus === true &&
                    (
                        <div className="search-menu" onFocus={() => handleFocus(true)} onBlur={() => handleFocus(false)}> {/* Book selection container*/}
                            <div>
                                {books.map((book) => (
                                    <div className="book-item" onClick={() => handleBookAdd(book.bookId)}>
                                        <p>{book.title}</p>
                                    </div>
                                ))}
                            </div>
                            <div className="search-nav-menu">
                                <button type="button" onClick={() => onPageChange(pagination.pageNumber - 1)}>&lt;</button>
                                <p>{pagination.pageNumber}</p>
                                <button type="button" onClick={() => onPageChange(pagination.pageNumber + 1)}>&gt;</button>
                            </div>
                        </div>
                    )}
            </div>
        </div>
    )
}

export default OrderFormBookSearch