import React from 'react';
import { useForm } from 'react-hook-form';
import { RegisterFormData, registerSchema } from '../../utils';
import { zodResolver } from '@hookform/resolvers/zod';
import { User } from '../../types';

interface RegisterProps {
  data: User | undefined;
  onSubmit: (userData: RegisterFormData) => Promise<void>;
}

const Register: React.FC<RegisterProps> = ({ data, onSubmit }) => {
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<RegisterFormData>({
    resolver: zodResolver(registerSchema),
  });

  return (
    <div>
      <h2>Register</h2>
      <form onSubmit={handleSubmit(onSubmit)}>
        <input
          {...register('firstName')}
          placeholder='First Name'
          value={data?.firstName}
        />
        <p>{errors.firstName?.message}</p>

        <input
          {...register('lastName')}
          placeholder='Last Name (Optional)'
          value={data?.lastName}
        />
        <p>{errors.lastName?.message}</p>

        <input
          {...register('email')}
          placeholder='Email'
          type='email'
          value={data?.email}
        />
        <p>{errors.email?.message}</p>

        <input
          {...register('phoneNumber')}
          placeholder='Phone Number'
          type='tel'
        />
        <p>{errors.phoneNumber?.message}</p>

        <input
          {...register('password')}
          placeholder='Password'
          type='password'
        />
        <p>{errors.password?.message}</p>

        <input
          {...register('confirmPassword')}
          placeholder='Confirm Password'
          type='password'
        />
        <p>{errors.confirmPassword?.message}</p>

        <button type='submit' disabled={isSubmitting}>
          {isSubmitting ? 'Registering...' : 'Register'}
        </button>
      </form>

      <div>
        <p>
          Already have an account?{' '}
          <span>
            <a href='/login'>Login</a>
          </span>
        </p>
      </div>
    </div>
  );
};

export default Register;
