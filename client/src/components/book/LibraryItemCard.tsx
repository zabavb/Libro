import { BookLibraryItem } from "@/types/types/book/BookDetails"
import noImageUrl from "@/assets/noImage.svg"
import phoneImg from '@/assets/phone.svg'
import headphoneImg from '@/assets/Headphones.svg'
import { useState } from "react"
interface LibraryItemCardProps {
    item: BookLibraryItem
    isAudio: boolean
}


const LibraryItemCard: React.FC<LibraryItemCardProps> = ({ item, isAudio }) => {
    const [hover, setHover] = useState<boolean>(false);
    
    return (
        <div className="flex flex-col gap-2.5 relative">
            <div className="relative cursor-pointer"
            onClick={() =>
                window.open(isAudio ? item.audioUrl : item.pdfFileUrl)
            }
            onMouseEnter={() => setHover(true)}
            onMouseLeave={() => setHover(false)}>
                <div aria-hidden={true} className={`transition-all ${hover ? "opacity-100" : "opacity-0"}`}>
                    <div className="w-full h-full absolute bg-black opacity-80" />
                    <img className="absolute left-[40px] top-[50px]" src={isAudio ? headphoneImg : phoneImg} />
                </div>
                <img src={item.imageUrl ?? noImageUrl} className="w-[200px] h-[200px] object-contain " />
            </div>
            <div className="flex flex-col gap-1.5">
                <p className="font-semibold text-lg leading-4">{item.title}</p>
                <p className="opacity-40 text-lg leading-4">{item.authorName}</p>
            </div>
        </div>
    )
}

export default LibraryItemCard