import React, { useState, useEffect, useCallback } from 'react';
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
} from 'recharts';
import { useNavigate } from 'react-router-dom';
import '../Admin/AdminPageStyle.css';
import { fetchActiveSubscriptionsCount } from '../../services/subscriptionService';
import { PeriodType } from '../../types/types/order/PeriodType';
// import { getOrderCounts } from '../../api/repositories/orderRepository';
import { useDispatch } from 'react-redux';
import { useAuth } from '@/state/context';
import { User } from '@/types';
import { addNotification } from '@/state/redux/slices/notificationSlice';
import { icons } from '@/lib/icons';

const topSales = [
  'Бріджертони',
  'Там де співають раки',
  'Приховані малюнки',
  'Наречена',
  'Озирайся і мовчи',
  'Квартира на двох',
  'Книга 7',
  'Книга 8',
  'Книга 9',
];

type SalesDataItem = {
  name: string;
  value: number;
};

const AdminPage: React.FC = () => {
  const [activeSubCount, setActiveSubCount] = useState<number>(0);
  const [period, setPeriod] = useState<PeriodType>('month');
  const [salesData, setSalesData] = useState<SalesDataItem[]>([]);
  //const [lastUpdated, setLastUpdated] = useState<string>("");
  const { token } = useAuth();
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const [user, setUser] = useState<User>({} as User);
  // const [loading, setLoading] = useState<boolean>(false);

  const handleMessage = useCallback(
    (message: string, type: 'success' | 'error') => {
      dispatch(addNotification({ message, type }));
    },
    [dispatch],
  );

  const handleNavigate = useCallback(
    (route: string) => navigate(route),
    [navigate],
  );

  const fetchUser = useCallback(async (): Promise<void> => {
    const data = JSON.parse(localStorage.getItem('user') || '{}') as User;
    if (token && data?.id) setUser(data);
    else {
      handleMessage('You are not authorized!', 'error');
      handleNavigate('/login');
    }
  }, [token, handleMessage, handleNavigate]);

  /* const censorPhonenumber = (phoneNumber: string | null | undefined) => {
    if (!phoneNumber) return '';

    return `+${phoneNumber.split('-')[0]} ******* ${phoneNumber.split('-')[2].slice(-2)}`;
  }; */

  useEffect(() => {
    (async () => {
      // setLoading(true);
      await fetchUser();
      // setLoading(false);
    })();
  }, [fetchUser]);

  useEffect(() => {
    // Fetch order statistics

    const fetchData = async () => {
      try {
        //const counts = await getOrderCounts(period);
        const mockCounts = [200, 140, 180];
        const labels =
          period === 'day'
            ? ['Позавчора', 'Вчора', 'Сьогодні']
            : period === 'week'
              ? ['2 тижні тому', 'Минулого тижня', 'Цього тижня']
              : ['2 міс. тому', 'Минулого місяця', 'Цього місяця'];
        const formattedData = mockCounts.map(
          (value: number, index: number) => ({
            name: labels[index],
            value,
          }),
        );
        setSalesData(formattedData);
      } catch (error) {
        console.error('Error fetching order statistics:', error);
      }
    };

    // Fetch active subscriptions count
    const loadActiveCount = async () => {
      try {
        const count = await fetchActiveSubscriptionsCount();
        setActiveSubCount(count);
      } catch (error) {
        console.error('Error fetching data:', error);
      }
    };

    fetchData();
    loadActiveCount();
  }, [period]);
  return (
    <div className='dashboard-container'>
      <main className='main-content'>
        <header className='dashboard-header'>
          <input type='text' placeholder='Пошук' />
          {/* <div className='user-info'>
            <img src={user.imageUrl} alt='User Avatar' />
            <p>{user.firstName} {user.lastName}</p>
          </div> */}

          <div className='profile-icon'>
            <img src={user?.imageUrl ? user.imageUrl : icons.bUser} className={`w-[43px] ${user?.imageUrl ? "bg-transparent" : "bg-[#FF642E]"} rounded-full`} />

            <p className='profile-name'>
              {user?.firstName ?? 'Unknown User'} {user?.lastName}
            </p>
          </div>
        </header>

        <div className='dashboard-grid'>
          <div className='left-grid'>
            <div className='sales-chart'>
              <div className='chart-header'>
                <h3>Статистика продажів</h3>
                <select
                  className='time-filter'
                  value={period}
                  onChange={(e) => setPeriod(e.target.value as PeriodType)}
                >
                  <option value='month'>Щомісяця</option>
                  <option value='week'>Щотижня</option>
                  <option value='day'>Щодня</option>
                </select>
              </div>
              <ResponsiveContainer width='100%' height='100%'>
                <BarChart data={salesData}>
                  <CartesianGrid strokeDasharray='3 3' />
                  <XAxis dataKey='name' />
                  <YAxis />
                  <Tooltip />
                  <Bar dataKey='value' fill='#ff6534' />
                </BarChart>
              </ResponsiveContainer>
            </div>
            <div className='bottom-grid'>
              <div className='listeners-box stat-box'>
                <h4>Активні підписники</h4>
                <p>{activeSubCount}</p>
              </div>

              <div className='new-users-box stat-box'>
                <h4>Нові користувачі</h4>
                <p>345</p>
              </div>
            </div>
          </div>

          <div className='top-sales'>
            <h3>Топ продажів</h3>
            <div className='top-sales-grid'>
              {topSales.map((index) => (
                <div key={index} className='book-card'>
                  <img src='/image.png' />
                </div>
              ))}
            </div>
          </div>
        </div>
      </main>
    </div>
  );
};

export default AdminPage;
