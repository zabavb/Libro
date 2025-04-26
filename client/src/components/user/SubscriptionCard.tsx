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
        <div>{subscription.title}</div>
        <div>{subscription.description}</div>

        <div>Edit</div>
        <div
          onClick={(e) => {
            e.stopPropagation();
            onDelete(e);
          }}
        >
          Remove
        </div>
      </li>
    </>
  );
};

export default SubscriptionCard;
