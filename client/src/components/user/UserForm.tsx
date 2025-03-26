import React, { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { UserFormData, userSchema } from '../../utils';
import { Role, UserForm as UserFormType } from '../../types';
import { dateToString } from '../../api/adapters/commonAdapters';
import { roleNumberToEnum } from '../../api/adapters/userAdapter';

interface UserFormProps {
  existingUser?: UserFormType;
  onAddUser: (user: UserFormData) => Promise<void>;
  onEditUser: (
    existingUser: UserFormType,
    updatedUser: UserFormData,
  ) => Promise<void>;
  loading: boolean;
}

const UserForm: React.FC<UserFormProps> = ({
  existingUser,
  onAddUser,
  onEditUser,
  loading,
}) => {
  const {
    register,
    handleSubmit,
    setValue,
    formState: { errors },
  } = useForm<UserFormData>({
    resolver: zodResolver(userSchema),
    defaultValues: {
      firstName: '',
      lastName: '',
      dateOfBirth: dateToString(new Date(new Date().getFullYear() - 18)),
      email: '',
      phoneNumber: '',
      role: Role.USER,
    },
  });

  useEffect(() => {
    if (existingUser) {
      setValue('lastName', existingUser.lastName ?? undefined);
      setValue('firstName', existingUser.firstName);
      setValue('email', existingUser.email ?? undefined);
      setValue('phoneNumber', existingUser.phoneNumber ?? undefined);
      setValue(
        'dateOfBirth',
        existingUser.dateOfBirth ? dateToString(existingUser.dateOfBirth) : '',
      );
      setValue('role', roleNumberToEnum(existingUser.role));
    }
  }, [existingUser, setValue]);

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
    <>
      {existingUser && (
        <div>
          <img src={existingUser.imageUrl ?? ''} alt={existingUser.firstName} />
          {existingUser.lastName} {existingUser.firstName}
        </div>
      )}

      <div>
        <form onSubmit={handleSubmit(onSubmit)}>
          <input {...register('lastName')} placeholder='Last Name' />
          <p>{errors.lastName?.message}</p>

          <input {...register('firstName')} placeholder='First Name' />
          <p>{errors.firstName?.message}</p>

          <input type='email' {...register('email')} placeholder='Email' />
          <p>{errors.email?.message}</p>

          <input
            type='tel'
            {...register('phoneNumber')}
            placeholder='Phone Number'
          />
          <p>{errors.phoneNumber?.message}</p>

          <input
            type='date'
            {...register('dateOfBirth')}
            placeholder='Date of Birth'
          />
          <p>{errors.dateOfBirth?.message}</p>

          <select {...register('role')}>
            <option value=''>Select Role</option>
            {Object.entries(Role).map(([key, value]) => (
              <option key={key} value={value}>
                {value}
              </option>
            ))}
          </select>
          <p>{errors.role?.message}</p>

          <button type='submit' disabled={loading}>
            {existingUser ? 'Update User' : 'Add User'}
          </button>
        </form>
      </div>

      {existingUser && (
        <>
          <p>All orders</p>
          <div>
            {existingUser.orders ? (
              existingUser.orders.map((order) => (
                <div>
                  <div>{order.bookNames}</div>
                  <div>{order.orderUiId}</div>
                  <div>{order.price}</div>
                </div>
              ))
            ) : (
              <p>No orders yet...</p>
            )}
          </div>
        </>
      )}

      {existingUser && (
        <div>
          {existingUser.feedbacksCount === 0 ? (
            <p>No feedbacks yet...</p>
          ) : (
            <>
              <p>Feedbacks ({existingUser.feedbacksCount})</p>
              <div>
                {existingUser.feedbacksCount === 1 ? (
                  <>
                    <div>{existingUser.feedbacks[0].rating}</div>
                    <div>{existingUser.feedbacks[0].comment}</div>
                    <div>{existingUser.feedbacks[0].headLabel}</div>
                    <div>{dateToString(existingUser.feedbacks[0].date)}</div>
                  </>
                ) : (
                  <>{feedbacksHandler}</>
                )}
              </div>
            </>
          )}
        </div>
      )}
    </>
  );
};

export default UserForm;
