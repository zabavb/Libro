import React from 'react';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts';
import { useNavigate } from 'react-router-dom';
import '../Admin/AdminDashboardStyle.css';

const salesData = [
  { name: 'Січень', value: 400 },
  { name: 'Лютий', value: 300 },
  { name: 'Березень', value: 450 }
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
  'Книга 9'
];

const DashboardPage: React.FC = () => {
    const navigate = useNavigate();
    return (
        <div className="dashboard-container">

        <main className="main-content">
            <header className="dashboard-header">
            <input type="text" placeholder="Пошук" />
            <div className="user-info">Ім’я Прізвище</div>
            </header>

            <section className="dashboard-grid">
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
                    <ResponsiveContainer width="100%" height={200}>
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
                    <div className="listeners-box stat-box yellow">
                        <h4>Прослуховування</h4>
                        <p>784</p>
                    </div>

                    <div className="new-users-box stat-box dark">
                        <h4>Нові користувачі</h4>
                        <p>345</p>
                    </div>
                </div>
            </div>

            <div className="top-sales">
                <h3>Топ dsadsadsasaddпродажів</h3>
                <div className="top-sales-grid">
                    {topSales.map((title, index) => (
                    <div key={index} className="book-card">
                        <img src="https://via.placeholder.com/100x140.png?text=Book" alt={title} />
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
