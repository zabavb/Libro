import { useEffect, useState } from "react";
import NewsletterAdvert from "../common/NewsletterAdvert"
import { BookCard } from "@/types/types/book/BookDetails";
import { getLikedBooks } from "@/services/likedBooksStorage";
import linkIconUrl from "@/assets/link.svg"

import BookCardContainer from "@/containers/books/BookCardContainer";
const LikedBooks: React.FC = () => {
    const [likedBooks, setLikedBooks] = useState<BookCard[]>([]);

    useEffect(() => {
        setLikedBooks(getLikedBooks());
    }, []);

    return (
        <div className="library-wrapper">
            <h1 className="text-white text-2xl font-semibold">Liked books</h1>
            <div className="flex gap-5">
                {likedBooks.length > 0 ?
                    (<div className="library-main-panel">
                        <div className="flex gap-16 flex-wrap">
                           {
                                likedBooks.map((item) => (
                                    <BookCardContainer book={item} key={item.bookId} />
                                ))
                            }
                        </div>
                    </div>
                    ) : (
                        <div className="flex flex-col gap-3.5">
                            <p className="text-[#9C9C97] font-bold text-lg">No liked books found</p>
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
export default LikedBooks