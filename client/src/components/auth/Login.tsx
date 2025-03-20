import React from 'react';
import { LoginFormData, loginSchema } from '../../utils';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { GoogleLogin, GoogleOAuthProvider } from '@react-oauth/google';

interface LoginProps {
  onOAuth: (token: string | undefined) => Promise<void>;
  onSubmit: (userData: LoginFormData) => Promise<void>;
}

const Login: React.FC<LoginProps> = ({ onOAuth, onSubmit }) => {
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<LoginFormData>({
    resolver: zodResolver(loginSchema),
  });
  const clientId = import.meta.env.VITE_GOOGLE_CLIENT_ID;

  return (
    <div>
      <h2>Login</h2>
      <GoogleOAuthProvider clientId={clientId}>
        <GoogleLogin
          onSuccess={async (credentialResponse) =>
            onOAuth(credentialResponse.credential)
          }
        />
      </GoogleOAuthProvider>

      <hr />

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
