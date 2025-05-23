import React from 'react';
import {
  User,
  UserCard,
  UserSort as UserSortType,
  UserViewFilter,
} from '../../types';
import UserCardContainer from '../../containers/user/UserCardContainer';
import UserFilter from './UserFilter';
import UserSort from './UserSort';
import Pagination from '../common/Pagination';
import Search from '../common/Search';
import '@/assets/styles/base/table-styles.css';
import '@/assets/styles/components/list-styles.css';
import { getUserFromStorage } from '@/utils/storage';
import { icons } from '@/lib/icons';
interface UserListProps {
  users?: UserCard[];
  loading: boolean;
  pagination: { pageNumber: number; pageSize: number; totalCount: number };
  onPageChange: (pageNumber: number) => void;
  onNavigate: (path: string) => void;
  onSearchTermChange: (searchTerm: string) => void;
  searchTerm: string;
  onFilterChange: (filters: UserViewFilter) => void;
  filters: UserViewFilter;
  onSortChange: (sorts: UserSortType) => void;
  sort: UserSort;
  onDeleted: (userId: string) => void;
}

const UserList: React.FC<UserListProps> = ({
  users = [],
  loading,
  pagination,
  onPageChange,
  searchTerm,
  onSearchTermChange,
  filters,
  onFilterChange,
  sort,
  onSortChange,
  onNavigate,
  onDeleted,
}) => {
  const user: User | null = getUserFromStorage();

  const Message = (text: string) => (
    <tr>
      <td
        colSpan={5}
        style={{
          textAlign: 'center',
          height: `${users.length * 65}px`,
        }}
      >
        {text}
      </td>
    </tr>
  );

  return (
    <div>
      <header className='header-container'>
        <Search
          searchTerm={searchTerm}
          onSearchTermChange={onSearchTermChange}
        />
        <button
          className='add-button'
          onClick={() => onNavigate('/admin/users/add')}
        >
          <img src={icons.bPlus} />
          <p>Add User</p>
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
        <div className='flex flex-col w-full'>
          <div className='flex gap-[22px]'>
            <UserFilter onFilterChange={onFilterChange} filters={filters} />
            <UserSort onSortChange={onSortChange} sort={sort} />
          </div>
          <div className='flex flex-row-reverse'>
            <p className='counter'>All users ({pagination.totalCount})</p>
          </div>
          <div className='table-wrapper'>
            <table>
              <thead className='m-5'>
                <tr>
                  <th style={{ width: '30%' }} className='table-header'>
                    Name and Surname
                  </th>
                  <th style={{ width: '20%' }} className='table-header'>
                    E-mail
                  </th>
                  <th style={{ width: '15%' }} className='table-header'>
                    Phone Number
                  </th>
                  <th
                    style={{ width: '15%' }}
                    className='table-header text-center'
                  >
                    Order amnt
                  </th>
                  <th style={{ width: '10%' }} className='table-header'></th>
                </tr>
              </thead>
              {loading ? (
                Message('Loading...')
              ) : users.length > 0 ? (
                <tbody>
                  {users.map((user) => (
                    <UserCardContainer
                      key={user.id}
                      user={user}
                      onDeleted={() => onDeleted(user.id)}
                    />
                  ))}
                </tbody>
              ) : (
                Message('No users found')
              )}
            </table>
          </div>
        </div>
      </main>
      <footer>
        <div className='pagination-container'>
          <Pagination pagination={pagination} onPageChange={onPageChange} />
        </div>
      </footer>
    </div>
  );
};

export default UserList;
