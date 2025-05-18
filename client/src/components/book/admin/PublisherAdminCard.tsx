import { Publisher } from "@/types/types/book/Publisher";
import { icons } from '@/lib/icons'
interface PublisherAdminCardProps {
    publisher: Publisher
    onDelete: (e: React.MouseEvent) => void
    onNavigate: () => void
}

const PublisherAdminCard: React.FC<PublisherAdminCardProps> = ({ publisher, onDelete, onNavigate }) => {
    return (
        <>
            <div className="flex font-semibold bg-[#1A1D23] text-white rounded-lg p-2.5 items-center">
                <div className="flex-1">{publisher.name}</div>
                <div className='flex gap-2 float-right'>
                    <button onClick={onNavigate} className='p-2.5 bg-[#FF642E] rounded-lg'><img src={icons.wPen} /></button>
                    <button onClick={onDelete} className='p-2.5 border-[1px] border-[#FF642E] rounded-lg'><img src={icons.oTrash} /></button>
                </div>
            </div>
        </>
    )
}

export default PublisherAdminCard