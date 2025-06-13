import React, { useCallback, useEffect, useMemo, useRef, useState } from "react"
import { addNotification } from "@/state/redux/slices/notificationSlice"
import { useDispatch } from "react-redux"
import { AppDispatch } from "@/state/redux"
import "@/assets/styles/components/book/author-search.css"
import { Category } from "@/types"
import { fetchCategoriesService } from "@/services"
interface BookFormCategorySearchProps {
    existingCategory?: string;
    onSelect: (categoryId: string) => void
    isEnabled: boolean
}

const BookFormCategorySearch: React.FC<BookFormCategorySearchProps> = ({ existingCategory, onSelect, isEnabled }) => {
    const [searchFocus, setSearchFocus] = useState<boolean>()
    const timeoutRef = useRef<NodeJS.Timeout | null>()
    const dispatch = useDispatch<AppDispatch>();
    const [categories, setCategories] = useState<Category[]>([]);
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

    const fetchCategoriesList = useCallback(async () => {
            setLoading(true);
            try{
                const response = await fetchCategoriesService(
                    paginationMemo.pageNumber,
                    paginationMemo.pageSize,
                    searchTerm,
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
    
                    setCategories(paginatedData.items);
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
                setCategories([])
            }
            setLoading(false);
        }, [paginationMemo, searchTerm, dispatch])

    useEffect(() => {
        const delayDebounce = setTimeout(() => {
            fetchCategoriesList();
        }, 500);

        return () => clearTimeout(delayDebounce);
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [pagination.pageNumber, pagination.pageSize,searchTerm])

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

    const handleBookAdd = (id: string, categoryName: string) => {
        onSelect(id)
        setSearchFocus(false)
        setSearchTerm(categoryName)
    }

    const onPageChange = (page: number) => {
        page = Math.min(page, Math.ceil(pagination.totalCount / pagination.pageSize));
        page = Math.max(1, page);

        setPagination((prev) => ({
            ...prev,
            pageNumber: page,
        }));
    }


    return (
        <div className="flex flex-col flex-1">
            <div className="input-row w-full">
                <label className='text-sm'>Category</label>
                <input
                    className='input-field'
                    placeholder="Category"
                    onFocus={() => handleFocus(true)}
                    onBlur={() => handleFocus(false)}
                    onChange={(e) => { setSearchTerm(e.target.value) }}
                    value={existingCategory ?? searchTerm}
                    disabled={isEnabled}
                />
            </div>
            <div className="relative">
                {searchFocus === true &&
                    (
                        <div className="search-menu" onFocus={() => handleFocus(true)} onBlur={() => handleFocus(false)}> 
                            <div>
                                {categories.map((category) => (
                                    <div className="book-item" onClick={() => handleBookAdd(category.categoryId, category.name)}>
                                        <p>{category.name}</p>
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

export default BookFormCategorySearch