import { dateToString } from '@/api/adapters/commonAdapters';
import { ComplicatedLoading, Subscription, User } from '@/types';
import { UserProfileFormData, userProfileSchema } from '@/utils';
import { zodResolver } from '@hookform/resolvers/zod';
import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';

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

  // const watchedPassword = watch('password');

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
    <div>
      <label>{label}</label>
      <div
        style={
          loading.isLoading && loading.fieldName === fieldName
            ? { opacity: 0.5 }
            : {}
        }
      >
        <input
          type={type}
          {...register(fieldName)}
          placeholder={placeholder}
          disabled={editableField !== fieldName}
        />
        {editableField === fieldName ? (
          <button type='button' onClick={() => handleSave(fieldName)}>
            Save
          </button>
        ) : (
          <button type='button' onClick={() => handleEditClick(fieldName)}>
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
      <div>
        <label>Change password</label>
        <div
          style={
            loading.isLoading && loading.fieldName === 'password'
              ? { opacity: 0.5 }
              : {}
          }
        >
          <input
            type='password'
            {...register('password')}
            placeholder='Password'
            disabled={editableField !== 'password'}
          />
          {editableField === 'password' ? (
            <button
              type='button'
              onClick={() => setEditableField('confirmPassword')}
            >
              Next
            </button>
          ) : (
            <button type='button' onClick={() => handleEditClick('password')}>
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
          >
            <input
              type='password'
              {...register('confirmPassword')}
              placeholder='Confirm password'
            />
            <button type='button' onClick={() => handleSave('password')}>
              Save
            </button>
          </div>
          <p>{errors.confirmPassword?.message}</p>
        </div>
      )}
    </>
  );

  return (
    <>
      <h1>Profile editing</h1>

      <hr />

      <div>
        <h3>Information</h3>
        <div
          style={
            loading.isLoading && loading.fieldName === 'all'
              ? { opacity: 0.5 }
              : {}
          }
        >
          {renderField('Last name', 'lastName', 'text', 'Last name')}
          {renderField('First name', 'firstName', 'text', 'First name')}
          {renderField('Email', 'email', 'email', 'Email')}
          {renderField('Phone number', 'phoneNumber', 'tel', 'Phone number')}
          {/* {renderField('Change password', 'password', 'password', 'Password')}
          {renderField(
            'Confirm password',
            'confirmPassword',
            'password',
            'Confirm password',
          )} */}
          {renderPasswordFields()}
          {renderField('Date of birth', 'dateOfBirth', 'date', 'Date of birth')}
        </div>
      </div>

      <hr />

      {subscription && (
        <div>
          <h3>{subscription.title}</h3>
          <img width='44' height='44' src={subscription.imageUrl} />
          <div>{isSubscribedFor365 ? 'Present' : 'Unpresent'}</div>
          <p>{subscription.description}</p>
          <input
            type='button'
            value='More about delivery'
            onClick={() => onNavigate(`/subscriptions/${subscription.id}`)}
          />
        </div>
      )}

      <hr />

      <div>
        <h3>Newsletter subscription</h3>
        <div>Unpresent</div>
        <p>
          We change email to book trends Information about new releases, book
          selections, secret promo codes
        </p>
      </div>

      <hr />

      <div>
        <h2>
          <span>-15%</span>
          discounts on the first purchase for subscribing to the newsletter
        </h2>
        <p>
          Приєднуйтеся до нашої спільноти, щоб отримувати інформацію про
          найновіші акції та товари
        </p>
        <div>
          <input type='text' placeholder='Enter email' />
          <input type='button' value='Subscribe' />
        </div>
      </div>
    </>
  );
};

export default Profile;
