import { Outlet, useLocation } from 'react-router-dom';
import '@/assets/styles/layout/admin-layout.css';
import { icons } from '@/lib/icons'
import logoUrl from '@/assets/logoLibro.svg';

const AdminLayout: React.FC = () => {
  const location = useLocation();
  const currentPath = location.pathname;

  const isActive = (path: string) => currentPath === path;

  return (
    <div className='admin-container'>
      {/* Sidebar */}
      <aside className='admin-sidebar'>
        <nav>
          <ul className='admin-nav-list'>
            <li className='logo-container'>
              <img src={logoUrl} />
            </li>
            <li className={isActive('/admin') ? 'active-link link' : 'link'}>
              <img
                src={icons.wDiagram}
                className={isActive('/admin') ? 'invert' : ''}
              />
              <a href='/admin'>Home</a>
            </li>
            <li
              className={isActive('/admin/books') ? 'active-link link' : 'link'}
            >
              <img
                src={icons.wBook}
                className={isActive('/admin/books') ? 'invert' : ''}
              />
              <a href='/admin/books'>Books</a>
            </li>
            <li
              className={isActive('/admin/users') ? 'active-link link' : 'link'}
            >
              <img
                src={icons.wUser}
                className={isActive('/admin/users') ? 'invert' : ''}
              />
              <a href='/admin/users'>Users</a>
            </li>
            <li
              className={
                isActive('/admin/orders') ? 'active-link link' : 'link'
              }
            >
              <img
                src={icons.wFile}
                className={isActive('/admin/orders') ? 'invert' : ''}
              />
              <a href='/admin/orders'>Orders</a>
            </li>
            <li
              className={
                isActive('/admin/delivery') ? 'active-link link' : 'link'
              }
            >
                <img
                src={icons.wTruck}
                className={isActive('/admin/delivery') ? 'invert' : ''}
              />
              <a href='/admin/delivery'>Deliveries</a>
            </li>
            <li
              className={
                isActive('/admin/subscriptions') ? 'active-link link' : 'link'
              }
            >
              <img
                src={icons.wCredit}
                className={isActive('/admin/subscriptions') ? 'invert' : ''}
              />
              <a href='/admin/subscriptions'>Subscriptions</a>
            </li>
          </ul>
          <div className='link logout-item'>
            <img src={icons.wLogout} />

            <a>Logout</a>
          </div>
        </nav>
      </aside>

      {/* Main Content Area */}
      <div style={{ display: 'flex', flexDirection: 'column', flex: 1 }}>
        {/* Header */}
        {/* <header style={{ background: "#333", color: "#fff", padding: "15px", textAlign: "center" }}>
					<h1>Admin Panel</h1>
				</header> */}
        <header></header>
        {/* Main Content */}
        <main style={{ flex: 1, overflowY: 'auto' }}>
          <Outlet />
        </main>
      </div>
    </div>
  );
};

export default AdminLayout;
