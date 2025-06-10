import Footer from "@/components/layouts/Footer";
import Header from "@/components/layouts/Header";
import LibraryContainer from "@/containers/books/LibraryContainer";

const LibraryPage = () => {

    return (
    <div className="flex flex-col min-h-screen">
      <Header />
      <main className="flex-1 overflow-auto">
        <LibraryContainer/>
      </main>
      <Footer />
    </div>
    );
};

export default LibraryPage
