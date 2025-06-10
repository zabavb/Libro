import Library from "@/components/book/Library";
import { fetchOwnedBooksService } from "@/services/bookService";
import { AppDispatch } from "@/state/redux";
import { addNotification } from "@/state/redux/slices/notificationSlice";
import { User } from "@/types";
import { BookLibraryItem } from "@/types/types/book/BookDetails";
import { getUserFromStorage } from "@/utils/storage";
import React, { useCallback, useEffect, useMemo, useState } from "react";
import { useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";


const LibraryContainer: React.FC = () => {
    const dispatch = useDispatch<AppDispatch>()
    const navigate = useNavigate()
    const [items, setItems] = useState<BookLibraryItem[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [pagination, setPagination] = useState({
        pageNumber: 1,
        pageSize: 10,
        totalCount: 0,
    })

      const user: User | null = getUserFromStorage();

const paginationMemo = useMemo(() => ({...pagination}), [pagination]);

    const fetchLibrary = useCallback(async () => {
        setLoading(true);
        try{
            const response = await fetchOwnedBooksService(
                user?.id ?? "",
                paginationMemo.pageNumber,
                paginationMemo.pageSize,
            );

            if(response.error)
                dispatch(
                    addNotification({
                        message: response.error,
                        type:'error',
                    }),
                );

            if(response && response.data) {
                const paginatedData = response.data;

                setItems(paginatedData.items);
                setPagination({
                    pageNumber: paginatedData.pageNumber,
                    pageSize: paginatedData.pageSize,
                    totalCount: paginatedData.totalCount
                })
            }else throw new Error('invalid response structure');
        }catch(error){
            dispatch(
                addNotification({
                    message: error instanceof Error ? error.message : String(error),
                    type: 'error'
                })
            )
            setItems([])
        }
        setLoading(false);
    }, [paginationMemo, dispatch, user])

    useEffect(() => {
        fetchLibrary();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    },[pagination.pageNumber])

    const handleNavigate = (path: string) => navigate(path)
    
    const handlePageChange = (pageNumber: number) => {
        setPagination((prev) => ({...prev, pageNumber}))
    }



    return (
        <Library 
            items={items}
            loading={loading}
            pagination={pagination}
            onNavigate={handleNavigate}
            onPageChange={handlePageChange}
        />
    );
};

export default LibraryContainer;
