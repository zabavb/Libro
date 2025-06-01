import { useEffect, useCallback, useState, useMemo } from "react"
import { useDispatch } from "react-redux"
import { AppDispatch } from "../../state/redux/index"
import { useNavigate } from "react-router-dom"
import { BookSort } from "@/types"
import { addNotification } from "@/state/redux/slices/notificationSlice"
import BookList from "@/components/book/BookList"
import { BookFilter } from "@/types/filters/BookFilter"
import { fetchBooksService } from "@/services/bookService"
import { BookCard } from "@/types/types/book/BookDetails"

const BookListContainer = () => {
  const dispatch = useDispatch<AppDispatch>()
  const navigate = useNavigate()
  const [books, setBooks] = useState<BookCard[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [sort, setSort] = useState<BookSort>({});
  const [filters, setFilters] = useState<BookFilter>({});
  const [pagination, setPagination] = useState({
    pageNumber: 1,
    pageSize: 10,
    totalCount: 0,
  })

  const paginationMemo = useMemo(() => ({...pagination}), [pagination]);

  const fetchBooksList = useCallback(async () => {
    setLoading(true);
    try{
      const response = await fetchBooksService(
        paginationMemo.pageNumber,
        paginationMemo.pageSize,
        searchTerm,
        filters,
        sort
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

        setBooks(paginatedData.items);
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
      setBooks([])
    }
    setLoading(false);
  }, [paginationMemo, searchTerm, sort, filters, dispatch])

  useEffect(() => {
    fetchBooksList();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  },[pagination.pageNumber, searchTerm])

  const handleNavigate = (path: string) => navigate(path)
  
  const handlePageChange = (pageNumber: number) => {
    setPagination((prev) => ({...prev, pageNumber}))
  }

  const handleSearchTermChange = (newSearchTerm: string) => {
    setSearchTerm(newSearchTerm);
    setPagination((prev) => ({ ...prev, pageNumber: 1}));
  }

  const handleSortChange = (field: keyof BookSort) => {
    setSort({ [field]: true});
    setPagination((prev) => ({ ...prev,pageNumber: 1}));
  }

    const handleFilterChange = (filters: BookFilter) => {
      setFilters(filters);
      setPagination((prev) => ({ ...prev, pageNumber: 1 }));
    };
  

  return (
    <BookList
      books={books}
      loading={loading}
      onFilterChange={handleFilterChange}
      filters={filters}
      onSortChange={handleSortChange}
      sort={sort}
      onSearchTermChange={handleSearchTermChange}
      searchTerm={searchTerm}
      onPageChange={handlePageChange}
      pagination={pagination}
      onNavigate={handleNavigate}
    />
  )
}

export default BookListContainer
