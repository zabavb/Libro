import React from 'react';
import { Role, UserCard as UserCardType } from '../../types';
import { icons } from '@/lib/icons'
interface UserCardProps {
  user: UserCardType;
  onNavigate: () => void;
  onDelete: (e: React.MouseEvent) => void;
}

const UserCard: React.FC<UserCardProps> = ({ user, onNavigate, onDelete }) => {
  return (
    <>
      <tr onClick={(e) => {
        e.stopPropagation();
      }}>
        <td>
          <div className='flex gap-4 items-center'>
            {user.fullName}
            {user.role === Role.ADMIN && 
            <div className='text-[#FF642E] border-[#FF642E] border-[1px] rounded-lg py-2 px-2 text-sm'>
              Admin
            </div>}
            {user.role === Role.MODERATOR && <div className='text-[#007AFF] border-[#007AFF] border-[1px] rounded-lg py-2 px-2 text-sm'>
              Moderator
            </div>}
          </div>
        </td>
        <td>
          {user.email}
        </td>
        <td>
          {user.phoneNumber}
        </td>
        <td className='text-center'>
          {user.order.ordersCount}
        </td>
        <td>
          <div className='flex gap-2'>
            <button onClick={onNavigate} className='p-2.5 bg-[#FF642E] rounded-lg'><img src={icons.wPen} /></button>
            <button onClick={onDelete} className='p-2.5 border-[1px] border-[#FF642E] rounded-lg'><img src={icons.oTrash} /></button>
          </div>
        </td>
      </tr>
    </>
  );
};

export default UserCard;
