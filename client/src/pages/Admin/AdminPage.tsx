import React, { useState, useEffect } from 'react';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts';
import { useNavigate } from 'react-router-dom';
import '../Admin/AdminDashboardStyle.css';
import { fetchActiveSubscriptionsCount } from '../../services/subscriptionService';


const topSales = [
  'Бріджертони',
  'Там де співають раки',
  'Приховані малюнки',
  'Наречена',
  'Озирайся і мовчи',
  'Квартира на двох',
  'Книга 7',
  'Книга 8',
  'Книга 9'
];


type PeriodType = 'day' | 'week' | 'month';

const AdminPage: React.FC = () => {
    const navigate = useNavigate();
    const [activeSubCount, setActiveSubCount] = useState<number>(0);
    const [period, setPeriod] = useState<PeriodType>('month');
    const [salesData, setSalesData] = useState<SalesDataItem[]>([]);
    const [lastUpdated, setLastUpdated] = useState<string>("");

    useEffect(() => {
        const loadActiveCount = async () => {
            try {
                const count = await fetchActiveSubscriptionsCount();
                setActiveSubCount(count);
                const response = await axios.get(`/api/statistics/orders/counts?period=${period}`);
            } catch (error) {
                console.error('Error fetching data:', error);
            }
        };
    
        loadActiveCount();
    }, []);
    return (
        <div className="dashboard-container">

        <main className="main-content">
            <header className="dashboard-header">
            <input type="text" placeholder="Пошук" />
            <div className="user-info">Ім’я Прізвище</div>
            </header>

            <div className="dashboard-grid">
            <div className="left-grid">
                <div className="sales-chart">
                    <div className='chart-header'>
                        <h3>Статистика продажів</h3>
                        <select className="time-filter">
                            <option>Щомісяця</option>
                            <option>Щотижня</option>
                            <option>Щодня</option>
                        </select>
                    </div>
                    <ResponsiveContainer width="100%" height="100%">
                    <BarChart data={salesData}>
                        <CartesianGrid strokeDasharray="3 3" />
                        <XAxis dataKey="name" />
                        <YAxis />
                        <Tooltip />
                        <Bar dataKey="value" fill="#ff6534" />
                    </BarChart>
                    </ResponsiveContainer>
                </div>
                <div className="bottom-grid">
                    <div className="listeners-box stat-box">
                        <h4>Активні підписники</h4>
                        <p>{activeSubCount}</p>
                    </div>

                    <div className="new-users-box stat-box">
                        <h4>Нові користувачі</h4>
                        <p>345</p>
                    </div>
                </div>
            </div>

            <div className="top-sales">
                <h3>Топ продажів</h3>
                <div className="top-sales-grid">
                    {topSales.map(( index) => (
                    <div key={index} className="book-card">
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
