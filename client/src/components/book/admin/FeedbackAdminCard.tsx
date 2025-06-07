import { icons } from '@/lib/icons'
import { FeedbackAdminCard as FeedbackAdminCardType } from '@/types/types/book/FeedbackCard';
import noImageUrl from "@/assets/noImage.svg"
interface FeedbackAdminCardProps {
    feedback: FeedbackAdminCardType
}

const FeedbackAdminCard: React.FC<FeedbackAdminCardProps> = ({ feedback }) => {
    const feedbackUid = feedback.feedbackId.split('-')[4];
    
    const formattedDate = new Date(feedback.date)
        .toLocaleDateString("en-GB", {
            year: "2-digit",
            month: "2-digit",
            day: "2-digit"
        })
        .replace(/\//g, ".");
    return (
        <div className='flex flex-col gap-5 rounded-lg px-2.5 py-4 bg-[#DEDBD1] text-[#1A1D23] text-lg'>
            <div className='flex gap-6'>
                <img 
                className='h-[60px] w-[40px]'
                src={feedback.bookImageUrl ?? noImageUrl}/>
                <div className='flex flex-col'>
                    <p className=''>{feedback.title} - {feedbackUid}</p>
                    <div className='flex gap-1'>
                        {[...Array(5)].map((_, i) => (
                        <img src={i + 1 <= feedback.rating ? icons.oStarFilled : icons.oStarEmpty}/>
                        ))}
                    </div>
                </div>
            </div>
            <div>
                <p>{feedback.comment}</p>
            </div>
            <div className='flex gap-3.5 text-[13px]'>
                <p>{formattedDate}</p>
                <p>{feedback.userName ?? "Unknown User"}</p>
            </div>
        </div>
    )
}

export default FeedbackAdminCard