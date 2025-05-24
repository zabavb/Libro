import React, { useEffect, useState } from 'react';
import '@/assets/styles/layout/user-panel.css';
import closeIcon from '@/assets/icons/menuClose.svg';
import { useNavigate } from 'react-router-dom';
import { icons } from '@/lib/icons';
import { getUserFromStorage } from '@/utils/storage';
import { User } from '@/types';
import { useAuth } from '@/state/context';

interface UserPanelProps {
  onLoginOpen?: () => void;
}

const censorPhonenumber = (phoneNumber: string | null | undefined) => {
  if (!phoneNumber) return '';

  return `+${phoneNumber.slice(0,2)} ******* ${phoneNumber.slice(-2)}`;
};

const UserPanel: React.FC<UserPanelProps> = ({ onLoginOpen }) => {
  const { logout } = useAuth();
  const navigate = useNavigate();
  const [isOpen, setIsOpen] = useState<boolean>(false);
  const user: User | null = getUserFromStorage();

  useEffect(() => {
    if (isOpen) {
      document.body.style.overflow = 'hidden';
    } else {
      document.body.style.overflow = '';
    }

    return () => {
      document.body.style.overflow = '';
    };
  }, [isOpen]);

  // Opens login instead of profile panel
  if (isOpen && !user && onLoginOpen) {
    setIsOpen(false);
    onLoginOpen();
  }

  const handleLogout = () => {
    logout();
    setIsOpen(false);
  };

  return (
    <div>
      <img
        src={icons.bUser}
        className='cursor-pointer'
        onClick={() => setIsOpen(true)}
      />

      <div
        className={`dim ${isOpen && 'visible'}`}
        aria-hidden={!isOpen}
        onClick={() => setIsOpen(false)}
      />
      <div className={`user-panel ${isOpen ? 'right-0' : 'right-[-445px]'}`}>
        <div className='p-[30px] bg-[#1A1D23] h-[17%] min-h-[170px] flex flex-col gap-[23px]'>
          <div>
            <img
              src={closeIcon}
              className='close-icon'
              onClick={() => setIsOpen(false)}
            />
            <h1 className='font-semibold text-2xl'>Profile</h1>
          </div>
          <div className='flex gap-[23px]'>
            <div className='icon-container-pfp'>
              <img
                src={user?.imageUrl ? user.imageUrl : icons.bUser}
                className='panel-icon'
              />
            </div>
            <div>
              <div className='flex flex-col gap-1'>
                <p>
                  {user?.firstName} {user?.lastName}
                </p>
                <p>
                  {user?.phoneNumber
                    ? censorPhonenumber(user?.phoneNumber)
                    : 'Unknown number'}
                </p>
              </div>
            </div>
          </div>
        </div>
        <div className='user-main'>
          <div className='panel-row' onClick={() => navigate('/orders')}>
            <div className='icon-container'>
              <img className='panel-icon' src={icons.wTruck} />
            </div>
            <p>Orders</p>
          </div>
          <div className='panel-row' onClick={() => navigate('/library')}>
            <div className='icon-container'>
              <img className='panel-icon' src={icons.wBookmark} />
            </div>
            <p>Library</p>
          </div>
          <div className='panel-row'>
            <div className='icon-container'>
              <img className='panel-icon invert' src={icons.bHeart} />
            </div>
            <p>Favorites</p>
          </div>
          <div className='panel-row'>
            <div className='icon-container'>
              <img className='panel-icon' src={icons.wBonus} />
            </div>
            <p>Bonuses</p>
          </div>
          <div className='panel-row' onClick={() => navigate('/profile')}>
            <div className='icon-container'>
              <img className='panel-icon' src={icons.wGear} />
            </div>
            <p>Settings</p>
          </div>
          <div className='panel-row' onClick={() => handleLogout()}>
            <div className='icon-container'>
              <img className='panel-icon' src={icons.wEnter} />
            </div>
            <p>Log out</p>
          </div>
        </div>
        <div className='user-footer min-h-[160px]'>
          <p className='text-gray'>Got any questions?</p>
          <h1 className='contact-info'>0-800-***-***</h1>
        </div>
      </div>
    </div>
  );
};

export default UserPanel;
