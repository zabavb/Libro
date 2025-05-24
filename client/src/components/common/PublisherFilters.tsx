import React, { useCallback, useEffect, useMemo, useState } from "react";
import DropdownWrapper from "./DropdownWrapper";
import { Publisher } from "@/types";
import { useDispatch } from "react-redux";
import { AppDispatch } from "@/state/redux";
import { fetchPublishersService } from "@/services";
import { addNotification } from "@/state/redux/slices/notificationSlice";
import { BookFilter } from "@/types/filters/BookFilter";

interface PublisherFiltersProps {
    onSelect: (option: keyof BookFilter, value: string) => void;
    filters: BookFilter;
}

const PublisherFilters: React.FC<PublisherFiltersProps> = ({ onSelect, filters }) => {
    const dispatch = useDispatch<AppDispatch>()
    const [publishers, setPublishers] = useState<Publisher[]>([]);
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

    const fetchPublishersList = useCallback(async () => {
        setLoading(true);
        try {
            const response = await fetchPublishersService(
                paginationMemo.pageNumber,
                paginationMemo.pageSize,
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
                setPublishers(paginatedData.items);
                setPagination({
                    pageNumber: paginatedData.pageNumber,
                    pageSize: paginatedData.pageSize,
                    totalCount: paginatedData.totalCount
                })
            } else throw new Error('invalid response structure');
        } catch (error) {
            dispatch(
                addNotification({
                    message: error instanceof Error ? error.message : String(error),
                    type: 'error'
                })
            )
            setPublishers([])
        }
        setLoading(false);
    }, [paginationMemo, dispatch])

    useEffect(() => {
        fetchPublishersList();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [pagination.pageNumber, pagination.pageSize])


    return (
        <div className="filter-container">
            <DropdownWrapper 
            triggerLabel="Publisher"
            triggerClassName={`transition-colors duration-100 hover:text-[#FF642E] ${filters.publisherid !== undefined && "text-[#FF642E]" }`}>
                <div className="flex flex-col gap-2">
                    {!loading ?
                        publishers.map((publisher) => (
                            <button
                                key={publisher.publisherId}
                                className={`text-start transition-colors duration-100 hover:text-[#FF642E] ${filters.publisherid == publisher.publisherId && "text-[#FF642E]"}`}
                                onClick={() => {
                                    onSelect("publisherid", publisher.publisherId)
                                }}>
                                {publisher.name}
                            </button>
                        ))
                        :
                        (<div>Loading</div>)
                    }
                    {pagination.totalCount > pagination.pageSize &&
                        <p onClick={handleLoadMore} aria-disabled={loading} className="cursor-pointer transition-colors duration-100 hover:text-[#FF642E]">
                            {loading ? "Loading..." : "Load more..."}
                        </p>
                    }
                </div>

            </DropdownWrapper>
        </div>
    )
}

export default PublisherFilters