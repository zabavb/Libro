import Footer from "@/components/layouts/Footer";
import Header from "@/components/layouts/Header";
import BookDetailsContainer from "@/containers/books/BookDetailsContainer";
import { useParams } from "react-router-dom";

const BookDetailsPage = () => {
    const { bookId } = useParams<{ bookId: string }>()
    return (
        <div className="flex flex-col min-h-screen">
            <Header />
            <main className="flex-1 overflow-auto">
                <BookDetailsContainer bookId={bookId ?? ""} />
            </main>
            <Footer />
        </div>
    );
};

export default BookDetailsPage