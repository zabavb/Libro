import React from 'react';
import { Feedback } from '@/types';
import { icons } from '@/lib/icons'
import { dateToString } from '@/api/adapters/commonAdapters';
interface FeedbackCardProps {
    feedback: Feedback;
}


const FeedbackCard: React.FC<FeedbackCardProps> = ({feedback}) => {
    const MAX_STARS = 5;
    return (
        <div className='py-5 px-[30px] gap-2.5 flex flex-col border-[#1A1D23] border-[1px]  rounded-[24px]'>
            <div className='flex gap-2'>
                <img src={icons.bUser}/>
                <p>USER_NAME</p>
            </div>
            <p>{dateToString(feedback.date)}</p>
            <div className='flex'>
                {Array.from({ length: MAX_STARS }).map((_, index) => (
                            <img
                                key={index}
                                src={index < (feedback.rating ?? 0) ? icons.oStarFilled : icons.oStarEmpty}
                                alt={index < (feedback.rating ?? 0) ? "Filled star" : "Empty star"}
                            />
                            ))}     
            </div>
            <p>
                {feedback.comment}
            </p>
            <a className='opacity-50 text-end'>More Details</a>
        </div>
    );
};

export default FeedbackCard;
