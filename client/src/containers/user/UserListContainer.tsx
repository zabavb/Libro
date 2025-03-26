import { useEffect, useState, useCallback, useMemo } from 'react';
import { useNavigate } from 'react-router-dom';
import UserList from '../../components/user/UserList';
import { UserCard, UserFilter, UserSort } from '../../types';
import { fetchUsersService } from '../../services/userService';
import { useDispatch } from 'react-redux';
import { AppDispatch } from '../../state/redux';
import { addNotification } from '../../state/redux/slices/notificationSlice';

const UserListContainer = () => {
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();

  const [users, setUsers] = useState<UserCard[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [filters, setFilters] = useState<UserFilter>({});
  const [sort, setSort] = useState<UserSort>({});
  const [pagination, setPagination] = useState({
    pageNumber: 1,
    pageSize: 10,
    totalCount: 0,
  });

  const paginationMemo = useMemo(() => ({ ...pagination }), [pagination]);

  const fetchUserList = useCallback(async () => {
    setLoading(true);
    try {
      const response = await fetchUsersService(
        paginationMemo.pageNumber,
        paginationMemo.pageSize,
        searchTerm,
        filters,
        sort,
      );

      if (response.error)
        dispatch(
          addNotification({
            message: response.error,
            type: 'error',
          }),
        );

      if (response && response.data) {
        const paginatedData = response.data;

        setUsers(paginatedData.items);
        setPagination({
          pageNumber: paginatedData.pageNumber,
          pageSize: paginatedData.pageSize,
          totalCount: paginatedData.totalCount,
        });
      } else throw new Error('Invalid response structure');
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
  }, [paginationMemo, searchTerm, filters, sort, dispatch]);

  useEffect(() => {
    fetchUserList();
  }, [fetchUserList]);

  const handleNavigate = (path: string) => navigate(path);

  const handleSearchTermChange = (newSearchTerm: string) => {
    setSearchTerm(newSearchTerm);
    setPagination((prev) => ({ ...prev, pageNumber: 1 }));
  };

  const handleFilterChange = (newFilters: UserFilter) => {
    setFilters(newFilters);
    setPagination((prev) => ({ ...prev, pageNumber: 1 }));
  };

  const handleSortChange = (field: keyof UserSort) => {
    setSort({ [field]: true });
    setPagination((prev) => ({ ...prev, pageNumber: 1 }));
  };

  const handlePageChange = (pageNumber: number) => {
    setPagination((prev) => ({ ...prev, pageNumber }));
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
      filters={filters}
      onSortChange={handleSortChange}
      sort={sort}
    />
  );
};

export default UserListContainer;
