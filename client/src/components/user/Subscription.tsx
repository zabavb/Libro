import { Subscription as SubscriptionType } from '../../types';

interface SubscriptionFormProps {
  subscription: SubscriptionType;
  isSubscribed: boolean;
  onSubscribe: (id: string) => Promise<void>;
  loading: boolean;
}

const Subscription: React.FC<SubscriptionFormProps> = ({
  subscription,
  isSubscribed,
  onSubscribe,
  loading,
}) => {
  return (
    <>
      <img src={subscription.imageUrl} alt={subscription.title} />
      <h1>{subscription.title}</h1>
      <p>Price: {subscription.price}</p>
      <p>Expiration Days: {subscription.expirationDays}</p>
      <p>Description: {subscription.description}</p>

      <button disabled={loading} onClick={() => onSubscribe(subscription.id)}>
        {isSubscribed ? 'Unsubscribe' : 'Subscribe'}
      </button>
    </>
  );
};

export default Subscription;
