import { useCallback, useEffect, useMemo, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import UserList from '../../components/user/UserList';
import {
  Bool,
  UserCard,
  UserFilter,
  UserSort,
  UserViewFilter,
} from '../../types';
import { fetchUsersService } from '../../services/userService';
import { useDispatch } from 'react-redux';
import { AppDispatch } from '../../state/redux';
import { addNotification } from '../../state/redux/slices/notificationSlice';
import {
  FromUserFilterToUserViewFilter,
  FromUserViewFilterToUserFilter,
} from '@/api/adapters/userAdapter';

const UserListContainer: React.FC = () => {
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();

  const [users, setUsers] = useState<UserCard[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [filters, setFilters] = useState<UserFilter>({});
  const [sort, setSort] = useState<UserSort>({
    alphabetical: Bool.NULL,
    youngest: Bool.NULL,
    roleSort: Bool.NULL,
  });
  const [pagination, setPagination] = useState({
    pageNumber: 1,
    pageSize: 10,
    totalCount: 0,
  });

  const paginationMemo = useMemo(() => ({ ...pagination }), [pagination]);

  const fetchUserList = useCallback(async () => {
    (async () => {
      setLoading(true);
      try {
        const response = await fetchUsersService(
          paginationMemo.pageNumber,
          paginationMemo.pageSize,
          searchTerm,
          filters,
          sort,
        );

        if (response.data) {
          const paginatedData = response.data;

          setUsers((prev) =>
            JSON.stringify(prev) === JSON.stringify(paginatedData.items)
              ? prev
              : paginatedData.items,
          );

          setPagination((prev) => {
            if (
              prev.pageNumber === paginatedData.pageNumber &&
              prev.pageSize === paginatedData.pageSize &&
              prev.totalCount === paginatedData.totalCount
            ) {
              return prev;
            }
            return {
              pageNumber: paginatedData.pageNumber,
              pageSize: paginatedData.pageSize,
              totalCount: paginatedData.totalCount,
            };
          });
        } else if (response.error) throw Error(response.error);
      } catch (error) {
        dispatch(
          addNotification({
            message: error instanceof Error ? error.message : String(error),
            type: 'error',
          }),
        );
        setUsers([]);
      }
      setLoading(false);
    })();
  }, [paginationMemo, searchTerm, filters, sort, dispatch]);

  useEffect(() => {
    fetchUserList();
  }, [fetchUserList]);

  const handleNavigate = (path: string) => navigate(path);

  const handleSearchTermChange = (newSearchTerm: string) => {
    setSearchTerm(newSearchTerm);
    setPagination((prev) => ({ ...prev, pageNumber: 1 }));
  };

  const handleFilterChange = (viewFilters: UserViewFilter) => {
    const newFilters = FromUserViewFilterToUserFilter(viewFilters);

    setFilters(newFilters);
    setPagination((prev) => ({ ...prev, pageNumber: 1 }));
  };

  const handleSortChange = (newSorts: UserSort) => {
    setSort(newSorts);
    setPagination((prev) => ({ ...prev, pageNumber: 1 }));
  };

  const handlePageChange = (pageNumber: number) => {
    setPagination((prev) => ({ ...prev, pageNumber }));
  };

  const handleDeleted = (userId: string) => {
    setUsers((prev) => prev.filter((user) => user.id !== userId));
  };

  return (
    <UserList
      users={users}
      loading={loading}
      pagination={pagination}
      onPageChange={handlePageChange}
      onNavigate={handleNavigate}
      onSearchTermChange={handleSearchTermChange}
      searchTerm={searchTerm}
      onFilterChange={handleFilterChange}
      filters={FromUserFilterToUserViewFilter(filters)}
      onSortChange={handleSortChange}
      sort={sort}
      onDeleted={handleDeleted}
    />
  );
};

export default UserListContainer;
