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
}

const PublisherFilters: React.FC<PublisherFiltersProps> = ({ onSelect }) => {
    const dispatch = useDispatch<AppDispatch>()
    const [selectedPublisher, setSelectedPublisher] = useState<string>();
    const [publishers, setPublishers] = useState<Publisher[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [pagination, setPagination] = useState({
        pageNumber: 1,
        pageSize: 1000,
        totalCount: 0,
    })
    

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
                console.log(paginatedData)
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
    },[pagination.pageNumber])


    return (
        <div className="filter-container">
            <DropdownWrapper triggerLabel="Publisher">
                <div className="flex flex-col ">
                {!loading ? 
                    publishers.map((publisher) => (
                        <button 
                        key={publisher.publisherId}
                        className={`text-start ${selectedPublisher == publisher.publisherId && "text-[#FF642E]"}`}
                        onClick={() => {
                            onSelect("publisherid",publisher.publisherId)
                            setSelectedPublisher(publisher.publisherId)
                            }}>
                            {publisher.name}
                        </button>
                    ))
                    :
                    (<div>Loading</div>)
                }
                </div>
            </DropdownWrapper>
        </div>
    )
}

export default PublisherFilters