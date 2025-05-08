import { Catalog } from "@/components/common/Catalog";
import AudioBookCard from "@/components/book/audioBook/AudioBookCard";
import AudioBookFilter from "@/components/book/audioBook/AudioBookFilter";
import { useEffect, useMemo, useState, useCallback } from "react";
import { fetchBooksService } from "@/services/bookService";
import { Book, BookSort } from "@/types";
import { BookFilter } from "@/types/filters/BookFilter";
import { useDispatch } from "react-redux";
import { AppDispatch } from "@/state/redux";
import { addNotification } from "@/state/redux/slices/notificationSlice";

const AudioBookCatalogContainer = () => {
  const dispatch = useDispatch<AppDispatch>();
  const [books, setBooks] = useState<Book[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [filters, setFilters] = useState<BookFilter>({});
  const [sort, setSort] = useState<BookSort>({});
  const [pagination, setPagination] = useState({
    pageNumber: 1,
    pageSize: 10,
    totalCount: 0,
  });

  const paginationMemo = useMemo(() => ({ ...pagination }), [pagination]);

  const fetchBookList = useCallback(async () => {
    setLoading(true);
    try {
      const updatedFilters = { ...filters, hasAudio: true };
      const response = await fetchBooksService(
        paginationMemo.pageNumber,
        paginationMemo.pageSize,
        "",
        updatedFilters,
        sort
      );

      if (response.error) {
        dispatch(
          addNotification({ message: response.error, type: "error" })
        );
      }

      if (response.data) {
        setBooks(response.data.items);
        setPagination({
          pageNumber: response.data.pageNumber,
          pageSize: response.data.pageSize,
          totalCount: response.data.totalCount,
        });
      }
    } catch (error) {
      dispatch(
        addNotification({
          message: error instanceof Error ? error.message : String(error),
          type: "error",
        })
      );
    }
    setLoading(false);
  }, [paginationMemo, filters, sort, dispatch]);

  useEffect(() => {
    fetchBookList();
  }, [fetchBookList]);

  const handlePageChange = (pageNumber: number) =>
    setPagination((prev) => ({ ...prev, pageNumber }));

  const handleFilterChange = (newFilters: BookFilter) => {
    setFilters(newFilters);
    setPagination((prev) => ({ ...prev, pageNumber: 1 }));
  };

  const handleSortChange = (field: string) => {
      setSort((prev) => ({ [field as keyof BookSort]: !prev[field as keyof BookSort] }));
      setPagination((prev) => ({ ...prev, pageNumber: 1 }));
    };

  return (
    <Catalog
      items={books}
      loading={loading}
      pagination={pagination}
      onPageChange={handlePageChange}
      filters={filters}
      onFilterChange={handleFilterChange}
      sort={sort}
      onSortChange={handleSortChange}
      CardComponent={AudioBookCard}
      FilterComponent={AudioBookFilter}
      title="Popular audiobooks"
    />
  );
};

export default AudioBookCatalogContainer;
