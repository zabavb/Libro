import { BookLibraryItem } from "@/types/types/book/BookDetails"
import noImageUrl from "@/assets/noImage.svg"

interface LibraryItemCardProps{
    item: BookLibraryItem
}


const LibraryItemCard: React.FC<LibraryItemCardProps> = ({item}) => {
    return(
        <div className="flex flex-col gap-2.5">
            <img src={item.imageUrl ?? noImageUrl} className="w-[200px] h-[200px] object-contain "/>
            <div className="flex flex-col gap-1.5">
                <p className="font-semibold text-lg leading-4">{item.title}</p>
                <p className="opacity-40 text-lg leading-4">{item.authorName}</p>
            </div>
        </div>
    )
}

export default LibraryItemCard