import React from 'react';
import { LoginFormData, loginSchema } from '../../utils';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';

interface LoginProps {
  onSubmit: (userData: LoginFormData) => Promise<void>;
}

const Login: React.FC<LoginProps> = ({ onSubmit }) => {
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<LoginFormData>({
    resolver: zodResolver(loginSchema),
  });

  return (
    <div>
      <h2>Login</h2>
      <form onSubmit={handleSubmit(onSubmit)}>
        <div>
          <input
            {...register('identifier')}
            placeholder='Email or Phone Number'
          />
          <p>{errors.identifier?.message}</p>
        </div>
        <div>
          <input
            {...register('password')}
            placeholder='Password'
            type='password'
          />
          <p>{errors.password?.message}</p>
        </div>

        <button type='submit' disabled={isSubmitting}>
          {isSubmitting ? 'Logging in...' : 'Login'}
        </button>

        <div>
          <p>
            Don't have an account yet?{' '}
            <span>
              <a href='/register'>Register</a>
            </span>
          </p>
        </div>
      </form>
    </div>
  );
};

export default Login;
