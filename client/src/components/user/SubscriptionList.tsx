import SubscriptionCardContainer from '../../containers/user/SubscriptionCardContainer';
import { SubscriptionCard } from '../../types';
import Loading from '../common/Loading';
import Pagination from '../common/Pagination';
import Search from '../common/Search';
import { FixedSizeList as List } from 'react-window';

interface SubscriptionListProps {
  subscriptions?: SubscriptionCard[];
  loading: boolean;
  pagination: { pageNumber: number; pageSize: number; totalCount: number };
  onPageChange: (pageNumber: number) => void;
  onNavigate: (path: string) => void;
  onSearchTermChange: (searchTerm: string) => void;
  searchTerm: string;
}

const UserList: React.FC<SubscriptionListProps> = ({
  subscriptions = [],
  loading,
  pagination,
  onPageChange,
  searchTerm,
  onSearchTermChange,
  onNavigate,
}) => {
  const Card = ({
    index,
    style,
  }: {
    index: number;
    style: React.CSSProperties;
  }) => (
    <div style={style}>
      <SubscriptionCardContainer subscription={subscriptions[index]} />
    </div>
  );

  return (
    <div>
      <p onClick={() => onNavigate('/admin')}>Back to Admin Dashboard</p>
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

export default UserList;
