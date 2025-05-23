import React, { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { UserFormData, userSchema } from '../../utils';
import { RoleView, User, UserForm as UserFormType } from '../../types';
import { dateToString } from '../../api/adapters/commonAdapters';
// import { numberToRoleView } from '../../api/adapters/userAdapter';
import { icons } from '@/lib/icons';
import '@/assets/styles/components/user/user-form.css';
import { getUserFromStorage } from '@/utils/storage';
import { useNavigate } from 'react-router-dom';
import { roleEnumToRoleView } from '@/api/adapters/userAdapter';

interface UserFormProps {
  existingUser?: UserFormType;
  onAddUser: (user: UserFormData) => Promise<void>;
  onEditUser: (
    existingUser: UserFormType,
    updatedUser: UserFormData,
  ) => Promise<void>;
  loading: boolean;
  onIsEdit: (isEdit: boolean) => void;
  isEdit: boolean;
}

const UserForm: React.FC<UserFormProps> = ({
  existingUser,
  onAddUser,
  onEditUser,
  loading,
  isEdit,
  onIsEdit,
}) => {
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<UserFormData>({
    resolver: zodResolver(userSchema),
    defaultValues: {
      firstName: '',
      lastName: '',
      dateOfBirth: dateToString(new Date(new Date().getFullYear() - 18)),
      email: '',
      phoneNumber: '',
      role: RoleView.USER,
    },
  });

  const navigate = useNavigate();
  const [localEdit, setLocalEdit] = useState<boolean>(isEdit);

  useEffect(() => {}, [isEdit, existingUser]);

  const loggedUser: User | null = getUserFromStorage();

  useEffect(() => {
    if (existingUser) {
      setLocalEdit(isEdit);
      /* setValue('lastName', existingUser.lastName ?? undefined);
      setValue('firstName', existingUser.firstName);
      setValue('email', existingUser.email ?? undefined);
      setValue('phoneNumber', existingUser.phoneNumber ?? undefined);
      setValue(
        'dateOfBirth',
        existingUser.dateOfBirth ? dateToString(existingUser.dateOfBirth) : '',
      );
      setValue('role', numberToRoleView(existingUser.role)); */
      reset({
        firstName: existingUser.firstName,
        lastName: existingUser.lastName ?? '',
        email: existingUser.email ?? '',
        phoneNumber: existingUser.phoneNumber ?? '',
        dateOfBirth: existingUser.dateOfBirth
          ? dateToString(existingUser.dateOfBirth)
          : '',
        role: roleEnumToRoleView(existingUser.role),
      });
    } else setLocalEdit(true);
  }, [existingUser, isEdit, reset]);

  const onSubmit = (data: UserFormData) => {
    if (existingUser) onEditUser(existingUser, data);
    else onAddUser(data);
  };

  const feedbacksHandler = (isExtend: boolean) => {
    if (isExtend) {
      const skipFirst = existingUser?.feedbacks.slice(1);
      if (skipFirst) {
        skipFirst.map((feedback) => (
          <div>
            <div>{feedback.headLabel}</div>
            <div>{feedback.rating}</div>
            <div>{feedback.comment}</div>
            <div>{dateToString(feedback.date)}</div>
          </div>
        ));
        <input onClick={() => feedbacksHandler}>Less</input>;
      }
    } else <input onClick={() => feedbacksHandler(true)}>More</input>;
  };

  return (
    <div>
      <header className='header-container'>
        <div className='flex gap-[60px] items-center'>
          <div className='flex gap-5 items-center'>
            <button
              className='form-back'
              onClick={() => navigate('/admin/users')}
            >
              <img src={icons.oArrowLeft} />
            </button>
            <h1 className='text-2xl font-semibold'>Users</h1>
          </div>
          {existingUser && (
            <button
              className={`form-edit-btn ${localEdit === true && 'edit-active'}`}
              onClick={() => {
                onIsEdit(!isEdit);
              }}
            >
              Edit
            </button>
          )}
        </div>
        <div className='profile-icon'>
          <div className='icon-container-pfp'>
            <img
              src={loggedUser?.imageUrl ? loggedUser.imageUrl : icons.bUser}
              className='panel-icon'
            />
          </div>
          <p className='profile-name'>
            {loggedUser?.firstName ?? 'Unknown User'} {loggedUser?.lastName}
          </p>
        </div>
      </header>
      <main className='flex px-[55px] py-4 gap-16'>
        <div className='flex flex-col w-[30%]'>
          <div className='flex flex-col gap-7'>
            <div className='user-profile-icon'>
              <div className='user-icon-container-pfp'>
                <img
                  src={
                    existingUser?.imageUrl ? existingUser.imageUrl : icons.bUser
                  }
                  className='user-icon'
                />
              </div>
              <p className='user-name'>
                {existingUser?.firstName ?? 'New User'} {existingUser?.lastName}
              </p>
            </div>
          </div>
          <div className='flex gap-16 w-full'>
            <form
              onSubmit={handleSubmit(onSubmit)}
              className='flex flex-col gap-4 w-full'
            >
              <div className='input-row'>
                <label className='text-sm'>Last name</label>
                <input
                  disabled={!localEdit}
                  {...register('lastName')}
                  className='input-field'
                  placeholder='Last Name'
                />
                <p>{errors.lastName?.message}</p>
              </div>
              <div className='input-row'>
                <label className='text-sm'>First name</label>
                <input
                  disabled={!localEdit}
                  {...register('firstName')}
                  className='input-field'
                  placeholder='First Name'
                />
                <p>{errors.firstName?.message}</p>
              </div>
              <div className='input-row'>
                <label className='text-sm'>E-mail</label>
                <input
                  disabled={!localEdit}
                  type='email'
                  {...register('email')}
                  className='input-field'
                  placeholder='Email'
                />
                <p>{errors.email?.message}</p>
              </div>
              <div className='input-row'>
                <label className='text-sm'>Phone number</label>
                <input
                  disabled={!localEdit}
                  className='input-field'
                  type='tel'
                  {...register('phoneNumber')}
                  placeholder='Phone Number'
                />
                <p>{errors.phoneNumber?.message}</p>
              </div>
              <div className='input-row'>
                <label className='text-sm'>Birth day</label>
                <input
                  disabled={!localEdit}
                  className='input-field'
                  type='date'
                  {...register('dateOfBirth')}
                  placeholder='Date of Birth'
                />
                <p>{errors.dateOfBirth?.message}</p>
              </div>
              <div className='input-row'>
                <label className='text-sm'>Role</label>
                <select
                  disabled={!localEdit}
                  className='input-field'
                  {...register('role')}
                >
                  <option value=''>Select Role</option>
                  {Object.entries(RoleView).map(([key, value]) => (
                    <option key={key} value={value}>
                      {value}
                    </option>
                  ))}
                </select>
                <p>{errors.role?.message}</p>
              </div>
              <button
                type='submit'
                disabled={loading}
                className='form-edit-btn fixed bottom-6 right-14'
              >
                {existingUser ? 'Save changes' : 'Add User'}
              </button>
            </form>
          </div>
        </div>
        <div className='flex flex-col gap-[33px] w-[50%]'>
          <div>
            <p className='font-semibold text-[#FF642E] text-2xl'>All orders</p>
            <table>
              <thead>
                <tr>
                  <th>Names</th>
                  <th>Uid</th>
                  <th>Price</th>
                </tr>
              </thead>
              <tbody></tbody>
              {existingUser &&
              existingUser.orders &&
              existingUser.orders.length > 0 ? (
                existingUser.orders.map((order) => (
                  <tr className='orders-row'>
                    <td>{order.bookNames}</td>
                    <td>{order.orderUiId}</td>
                    <td>{order.price}</td>
                  </tr>
                ))
              ) : (
                <tr className='orders-row'>
                  <td>No orders yet...</td>
                  <td></td>
                  <td></td>
                </tr>
              )}
            </table>
          </div>
          <div>
            <p className='text-2xl text-[#FF642E] font-semibold'>
              Feedbacks ({existingUser?.feedbacksCount ?? 0} psc.)
            </p>
            <div>
              {existingUser &&
              existingUser.feedbacksCount &&
              existingUser.feedbacksCount > 0 ? (
                existingUser.feedbacksCount === 1 ? (
                  <>
                    <div className='feedback-item'>
                      <div>{existingUser.feedbacks[0].rating}</div>
                      <div>{existingUser.feedbacks[0].comment}</div>
                      <div>{existingUser.feedbacks[0].headLabel}</div>
                      <div>{dateToString(existingUser.feedbacks[0].date)}</div>
                    </div>
                    <p className='font-semibold text-[#FF642E] text-[16px]'>
                      More
                    </p>
                  </>
                ) : (
                  <>{feedbacksHandler}</>
                )
              ) : (
                <div>No feedbacks yet...</div>
              )}
            </div>
          </div>
        </div>
      </main>
    </div>
    //   <div>
    //     {existingUser && existingUser.subscriptions.length > 0 ? (
    //       <>
    //         {existingUser?.subscriptions.map((subscription) => (
    //           <div key={subscription.title}>
    //             <img src={subscription.imageUrl} alt={subscription.title} />
    //             <div>{subscription.title}</div>
    //             <div>{subscription.description}</div>
    //           </div>
    //         ))}
    //       </>
    //     ) : (
    //       <p>No subscriptions yet...</p>
    //     )}
    //   </div>
    // </>
  );
};

export default UserForm;
