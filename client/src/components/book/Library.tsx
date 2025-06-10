import { BookLibraryItem } from "@/types/types/book/BookDetails";
import NewsletterAdvert from "../common/NewsletterAdvert";
import Pagination from "../common/Pagination";
import "@/assets/styles/components/book/library.css"
import LibraryItemCard from "./LibraryItemCard";
import { useEffect, useState } from "react";
import LibraryFilters from "./LibraryFilters";
import linkIconUrl from '@/assets/link.svg'
interface LibraryProps {
    items?: BookLibraryItem[];
    loading: boolean;
    pagination: { pageNumber: number; pageSize: number; totalCount: number };
    onPageChange: (pageNumber: number) => void;
    onNavigate: (path: string) => void;
}

const Library: React.FC<LibraryProps> = ({ items, loading, pagination, onPageChange }) => {
    const [filteredItems, setFilteredItems] = useState<BookLibraryItem[]>(items ?? []);
    const [isAudio, setIsAudio] = useState<boolean>(true);

    useEffect(() => {
        if (isAudio && items)
            setFilteredItems(items?.filter(i => i.audioUrl))

        if (!isAudio && items)
            setFilteredItems(items?.filter(i => i.pdfFileUrl))
    }, [isAudio, items])

    return (
        <div className="library-wrapper">
            <h1 className="text-white text-2xl font-semibold">Library</h1>
            <div className="flex gap-5">
                <div className="library-filter-panel">
                    <LibraryFilters
                        isAudio={isAudio}
                        onTypeChange={setIsAudio}
                    />
                </div>
                {filteredItems.length > 0 ?
                    (<div className="library-main-panel">
                        <div className="flex gap-16 flex-wrap">
                            {loading ? (<>Loading...</>) : (
                                filteredItems.map((item) => (
                                    <LibraryItemCard item={item} isAudio={isAudio} />
                                ))
                            )}
                        </div>
                        <Pagination
                            pagination={pagination}
                            onPageChange={onPageChange}
                        />
                    </div>
                    ) : (
                        <div className="flex flex-col gap-3.5">
                            <p className="text-[#9C9C97] font-bold text-lg">Your library is empty</p>
                            <div className="flex gap-2.5">
                                <a className="tosite-btn" href="/">Go to the website</a>
                                <a href="/"><img src={linkIconUrl} /></a>
                            </div>
                        </div>
                    )
                }
            </div>
            <NewsletterAdvert />
        </div>
    )
}

export default Library;
