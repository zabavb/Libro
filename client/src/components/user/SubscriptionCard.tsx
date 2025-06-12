import '../../assets/styles/components/subscription.css'
import { SubscriptionCard as SubscriptionCardType } from '../../types';

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
    <>
      <hr />
      <li
        className="subscription-card"
        onClick={(e) => {
          e.stopPropagation();
          onNavigate();
        }}
      >
        <img
          width={44}
          height={44}
          src={subscription.imageUrl}
          alt={subscription.title}
        />
        <div className="subscription-info">
          <div className="subscription-title">{subscription.title}</div>
          <div className="subscription-subdesc">{subscription.subdescription}</div>
        </div>
        <div className="subscription-actions">
          <div className="edit">Edit</div>
          <div
            className="remove"
            onClick={(e) => {
              e.stopPropagation();
              onDelete(e);
            }}
          >
            Remove
          </div>
        </div>
      </li>
    </>
  );
};

export default SubscriptionCard;
