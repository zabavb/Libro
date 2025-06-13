import Header from "@/components/layouts/Header";
import Footer from "@/components/layouts/Footer";
import LikedBooks from "@/components/book/LikedBooks";


const LikedBooksPage = () => {

    return (
    <div className="flex flex-col min-h-screen">
      <Header />
      <main className="flex-1 overflow-auto">
        <LikedBooks/>
      </main>
      <Footer />
    </div>
    );
};


export default LikedBooksPage;
