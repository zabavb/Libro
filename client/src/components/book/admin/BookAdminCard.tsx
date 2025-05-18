import { Book } from "../../../types";
import { icons } from '@/lib/icons'
interface BookAdminCardProps {
    book: Book
    onDelete: (e: React.MouseEvent) => void
    onNavigate: () => void 
}

const BookAdminCard: React.FC<BookAdminCardProps> = ({book, onDelete,onNavigate}) => {
    const bookUid = book.bookId.split('-')[4];
    return(
        <>
            <tr>
                <td className="flex justify-center">
                    <img className="w-[50px] h-[70px]" src={book.imageUrl ? book.imageUrl :`https://picsum.photos/seed/${book.bookId}/50/70`} />
                </td>
                <td>
                    {book.title}
                </td>
                <td>
                    AUTHOR TBI
                </td>
                <td>
                    {bookUid}
                </td>
                <td>
                    GENRE TBI
                </td>
                <td>
                    {book.price}
                </td>
                <td>
                    <div className='flex gap-2'>
                        <button onClick={onNavigate} className='p-2.5 bg-[#FF642E] rounded-lg'><img src={icons.wPen} /></button>
                        <button onClick={onDelete} className='p-2.5 border-[1px] border-[#FF642E] rounded-lg'><img src={icons.oTrash} /></button>
                    </div>
                </td>
            </tr>
        </>
    )
}

export default BookAdminCard
