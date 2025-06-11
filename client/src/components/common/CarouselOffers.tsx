import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faChevronLeft, faChevronRight } from "@fortawesome/free-solid-svg-icons";
import { Carousel, IconButton } from "@material-tailwind/react";

import img1 from "@/assets/images/1.png";
import img2 from "@/assets/images/2.png";
import img3 from "@/assets/images/3.png";
import img4 from "@/assets/images/4.png";
import img5 from "@/assets/images/5.png";
import img6 from "@/assets/images/6.png";

export function CarouselOffers() {
  return (
    <div className="w-full aspect-[16/5] rounded-xl overflow-hidden relative">
      <Carousel
        autoplay
        loop
        className="h-full w-full"
        placeholder=""
        onPointerEnterCapture={() => {}}
        onPointerLeaveCapture={() => {}}
        navigation={({ setActiveIndex, activeIndex, length }) => (
          <div className="absolute bottom-4 left-2/4 z-50 flex -translate-x-2/4 gap-2">
            {new Array(length).fill("").map((_, i) => (
              <span
                key={i}
                className={`block h-1 cursor-pointer rounded-2xl transition-all content-[''] ${
                  activeIndex === i ? "w-8 bg-white" : "w-4 bg-white/50"
                }`}
                onClick={() => setActiveIndex(i)}
              />
            ))}
          </div>
        )}
        prevArrow={({ handlePrev }) => (
          <IconButton
            variant="text"
            color="white"
            size="lg"
            onClick={handlePrev}
            className="!absolute top-2/4 left-4 -translate-y-2/4"
            placeholder=""
            onPointerEnterCapture={() => {}}
            onPointerLeaveCapture={() => {}}
          >
            <FontAwesomeIcon icon={faChevronLeft} className="h-6 w-6" />
          </IconButton>
        )}
        nextArrow={({ handleNext }) => (
          <IconButton
            variant="text"
            color="white"
            size="lg"
            onClick={handleNext}
            className="!absolute top-2/4 right-4 -translate-y-2/4"
            placeholder=""
            onPointerEnterCapture={() => {}}
            onPointerLeaveCapture={() => {}}
          >
            <FontAwesomeIcon icon={faChevronRight} className="h-6 w-6" />
          </IconButton>
        )}
      >
        <img
          src={img1}
          alt="image 1"
          className="w-full h-full object-cover"
        />
        <img
          src={img2}
          alt="image 2"
          className="w-full h-full object-cover"
        />
        <img
          src={img3}
          alt="image 3"
          className="w-full h-full object-cover"
        />
        <img
          src={img4}
          alt="image 4"
          className="w-full h-full object-cover"
        />
        <img
          src={img5}
          alt="image 5"
          className="w-full h-full object-cover"
        />
        <img
          src={img6}
          alt="image 6"
          className="w-full h-full object-cover"
        />
      </Carousel>
    </div>
  );
}