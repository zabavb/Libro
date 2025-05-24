import React, { useCallback, useEffect, useMemo, useState } from "react";
import DropdownWrapper from "./DropdownWrapper";
import { Author } from "@/types";
import { useDispatch } from "react-redux";
import { AppDispatch } from "@/state/redux";
import { addNotification } from "@/state/redux/slices/notificationSlice";
import { BookFilter } from "@/types/filters/BookFilter";
import { fetchAuthorsService } from "@/services/authorService";

interface AuthorFiltersProps {
    onSelect: (option: keyof BookFilter, value: string) => void;
    filters: BookFilter;
}

const AuthorFilters: React.FC<AuthorFiltersProps> = ({ onSelect, filters }) => {
    const dispatch = useDispatch<AppDispatch>()
    const [searchTerm, setSearchTerm] = useState<string>("");
    const [authors, setAuthors] = useState<Author[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [pagination, setPagination] = useState({
        pageNumber: 1,
        pageSize: 10,
        totalCount: 0,
    })

    const handleLoadMore = () => {
        if (pagination.pageSize < pagination.totalCount) {
            const newSize = pagination.pageSize + 10
            setPagination((prev) => ({ ...prev, pageSize: newSize }))
        }
    }


    const paginationMemo = useMemo(() => ({ ...pagination }), [pagination]);

    const fetchAuthorsList = useCallback(async () => {
        setLoading(true);
        try {
            const response = await fetchAuthorsService(
                paginationMemo.pageNumber,
                paginationMemo.pageSize,
                searchTerm
            );

            if (response.error)
                dispatch(
                    addNotification({
                        message: response.error,
                        type: 'error',
                    }),
                );

            if (response && response.data) {
                const paginatedData = response.data;
                setAuthors(paginatedData.items);
                setPagination({
                    pageNumber: paginatedData.pageNumber,
                    pageSize: paginatedData.pageSize,
                    totalCount: paginatedData.totalCount
                })
            } else throw new Error('invalid response structure');
            // eslint-disable-next-line @typescript-eslint/no-unused-vars
        } catch (error) {
            setAuthors([])
        }
        setLoading(false);
    }, [paginationMemo, dispatch, searchTerm])

    useEffect(() => {
        const delayDebounce = setTimeout(() => {
            fetchAuthorsList();
        }, 500);

        return () => clearTimeout(delayDebounce);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [pagination.pageNumber, pagination.pageSize, searchTerm])


    return (
        <div className="filter-container">
            <DropdownWrapper triggerLabel="Author">
                <div className="ml-1 my-1">
                    <input 
                    className="text-black rounded-lg border-black bg-[#F4F0E5] p-1" 
                    onChange={(e) => setSearchTerm(e.target.value)}
                    placeholder="Author name"/>
                </div>
                <div className="flex flex-col ">
                    {!loading &&
                        authors.map((author) => (
                            <button
                                key={author.authorId}
                                className={`text-start transition-colors duration-100 hover:text-[#FF642E] ${filters.authorId == author.authorId && "text-[#FF642E]"}`}
                                onClick={() => {
                                    onSelect("authorId", author.authorId)
                                }}>
                                {author.name}
                            </button>
                        ))
                    }
                </div>
                {pagination.totalCount > pagination.pageSize &&
                    <p onClick={handleLoadMore} aria-disabled={loading} className="cursor-pointer transition-colors duration-100 hover:text-[#FF642E]">
                        {loading ? "Loading..." : "Load more..."}
                    </p>
                }
            </DropdownWrapper>
        </div>
    )
}

export default AuthorFilters