import { CartItem } from '@/types/types/cart/CartItem'
import '@/assets/styles/components/book/book-details.css'
import truckIcon from '@/assets/icons/truckIcon.svg'
import CartIcon from '@/assets/icons/cartIcon.svg'
import FeedbackCard from './FeedbackCard'
import { useEffect, useState } from 'react'
import { isBookLiked, likeBook, unlikeBook } from '@/services/likedBooksStorage'
import noImageUrl from '@/assets/noImage.svg'
import { BookDetailsToBookCard } from '@/api/adapters/bookAdapter'
import {icons} from '@/lib/icons'
import { BookDetails as BookDetailsType } from '@/types/types/book/BookDetails'
interface BookDetailsProps {
    onAddItem: (item: CartItem) => void
    book: BookDetailsType
    loading: boolean
}

const BookDetails: React.FC<BookDetailsProps> = ({ onAddItem, book, loading }) => {
    
    const [liked, setLiked] = useState(false);
    const MAX_STARS = 5;
    useEffect(() => {
        if(book)
            setLiked(isBookLiked(book.bookId));
    }, [book]);

    const toggleLike = (e: React.MouseEvent) => {
        e.stopPropagation();
        if (liked) {
            unlikeBook(book.bookId);
        } else {
            likeBook(BookDetailsToBookCard(book));
        }
        setLiked(!liked);
    };
    
    if (loading) return <p>Loading...</p>
    return (
        <div className="px-16 py-5">
            <div className="flex gap-[60px]">
                {/* Image column */}
                <div className='flex flex-col'>
                    <img className="w-[370px] h-[570px]" alt="Loading..." src={book.imageUrl ? book.imageUrl : noImageUrl} />
                    <div className='w-[370]'>
                        <button onClick={toggleLike}   className={`like-btn transition-all duration-300 ease-in-out border-2 ${
                                liked ? 'bg-[#FF642E] border-[#FF642E]' : 'border-black'
                            }`}>
                            <img className={`transition-all duration-300 ease-in-out ${liked && "invert"}`} src={icons.bHeart}/>
                        </button>
                    </div>
                </div>
                {/* Info column */}
                <div className="flex flex-col gap-[41px]">
                    <div>
                        <h1 className="text-4xl">{book.title}</h1>
                        <p className="sub-text">{book.authorName}</p>
                        <div className="flex">
                            {Array.from({ length: MAX_STARS }).map((_, index) => (
                            <img
                                key={index}
                                src={index < (book.bookFeedbacks.avgRating ?? 0) ? icons.oStarFilled : icons.oStarFilled}
                                alt={index < (book.bookFeedbacks.avgRating ?? 0) ? "Filled star" : "Empty star"}
                            />
                            ))}       
                             <p className="text-[#FF642E]">{book.bookFeedbacks.feedbackAmount} Feedbacks</p>
                        </div>
                    </div>
                    <div className="row">
                        <h1 className="row-title">Format</h1>
                        <div className="flex gap-5">
                            <div className="booktype-btn active">
                                <p>Physical</p>
                                <p className="font-semibold">{book.price.toFixed(2)} UAH</p>
                            </div>
                            {
                                book.hasDigital &&(
                                    <div className="booktype-btn">
                                        <p>Digital</p>
                                        <p className="font-semibold">{book.price.toFixed(2)} UAH</p>
                                    </div>
                                )
                            }

                        </div>
                    </div>
                    <div className="row">
                        <h1 className="row-title">Publisher</h1>
                        <p className="option grayed">{book.publisherName}</p>
                    </div>
                    <div className="row">
                        <h1 className="row-title">Release Year</h1>
                        <p className="option grayed">{new Date(book.year).getFullYear()}</p>
                    </div>
                    <div  className="row">
                        <h1 className="row-title">Category</h1>
                        <div>
                            <p className="option">{book.categoryName}</p>
                        </div>
                    </div>
                    <div className="row">
                        <h1 className="row-title">Description</h1>
                        <p>{book.description}</p>
                    </div>
                    <div className="row">
                        <h1 className="row-title">Product Information</h1>
                        <div className="information-row">
                            <p>Format</p>
                            <p>000x000</p>
                        </div>
                        <div className="information-row">
                            <p>Author</p>
                            <p>{book.authorName}</p>
                        </div>
                        <div className="information-row">
                            <p>Type</p>
                            <p>{book.cover}</p>
                        </div>
                    </div>
                    <div className="row">
                        <h1 className="row-title">About author</h1>
                        <div className="flex gap-5">
                            <img className="w-[160px] h-[240px]" src={book.authorImageUrl} alt='Author Image'/>
                            <div className="flex flex-col gap-5">
                            <p className="max-w-[500px] h-[200px] overflow-hidden text-sm">{book.authorDescription}</p>
                            <p className="sub-text cursor-pointer">More about the author</p>
                            </div>
                        </div>
                    </div>
                    <div className="row">
                        <h1 className="row-title">Reviews</h1>
                        <div className="flex gap-5">
                            {book.latestFeedback.map((feedback) => (
                                <FeedbackCard feedback={feedback}/>
                            ))}
                        </div>
                        <p className="sub-text cursor-pointer">Show all reviews</p>
                        <button className="feedback-btn">Leave a review</button>
                    </div>
                </div>
                {/* Cart column */}
                <div className="buy-container">
                    <div className="flex flex-col gap-2.5">
                        <h1>{book.price.toFixed(2)} UAH</h1>
                        {/* Update later */}
                        <div className="flex">
                            <img src={truckIcon}/>
                            <p className='text-[#9C9C97]'>{book.quantity} pcs. in stock</p>
                        </div>
                    </div>
                    <div className="flex flex-col gap-[17px]">
                        <button className="btn-cart">
                            <div className='flex items-center gap-2.5' 
                            onClick={() => onAddItem({ bookId: book.bookId, amount: 1, name: book.title, price: book.price })}>
                                <img src={CartIcon} className='invert w-[14px] h-[14px]'/>
                                <p>Add to cart</p>
                            </div>
                        </button>
                        <button className="btn-buy">    
                            <p>Buy now</p>
                        </button>
                    </div>
                </div>
            </div>

            {/* History */}
            <div>

            </div>
        </div>
    )
}

export default BookDetails