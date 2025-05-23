import { useDispatch } from 'react-redux';
import { AppDispatch } from '../../state/redux';
import { useNavigate } from 'react-router-dom';
import { useCallback, useEffect, useMemo, useState } from 'react';
import { addNotification } from '../../state/redux/slices/notificationSlice';
import { SubscriptionCard } from '../../types';
import { fetchSubscriptionsService } from '../../services/subscriptionService';
import SubscriptionList from '../../components/user/SubscriptionList';

const SubscriptionListContainer: React.FC = () => {
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();

  const [subscriptions, setSubscriptions] = useState<SubscriptionCard[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [pagination, setPagination] = useState({
    pageNumber: 1,
    pageSize: 10,
    totalCount: 0,
  });

  const paginationMemo = useMemo(() => ({ ...pagination }), [pagination]);

  const fetchSubscriptionList = useCallback(async () => {
    (async () => {
      setLoading(true);
      try {
        const response = await fetchSubscriptionsService(
          (paginationMemo.pageNumber = 1),
          (paginationMemo.pageSize = 10),
          searchTerm,
        );

        if (response.data) {
          const paginatedData = response.data;

          setSubscriptions((prev) =>
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
        setSubscriptions([]);
      }
      setLoading(false);
    })();
  }, [paginationMemo, searchTerm, dispatch]);

  useEffect(() => {
    fetchSubscriptionList();
  }, [fetchSubscriptionList]);

  const handleNavigate = (path: string) => navigate(path);

  const handleSearchTermChange = (newSearchTerm: string) => {
    setSearchTerm(newSearchTerm);
    setPagination((prev) => ({ ...prev, pageNumber: 1 }));
  };

  const handlePageChange = (pageNumber: number) => {
    setPagination((prev) => ({ ...prev, pageNumber }));
  };

  const handleDeleted = (subscriptionId: string) => {
    setSubscriptions((prev) => prev.filter((subscription) => subscription.id !== subscriptionId));
  };

  return (
    <SubscriptionList
      subscriptions={subscriptions}
      loading={loading}
      pagination={pagination}
      onPageChange={handlePageChange}
      onNavigate={handleNavigate}
      onSearchTermChange={handleSearchTermChange}
      searchTerm={searchTerm}
      onDeleted={handleDeleted}
    />
  );
};

export default SubscriptionListContainer;
