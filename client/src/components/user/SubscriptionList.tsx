import SubscriptionCardContainer from '../../containers/user/SubscriptionCardContainer';
import { SubscriptionCard } from '../../types';
import Loading from '../common/Loading';
import Pagination from '../common/Pagination';
import Search from '../common/Search';
import { FixedSizeList as List } from 'react-window';
import '../../assets/styles/components/subscription-list.css'
interface SubscriptionListProps {
  subscriptions?: SubscriptionCard[];
  loading: boolean;
  pagination: { pageNumber: number; pageSize: number; totalCount: number };
  onPageChange: (pageNumber: number) => void;
  onNavigate: (path: string) => void;
  onSearchTermChange: (searchTerm: string) => void;
  searchTerm: string;
  onDeleted: (subscriptionId: string) => void;
}

const SubscriptionList: React.FC<SubscriptionListProps> = ({
  subscriptions = [],
  loading,
  pagination,
  onPageChange,
  searchTerm,
  onSearchTermChange,
  onNavigate,
  onDeleted,
}) => {
  const Card = ({
    index,
    style,
  }: {
    index: number;
    style: React.CSSProperties;
  }) => (
    <div style={style}>
      <SubscriptionCardContainer
        key={subscriptions[index].id}
        subscription={subscriptions[index]}
        onDeleted={() => onDeleted(subscriptions[index].id)}
      />
    </div>
  );

  return (
    <div className="subscription-list-wrapper">
      <p onClick={() => onNavigate('/admin/subscriptions/add')}>
        Add Subscription
      </p>
      <h1>Subscription List</h1>

      <Search searchTerm={searchTerm} onSearchTermChange={onSearchTermChange} />

      {loading ? (
        <Loading />
      ) : subscriptions.length > 0 ? (
        <List
          height={600}
          itemCount={subscriptions.length}
          itemSize={100}
          width={'100%'}
        >
          {Card}
        </List>
      ) : (
        <p>No subscriptions found.</p>
      )}

      <hr />

      <Pagination pagination={pagination} onPageChange={onPageChange} />
    </div>
  );
};

export default SubscriptionList;
