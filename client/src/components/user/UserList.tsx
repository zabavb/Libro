import React from 'react';
import { FixedSizeList as List } from 'react-window';
import {
  UserCard,
  UserSort as UserSortType,
  UserViewFilter,
} from '../../types';
import UserCardContainer from '../../containers/user/UserCardContainer';
import UserFilter from './UserFilter';
import UserSort from './UserSort';
import Pagination from '../common/Pagination';
import Search from '../common/Search';
import Loading from '../common/Loading';

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
}) => {
  const Card = ({
    index,
    style,
  }: {
    index: number;
    style: React.CSSProperties;
  }) => (
    <div style={style}>
      <UserCardContainer user={users[index]} />
    </div>
  );

  return (
    <div>
      <p onClick={() => onNavigate('/admin')}>Back to Admin Dashboard</p>
      <p onClick={() => onNavigate('/admin/users/add')}>Add User</p>
      <h1>User List</h1>

      <Search searchTerm={searchTerm} onSearchTermChange={onSearchTermChange} />
      <UserFilter filters={filters} onFilterChange={onFilterChange} />
      <UserSort sort={sort} onSortChange={onSortChange} />

      {loading ? (
        <Loading />
      ) : users.length > 0 ? (
        <List
          height={600}
          itemCount={users.length}
          itemSize={100}
          width={'100%'}
        >
          {Card}
        </List>
      ) : (
        <p>No users found.</p>
      )}

      <hr />

      <Pagination pagination={pagination} onPageChange={onPageChange} />
    </div>
  );
};

export default UserList;
