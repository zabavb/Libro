import { Author } from "../../../types";
import { icons } from '@/lib/icons'
interface AuthorAdminCardProps {
    author: Author
    onDelete: (e: React.MouseEvent) => void
    onNavigate: () => void 
}

const AuthorAdminCard: React.FC<AuthorAdminCardProps> = ({author, onDelete,onNavigate}) => {
    return(
        <>
            <tr>
                <td className="flex justify-center">
                    <img src={`https://picsum.photos/seed/${author.id}/50/70`} />
                </td>
                <td>
                    {author.name}
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

export default AuthorAdminCard