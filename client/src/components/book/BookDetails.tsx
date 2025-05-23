import { Book } from '@/types'
import { CartItem } from '@/types/types/cart/CartItem'
import '@/assets/styles/components/book/book-details.css'
import starIcon from '@/assets/icons/ratingStar.svg'
import truckIcon from '@/assets/icons/truckIcon.svg'
import CartIcon from '@/assets/icons/cartIcon.svg'
import FeedbackCard from './FeedbackCard'
import { useEffect, useState } from 'react'
import { isBookLiked, likeBook, unlikeBook } from '@/services/likedBooksStorage'
import noImageUrl from '@/assets/noImage.svg'
import { BookToBookView } from '@/api/adapters/bookAdapter'
import {icons} from '@/lib/icons'
interface BookDetailsProps {
    onAddItem: (item: CartItem) => void
    book: Book
    loading: boolean
}

const BookDetails: React.FC<BookDetailsProps> = ({ onAddItem, book, loading }) => {
    
    const [liked, setLiked] = useState(false);

    useEffect(() => {
        if(book)
            setLiked(isBookLiked(book.bookId));
    }, [book]);

    const toggleLike = (e: React.MouseEvent) => {
        e.stopPropagation();
        if (liked) {
            unlikeBook(book.bookId);
        } else {
            likeBook(BookToBookView(book));
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
                        <p className="sub-text">Author Name</p>
                        <div className="flex">
                             <img src={starIcon}/>
                             <img src={starIcon}/>
                             <img src={starIcon}/>
                             <img src={starIcon}/>
                             <img src={starIcon}/>         
                             <p className="text-[#FF642E]">X Feedbacks</p>
                        </div>
                    </div>
                    <div className="row">
                        <h1 className="row-title">Format</h1>
                        <div className="flex gap-5">
                            <div className="booktype-btn active">
                                <p>Physical</p>
                                <p className="font-semibold">{book.price.toFixed(2)} UAH</p>
                            </div>
                            <div className="booktype-btn">
                                <p>Digital</p>
                                <p className="font-semibold">{book.price.toFixed(2)} UAH</p>
                            </div>
                        </div>
                    </div>
                    <div className="row">
                        <h1 className="row-title">Publisher</h1>
                        <p className="option grayed">{book.publisherId}</p>
                    </div>
                    <div className="row">
                        <h1 className="row-title">Release Year</h1>
                        <p className="option grayed">{new Date(book.year).getFullYear()}</p>
                    </div>
                    <div  className="row">
                        <h1 className="row-title">Category</h1>
                        <div>
                            <p className="option">CATEGORY_TMP</p>
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
                            <p>{book.authorId}</p>
                        </div>
                        <div className="information-row">
                            <p>Type</p>
                            <p>{book.cover}</p>
                        </div>
                    </div>
                    <div className="row">
                        <h1 className="row-title">About author</h1>
                        <div className="flex gap-5">
                            <img className="w-[160px] h-[240px]" src={`https://picsum.photos/seed/${book.bookId}/160/240`}/>
                            <div className="flex flex-col gap-5">
                            <p className="max-w-[500px] h-[200px] overflow-hidden text-sm">Lorem ipsum dolor, sit amet consectetur adipisicing elit. Repellat hic, culpa magni sunt debitis optio suscipit placeat at illo dolorem blanditiis quasi! Facilis accusamus totam quasi nam! Dolore, deleniti dolorem.
                            Vitae, voluptate voluptatem. Accusantium, est illo beatae vel aliquam illum eos, maxime voluptate hic quam consectetur et dignissimos autem nam ut totam fugit ratione distinctio esse aperiam nisi cupiditate libero.
                            Dignissimos eius repudiandae quisquam. Cum officiis, non nam iure sapiente nobis id hic quos. Similique voluptatem quos ducimus voluptatibus, quae sequi praesentium natus velit? Omnis rem eum dignissimos recusandae repellat.</p>
                            <p className="sub-text cursor-pointer">More about the author</p>
                            </div>
                        </div>
                    </div>
                    <div className="row">
                        <h1 className="row-title">Reviews</h1>
                        <div className="flex gap-5">
                            <FeedbackCard feedbackId="FeedbackId1"/>
                            <FeedbackCard feedbackId="FeedbackId2"/>
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