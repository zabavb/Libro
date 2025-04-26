import React from 'react';
import { LoginFormData, loginSchema } from '../../utils';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { GoogleLogin, GoogleOAuthProvider } from '@react-oauth/google';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUser } from '@fortawesome/free-solid-svg-icons';

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
    <div className="flex justify-center items-center min-h-screen bg-gray-100">
      <div className="w-full max-w-md px-6 py-8 rounded-lg shadow-lg" style={{ backgroundColor: '#F4F0E5' }}>
        {/* User Icon */}
        <div className="flex justify-center mb-4">
          <div className="w-12 h-12 bg-orange-500 rounded-full flex items-center justify-center">
            <FontAwesomeIcon icon={faUser} className="text-white text-xl" />
          </div>
        </div>

        <h1 className="text-2xl font-bold text-center text-gray-800">Log in</h1>
        <p className="text-sm text-center text-gray-700 mt-1">
          Please login to access your account and manage your preferences
        </p>

        {/* Google Auth */}
        <GoogleOAuthProvider clientId={clientId}>
          <div className="mt-6 flex justify-center">
            <GoogleLogin
              onSuccess={async (credentialResponse) =>
                onOAuth(credentialResponse.credential)
              }
              onError={() => console.error('Google Login Failed')}
              theme="outline"
              size="large"
              text="continue_with"
              width="100%"
            />
          </div>
        </GoogleOAuthProvider>

        <p className="my-4 text-center text-sm text-gray-600">or</p>

        {/* Login Form */}
        <form onSubmit={handleSubmit(onSubmit)} className="flex flex-col gap-4">
          <div>
            <input
              {...register('identifier')}
              placeholder="Email or Phone Number"
              className={`w-full px-4 py-2 border rounded-md text-gray-900 placeholder-gray-500 focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500 ${
                errors.identifier ? 'border-red-500' : 'border-gray-300'
              }`}
            />
            {errors.identifier && (
              <span className="mt-1 text-red-500 text-xs block">
                {errors.identifier.message}
              </span>
            )}
          </div>

          <div>
            <input
              {...register('password')}
              type="password"
              placeholder="Password"
              className={`w-full px-4 py-2 border rounded-md text-gray-900 placeholder-gray-500 focus:outline-none focus:ring-2 focus:ring-orange-500 focus:border-orange-500 ${
                errors.password ? 'border-red-500' : 'border-gray-300'
              }`}
            />
            {errors.password && (
              <span className="mt-1 text-red-500 text-xs block">
                {errors.password.message}
              </span>
            )}
            <span className="mt-2 text-right text-blue-600 text-xs cursor-pointer hover:underline block">
              Forgot Password?
            </span>
          </div>

          <button
            type="submit"
            disabled={isSubmitting}
            className="mt-4 w-full bg-gray-800 text-white py-3 rounded-md hover:bg-gray-900 disabled:bg-gray-600"
          >
            {isSubmitting ? 'Logging in...' : 'Login'}
          </button>

          <p className="mt-4 text-center text-sm text-gray-700">
            Don't have an account yet?{' '}
            <a href="/register" className="text-orange-600 hover:underline font-medium">
              Register
            </a>
          </p>
        </form>
      </div>
    </div>
  );
};

export default Login;
