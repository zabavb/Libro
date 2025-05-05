import { dateToString } from '@/api/adapters/commonAdapters';
import { ComplicatedLoading, Role, Subscription, User } from '@/types';
import { UserProfileFormData, userProfileSchema } from '@/utils';
import { zodResolver } from '@hookform/resolvers/zod';
import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import '@/assets/styles/components/common/profile.css'
import { icons } from '@/lib/icons'
import NewsletterAdvert from '../common/NewsletterAdvert';
import { roleEnumToNumber } from '@/api/adapters/userAdapter';
interface ProfileProps {
  user: User;
  onEditUser: (
    field: keyof UserProfileFormData,
    value: UserProfileFormData[keyof UserProfileFormData],
  ) => Promise<void>;
  onUpdatePassword: (id: string, password: string) => Promise<void>;
  subscription: Subscription | undefined;
  isSubscribedFor365: boolean;
  loading: ComplicatedLoading<UserProfileFormData>;
  onNavigate: (route: string) => void;
}


const Profile: React.FC<ProfileProps> = ({
  user,
  onEditUser,
  onUpdatePassword,
  subscription,
  isSubscribedFor365,
  loading,
  onNavigate,
}) => {
  const {
    register,
    setValue,
    watch,
    formState: { errors },
    trigger,
  } = useForm<UserProfileFormData>({
    resolver: zodResolver(userProfileSchema),
    defaultValues: {
      firstName: '',
      lastName: '',
      email: '',
      phoneNumber: '',
      dateOfBirth: dateToString(user.dateOfBirth ?? new Date()),
      password: '',
      confirmPassword: '',
    },
  });


  
  const [editableField, setEditableField] = useState<
    keyof UserProfileFormData | null
  >(null);

  useEffect(() => {
    if (user) {
      setValue('lastName', user.lastName ?? '');
      setValue('firstName', user.firstName);
      setValue('email', user.email ?? '');
      setValue('phoneNumber', user.phoneNumber ?? '');
      setValue(
        'dateOfBirth',
        user.dateOfBirth ? dateToString(user.dateOfBirth) : '',
      );
    }
  }, [user, setValue]);

  const handleSave = async (field: keyof UserProfileFormData) => {
    let isValid: boolean;
    if (field === 'password' || field === 'confirmPassword')
      isValid = await trigger();
    else isValid = await trigger(field);

    if (!isValid) return;

    setEditableField(null);
    const value = watch(field);

    if (field === 'password') await onUpdatePassword(user.id, value as string);
    else onEditUser(field, value);
  };

  const handleEditClick = (field: keyof UserProfileFormData) => {
    setEditableField(field);
  };

  const renderField = (
    label: string,
    fieldName: keyof UserProfileFormData,
    type: string = 'text',
    placeholder: string = '',
  ) => (
    <div className='flex flex-col gap-1.5'>
      <p className='text-dark text-sm'>{label}</p>
      <div
        style={
          loading.isLoading && loading.fieldName === fieldName
            ? { opacity: 0.5 }
            : {}
        }
        className='input-field'
      >
        <input
          className='input-area'
          type={type}
          {...register(fieldName)}
          placeholder={placeholder}
          disabled={editableField !== fieldName}
        />
        {editableField === fieldName ? (
          <button className='input-btn' type='button' onClick={() => handleSave(fieldName)}>
            Save
          </button>
        ) : (
          <button className='input-btn' type='button' onClick={() => handleEditClick(fieldName)}>
            Edit
          </button>
        )}
      </div>
      <p>{errors[fieldName]?.message}</p>
    </div>
  );

  const renderPasswordFields = () => (
    <>
      {/* Password field */}
      <div className='flex flex-col gap-1.5'>
        <p className='text-dark text-sm'>Change password</p>
        <div
          style={
            loading.isLoading && loading.fieldName === 'password'
              ? { opacity: 0.5 }
              : {}
          }
          className='input-field'
        >
          <input
            className='input-area'
            type='password'
            {...register('password')}
            placeholder='Password'
            disabled={editableField !== 'password'}
          />
          {editableField === 'password' ? (
            <button
              className='input-btn'
              type='button'
              onClick={() => setEditableField('confirmPassword')}
            >
              Next
            </button>
          ) : (
            <button className='input-btn' type='button' onClick={() => handleEditClick('password')}>
              Change
            </button>
          )}
        </div>
        <p>{errors.password?.message}</p>
      </div>

      {/* Confirm password field */}
      {editableField === 'confirmPassword' && (
        <div>
          <label>Confirm password</label>
          <div
            style={
              loading.isLoading && loading.fieldName === 'confirmPassword'
                ? { opacity: 0.5 }
                : {}
            }
            className='input-field'
          >
            <input
              className='input-area'
              type='password'
              {...register('confirmPassword')}
              placeholder='Confirm password'
            />
            <button className='input-btn' type='button' onClick={() => handleSave('password')}>
              Save
            </button>
          </div>
          <p>{errors.confirmPassword?.message}</p>
        </div>
      )}
    </>
  );

  return (
    <div className='form-container'>
      <h1 className='text-2xl text-[#F4F0E5] font-semibold'>Profile editing</h1>

      <div className='flex gap-[18px]'>
        <div className='form-section w-[70%]'>
          <h3 className='font-semibold text-xl text-dark'>Information</h3>
          <div
            style={
              loading.isLoading && loading.fieldName === 'all'
                ? { opacity: 0.5 }
                : {}
            }
            className='flex flex-col gap-5'
          >
            {renderField('Last name', 'lastName', 'text', 'Last name')}
            {renderField('First name', 'firstName', 'text', 'First name')}
            {renderField('Email', 'email', 'email', 'Email')}
            {renderField('Phone number', 'phoneNumber', 'tel', 'Phone number')}
            {renderPasswordFields()}
            {renderField('Date of birth', 'dateOfBirth', 'date', 'Date of birth')}
          </div>
        </div>
        <div className='flex flex-col gap-[13px] w-[30%]'>
          {user.role === roleEnumToNumber(Role.ADMIN) ||
           user.role === roleEnumToNumber(Role.ADMIN) &&
           (
            <a href='#' className='admin-btn'>
            <div className='flex gap-4 justify-center'>
              <img src={icons.bPen} />
              <p>Go to admin panel</p>
            </div>
          </a>
           )}
          <div className='form-section'>
            <div className='flex justify-between items-center'>
              <h3 className='font-semibold text-xl text-dark'>Delivery 365</h3>
              <div>
                {isSubscribedFor365 ? (
                  <div className='sub-active'>
                    Active
                  </div>
                ) : (
                  <div className='sub-inactive'>
                    Inactive
                  </div>
                )}
              </div>
            </div>
            <div className='flex gap-1.5'>
              <div className='sub-image-container'>
                <p className='sub-image'>365</p>
              </div>
              <div>
                <p className='sub-desc'>Subscription for free delivery of orders from Libro
                  throughout Ukraine. Valid for all orders over UAH 100
                  for 1 year from the moment of registration.
                  There are no restrictions on the number of orders.</p>
              </div>
            </div>
          </div>
          <div className='form-section'>
            <div className='flex justify-between items-center'>
              <h3 className='font-semibold text-xl text-dark'>Newsletter subscription</h3>
              <div>
                {subscription ? (
                  <div className='sub-active'>
                    Active
                  </div>
                ) : (
                  <div className='sub-inactive'>
                    Inactive
                  </div>
                )}
              </div>
            </div>
            <div>
            <p className='sub-desc'>We exchange emails for book trends,
            information about new releases, book collections, secret promo codes</p>
            </div>
          </div>
        </div>
      </div>
      <NewsletterAdvert/>
    </div>
  );
};

export default Profile;
