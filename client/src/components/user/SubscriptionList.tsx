import SubscriptionCardContainer from '../../containers/user/SubscriptionCardContainer';
import { SubscriptionCard, User } from '../../types';
import Loading from '../common/Loading';
import Pagination from '../common/Pagination';
import Search from '../common/Search';
import { FixedSizeList as List } from 'react-window';
import '../../assets/styles/components/subscription-list.css'
import { getUserFromStorage } from '@/utils/storage';
import { icons } from "@/lib/icons"
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
  const user: User | null = getUserFromStorage();

  return (
    <div>
      <header className="header-container">
        <Search
          searchTerm={searchTerm}
          onSearchTermChange={onSearchTermChange} />

                        <button className="add-button" onClick={() => onNavigate("/admin/subscriptions/add")}>
                            <img src={icons.bPlus}/>
                            <p>
                                Add Subscription
                            </p>
                        </button>
        <div className="profile-icon">
          
          <img src={user?.imageUrl ? user.imageUrl : icons.bUser} className={`w-[43px] ${user?.imageUrl ? "bg-transparent" : "bg-[#FF642E]"} rounded-full`} />
    
          <p className="profile-name">{user?.firstName ?? "Unknown User"} {user?.lastName}</p>
        </div>

      </header>
      <main className="main-container">
        {subscriptions.length > 0 ? (
          <div className="flex flex-col w-full gap-2.5">
            <h1 className='font-semibold text-[#FF642E] text-2xl'>All Subscriptions</h1>
            <div className="flex flex-col gap-2.5">
              {subscriptions.map((subscription) => (
                <div className="flex flex-col">
                  <SubscriptionCardContainer
                    key={subscription.id}
                    subscription={subscription}
                    onDeleted={() => onDeleted(subscription.id)}
                  /> 
                </div>
              ))}
            </div>
          </div>
        ) : (
          <p>No subscriptions found.</p>
        )}
      </main>
      <footer>
        <div className="pagination-container">
          <Pagination
            pagination={pagination}
            onPageChange={onPageChange}
          />
        </div>
      </footer>
    </div>
    // <div className="subscription-list-wrapper">
    //   <p onClick={() => onNavigate('/admin/subscriptions/add')}>
    //     Add Subscription
    //   </p>
    //   <h1>Subscription List</h1>

    //   <Search searchTerm={searchTerm} onSearchTermChange={onSearchTermChange} />

    //   {loading ? (
    //     <Loading />
    //   ) : subscriptions.length > 0 ? (
    //     <List
    //       height={600}
    //       itemCount={subscriptions.length}
    //       itemSize={100}
    //       width={'100%'}
    //     >
    //       {Card}
    //     </List>
    //   ) : (
    //     <p>No subscriptions found.</p>
    //   )}

    //   <hr />

    //   <Pagination pagination={pagination} onPageChange={onPageChange} />
    // </div>
  );
};

export default SubscriptionList;
