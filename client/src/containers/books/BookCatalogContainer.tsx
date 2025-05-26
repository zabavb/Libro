import BookCatalog from "@/components/book/BookCatalog";
import { fetchBooksService } from "@/services/bookService";
import { AppDispatch } from "@/state/redux"
import { addNotification } from "@/state/redux/slices/notificationSlice";
import { Book, BookSort } from "@/types";
import { BookFilter } from "@/types/filters/BookFilter";
import { useCallback, useEffect, useMemo, useState } from "react";
import { useDispatch } from "react-redux"
import { useNavigate } from "react-router-dom"
import { useSearchParams } from "react-router-dom";

type BookCatalogContainerProps = {
    isAudioOnly?: boolean; 
  };
  

const BookCatalogContainer = ({ isAudioOnly = false }: BookCatalogContainerProps) => {
    const dispatch = useDispatch<AppDispatch>();
    const navigate = useNavigate();
    const [books, setBooks] = useState<Book[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [searchTerm, setSearchTerm] = useState<string>('');
    const [filters, setFilters] = useState<BookFilter>({});
    const [sort, setSort] = useState<BookSort>({});
    const [loadedAll, setLoadedAll] = useState<boolean>(false);
    const [searchParams] = useSearchParams();
    const [pagination, setPagination] = useState({
        pageNumber: 1,
        pageSize: 10,
        totalCount: 0,
    })
    

    const paginationMemo = useMemo(() => ({...pagination}), [pagination]);
    const fetchBookList = useCallback(async () => {
        setLoading(true);
        try{
            const updatedFilters = { ...filters };
            if (isAudioOnly) {
            updatedFilters.hasAudio = true; 
            }
            const response = await fetchBooksService(
                paginationMemo.pageNumber,
                paginationMemo.pageSize,
                searchTerm,
                updatedFilters,
                sort
            );
            
            if(response.error)
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
                    totalCount:paginatedData.totalCount,
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
    }, [paginationMemo, searchTerm, filters, sort, dispatch, isAudioOnly])

    const handleLoadMore = () => {
        if(!loadedAll){
            const newSize = pagination.pageSize + 10
            setPagination((prev) => ({...prev, pageSize:newSize}))
        }
    }

    useEffect(() => {
        if(pagination.pageSize >= pagination.totalCount){
            setLoadedAll(true)
        }
        else{
            setLoadedAll(false)
        }
    },[pagination.pageSize, pagination.totalCount, loading])


    useEffect(() => {
        fetchBookList()
        // eslint-disable-next-line react-hooks/exhaustive-deps
    },[sort, filters, pagination.pageNumber, pagination.pageSize, searchTerm])

    useEffect(() => {
        const subCategoryId = searchParams.get("subcategory");
        const categoryId = searchParams.get("category");
        const searchQuery = searchParams.get("search");

        const newFilters: BookFilter = {};

        if (subCategoryId) newFilters.subcategoryId = subCategoryId;
        if (categoryId) newFilters.categoryId = categoryId;

        setFilters(newFilters);
        setSearchTerm(searchQuery || ''); // ⬅️ додано
        setPagination((prev) => ({ ...prev, pageNumber: 1 }));
    }, [searchParams]);


    const handleNavigate = (path: string) => navigate(path);

     const handleSearchTermChange = (newSearchTerm: string) => {
        setSearchTerm(newSearchTerm);
        setPagination((prev) => ({ ...prev, pageNumber: 1}));
    };
    
    const handleFilterChange = (newFilters: BookFilter) => {
        setFilters(newFilters);
        setPagination((prev) => ({ ... prev, pageNumber: 1}));
    }
    
    const handleSortChange = (field: keyof BookSort) => {
        setSort((prevSort) => {
          const current = prevSort[field];
          return { [field]: current === true ? false : true }; // toggles direction
        });
        setPagination((prev) => ({ ...prev, pageNumber: 1 }));
      };
    
    const handlePageChange = (pageNumber: number) => {
        setPagination((prev) => ({...prev, pageNumber}))
    }
    
    return (
        <BookCatalog
            books={books}
            loading={loading}
            pagination={pagination}
            onPageChange={handlePageChange}
            onNavigate={handleNavigate}
            searchTerm={searchTerm}
            onSearchTermChange={handleSearchTermChange}
            filters={filters}
            onFilterChange={handleFilterChange}
            sort={sort}
            onSortChange={handleSortChange}
            isAudioOnly={isAudioOnly}
            onLoadMore={handleLoadMore}
            loadedAll={loadedAll}
        />
    )
}

export default BookCatalogContainer