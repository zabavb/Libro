import React from 'react';
import { DeliveryType, User } from '../../../types';
import DeliveryTypeAdminCardContainer from '../../../containers/order/DeliveryTypeAdminCardContainer';
import Pagination from '../../common/Pagination';
import Search from '../../common/Search';
import DeliverySort from '../DeliverySort';
import '@/assets/styles/base/table-styles.css';
import '@/assets/styles/components/list-styles.css';
import { icons } from '@/lib/icons';
import { getUserFromStorage } from '@/utils/storage';
interface DeliveryTypeListProps {
  deliveryTypes?: DeliveryType[];
  loading: boolean;
  pagination: { pageNumber: number; pageSize: number; totalCount: number };
  onPageChange: (pageNumber: number) => void;
  onNavigate: (path: string) => void;
  onSortChange: (field: keyof DeliverySort) => void;
  sort: DeliverySort;
  onSearchTermChange: (searchTerm: string) => void;
  searchTerm: string;
  handleNavigate: (path: string) => void;
}

const DeliveryTypeList: React.FC<DeliveryTypeListProps> = ({
  deliveryTypes = [],
  loading,
  pagination,
  onPageChange,
  searchTerm,
  onSearchTermChange,
  handleNavigate,
}) => {
  const user: User | null = getUserFromStorage();
  if (loading) return <p>Loading...</p>;
  return (
    <div>
      <header className='header-container'>
        <Search
          searchTerm={searchTerm}
          onSearchTermChange={onSearchTermChange}
        />
        <button
          className='add-button'
          onClick={() => handleNavigate('/admin/delivery/add')}
        >
          <img src={icons.bPlus} />
          <p>Add Delivery</p>
        </button>
        <div className='profile-icon'>
          <div className='icon-container-pfp'>
            <img
              src={user?.imageUrl ? user.imageUrl : icons.bUser}
              className='panel-icon'
            />
          </div>
          <p className='profile-name'>
            {user?.firstName ?? 'Unknown User'} {user?.lastName}
          </p>
        </div>
      </header>
      <main className='main-container'>
        {deliveryTypes.length > 0 ? (
          <div className='flex flex-col w-full'>
            <div className='flex flex-row-reverse'>
              <p className='counter'>
                All delivery types ({pagination.totalCount})
              </p>
            </div>
            <div className='table-wrapper'>
              <table>
                <thead className='m-5'>
                  <tr>
                    <th style={{ width: '50%' }} className='table-header'>
                      Service name
                    </th>
                    <th style={{ width: '40%' }} className='table-header'>
                      Delivery
                    </th>
                    <th style={{ width: '10%' }} className='table-header'></th>
                  </tr>
                </thead>
                {loading ? (
                  <tr>
                    <td
                      colSpan={5}
                      style={{
                        textAlign: 'center',
                        height: `${deliveryTypes.length * 65}px`,
                      }}
                    >
                      Loading...
                    </td>
                  </tr>
                ) : (
                  <tbody>
                    {deliveryTypes.map((deliveries) => (
                      <DeliveryTypeAdminCardContainer
                        key={deliveries.id}
                        deliveryType={deliveries}
                      />
                    ))}
                  </tbody>
                )}
              </table>
            </div>
          </div>
        ) : (
          <p>No delivery types found.</p>
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

export default DeliveryTypeList;
