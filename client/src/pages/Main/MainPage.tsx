import { useNavigate } from "react-router-dom";
import "../../index.css";
import Header from "@/components/layouts/Header";
import Footer from "@/components/layouts/Footer";
import { CarouselOffers } from "@/components/common/CarouselOffers";
import { useAuth } from "../../state/context";
import BookCatalogContainer from "@/containers/books/BookCatalogContainer";
import React from "react";
import BookOfTheWeek from "@/components/book/BookOfTheWeek";
import testPhoto from "@/assets/BookImage.svg";
import PromoContainer from "@/containers/books/PromoContainer";
import BookOfTheWeekContainer from "@/containers/books/BookOfTheWeekContainer";
import FeedbackListContainer from "@/containers/books/FeedbackListContainer";
import FeedbackCardContainer from "@/containers/books/FeedbackCardContainer";
const MainPage: React.FC = () => {


  const handleOrder = () => {
    console.log("Order placed for Book of the Week!");
  };
  const handleFavorite = () => {
    console.log("Book added to favorites!");
  };

  return (
    <div className="flex flex-col min-h-screen">
      <Header />
      <main className="flex-1">
        <div className="px-16 py-10">
          <CarouselOffers />
        </div>

          <BookCatalogContainer />
          <BookCatalogContainer isAudioOnly />
          <BookOfTheWeekContainer />
          <FeedbackCardContainer />
          <PromoContainer />

      </main>
      <Footer />
    </div>
  );
};

export default MainPage;
