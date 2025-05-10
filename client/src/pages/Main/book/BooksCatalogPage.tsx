import Footer from "@/components/layouts/Footer";
import Header from "@/components/layouts/Header";
import BookCatalogContainer from "@/containers/books/BookCatalogContainer";

const BookCatalogPage = () => {

    return (
    <div className="flex flex-col min-h-screen">
      <Header />
      <main className="flex-1 overflow-auto">
        <BookCatalogContainer/>
      </main>
      <Footer />
    </div>
    );
};

export default BookCatalogPage
