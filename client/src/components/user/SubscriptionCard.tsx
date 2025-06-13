import '../../assets/styles/components/subscription.css'
import { SubscriptionCard as SubscriptionCardType } from '../../types';
import { icons } from "@/lib/icons"
interface SubscriptionCardProps {
  subscription: SubscriptionCardType;
  onNavigate: () => void;
  onDelete: (e: React.MouseEvent) => void;
}

const SubscriptionCard: React.FC<SubscriptionCardProps> = ({
  subscription,
  onNavigate,
  onDelete,
}) => {
  return (
      <div className="flex rounded-lg p-2.5 bg-[#1A1D23] justify-between">

        <div className="flex flex-1 text-white items-center gap-4">
          <img
            width={44}
            height={44}
            src={subscription.imageUrl}
            alt={subscription.title}
            className='border-[1px] border-[#FFFFFF] rounded-full'/>
          <div className="font-semibold text-base">{subscription.title}</div>
          <div className="text-base">{subscription.subdescription}</div>
        </div>
        <div className='flex gap-2'>
          <button onClick={onNavigate} className='p-2.5 bg-[#FF642E] rounded-lg'><img src={icons.wPen} /></button>
          <button onClick={onDelete} className='p-2.5 border-[1px] border-[#FF642E] rounded-lg'><img src={icons.oTrash} /></button>
        </div>
      </div>
  );
};

export default SubscriptionCard;
