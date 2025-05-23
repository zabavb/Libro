// to delete
import React, { useCallback, useEffect, useState } from 'react';
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
import '../Admin/AdminDashboardStyle.css';
import { useAuth } from '@/state/context';
import { User } from '@/types';
import { useDispatch } from 'react-redux';
import { addNotification } from '@/state/redux/slices/notificationSlice';

const salesData = [
  { name: 'Січень', value: 400 },
  { name: 'Лютий', value: 300 },
  { name: 'Березень', value: 450 },
];

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

const DashboardPage: React.FC = () => {
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

  useEffect(() => {
      (async () => {
        // setLoading(true);
        await fetchUser();
        // setLoading(false);
      })();
    }, [fetchUser]);

  return (
    <div className='dashboard-container'>
      <main className='main-content'>
        <header className='dashboard-header'>
          <input type='text' placeholder='Пошук' />
          <div className='user-info'>{user.firstName} {user.lastName}</div>
        </header>

        <section className='dashboard-grid'>
          <div className='left-grid'>
            <div className='sales-chart'>
              <div className='chart-header'>
                <h3>Статистика продажів</h3>
                <select className='time-filter'>
                  <option>Щомісяця</option>
                  <option>Щотижня</option>
                  <option>Щодня</option>
                </select>
              </div>
              <ResponsiveContainer width='100%' height={200}>
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
              <div className='listeners-box stat-box yellow'>
                <h4>Прослуховування</h4>
                <p>784</p>
              </div>

              <div className='new-users-box stat-box dark'>
                <h4>Нові користувачі</h4>
                <p>345</p>
              </div>
            </div>
          </div>

          <div className='top-sales'>
            <h3>Топ продажів</h3>
            <div className='top-sales-grid'>
              {topSales.map((title, index) => (
                <div key={index} className='book-card'>
                  <img
                    src='https://via.placeholder.com/100x140.png?text=Book'
                    alt={title}
                  />
                  <p>{title}</p>
                </div>
              ))}
            </div>
          </div>
        </section>
      </main>
    </div>
  );
};

export default DashboardPage;
