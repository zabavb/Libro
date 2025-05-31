import { getUserFromStorage } from '@/utils/storage';
import OrderAdminCardContainer from '../../../containers/order/OrderAdminCardContainer';
import { OrderFilter, OrderSort, Status, User } from '../../../types';
import Pagination from '../../common/Pagination';
import Search from '../../common/Search';
import '@/assets/styles/base/table-styles.css';
import '@/assets/styles/components/list-styles.css';
import { icons } from '@/lib/icons';
import { OrderWithUserName } from '@/types/types/order/OrderWithUserName';
interface OrderListProps {
  orders?: OrderWithUserName[];
  loading: boolean;
  pagination: { pageNumber: number; pageSize: number; totalCount: number };
  onPageChange: (pageNumber: number) => void;
  onNavigate: (path: string) => void;
  onSearchTermChange: (searchTerm: string) => void;
  searchTerm: string;
  onFilterChange: (filters: OrderFilter) => void;
  filters: OrderFilter;
  onSortChange: (sort: OrderSort) => void;
  sort: OrderSort;
}

const OrderList: React.FC<OrderListProps> = ({
  orders = [],
  loading,
  pagination,
  onPageChange,
  searchTerm,
  onSearchTermChange,
  onFilterChange,
  filters,
  onSortChange,
  sort,
}) => {
  const user: User | null = getUserFromStorage();

  return (
    <div>
      <header className='header-container'>
        <Search
          searchTerm={searchTerm}
          onSearchTermChange={onSearchTermChange}
        />
        <select
          value={filters.status || ''}
          onChange={(e) =>
            onFilterChange({ ...filters, status: e.target.value as Status })
          }
          className='ml-4'
        >
          <option value=''>All statuses</option>
          <option value='pending'>Pending</option>
          <option value='shipped'>Shipped</option>
          <option value='delivered'>Delivered</option>
          <option value='cancelled'>Cancelled</option>
        </select>

        {/* Сортування за ціною
        <select
          value={sort.orderPrice ? 'price' : ''}
          onChange={(e) =>
            onSortChange(e.target.value === 'price' ? { orderPrice: true } : {})
          }
          className='ml-4'
        >
          <option value=''>Default sort</option>
          <option value='price'>Sort by Price</option>
        </select> */}
        <div className='profile-icon'>
          <img src={user?.imageUrl ? user.imageUrl : icons.bUser} className={`w-[43px] ${user?.imageUrl ? "bg-transparent" : "bg-[#FF642E]"} rounded-full`} />

          <p className='profile-name'>
            {user?.firstName ?? 'Unknown User'} {user?.lastName}
          </p>
        </div>
      </header>
      <main className='main-container'>
        {orders.length > 0 ? (
          <div className='flex flex-col w-full'>
            <div className='flex flex-row-reverse'>
              <p className='counter'>Found orders ({pagination.totalCount})</p>
            </div>
            <div className='table-wrapper'>
              <table>
                <thead className='m-5'>
                  <tr>
                    <th style={{ width: '30%' }} className='table-header'>
                      Name and Surname
                    </th>
                    <th style={{ width: '25%' }} className='table-header'>
                      Order
                    </th>
                    <th style={{ width: '10%' }} className='table-header'>
                      Price
                    </th>
                    <th
                      style={{ width: '15%' }}
                      className='table-header text-center'
                    >
                      Status
                    </th>
                    <th style={{ width: '10%' }} className='table-header'></th>
                  </tr>
                </thead>
                {loading ? (
                  <tbody>
                    <tr>
                      <td
                        colSpan={5}
                        style={{
                          textAlign: 'center',
                          height: `${orders.length * 65}px`,
                        }}
                      >
                        Loading...
                      </td>
                    </tr>
                  </tbody>
                ) : (
                  <tbody>
                    {orders.map((order) => (
                      <OrderAdminCardContainer key={order.orderUiId} order={order} />
                    ))}
                  </tbody>
                )}
              </table>
            </div>
          </div>
        ) : loading ? (
          <p>Loading...</p>
        ) : (
          <p>No orders found.</p>
        )}
      </main>
      <footer>
        <div className='pagination-container'>
          <Pagination pagination={pagination} onPageChange={onPageChange} />
        </div>
      </footer>
    </div>
  );
};

export default OrderList;
