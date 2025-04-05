import BookCatalogContainer from "@/containers/books/BookCatalogContainer";
import { useNavigate } from "react-router-dom"

const BookCatalogPage = () => {
    const navigate = useNavigate();

    return (
        <div>
            <header>
                <h1>Catalog</h1>
                <button onClick={() => navigate('/')}>
                Back
                </button>
            </header>
            <main>
                <BookCatalogContainer/>
            </main>
        </div>
    );
};

export default BookCatalogPage