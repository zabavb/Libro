import BookCatalogContainer from "@/containers/books/BookCatalogContainer";
import BookDetailsContainer from "@/containers/books/BookDetailsContainer";
import { useParams } from "react-router-dom";

const BookDetailsPage = () => {
    const { bookId } = useParams<{bookId: string}>()
    return (
        <div>
            <main>
                <BookDetailsContainer bookId={bookId ?? ""}/>
            </main>
        </div>
    );
};

export default BookDetailsPage