import React from 'react';
import { useForm } from 'react-hook-form';
import { RegisterFormData, registerSchema } from '../../utils';
import { zodResolver } from '@hookform/resolvers/zod';
import { User } from '../../types';
import * as Checkbox from '@radix-ui/react-checkbox';
import { faUser, faCheck } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

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
    <div className="flex justify-center items-center min-h-screen bg-gray-100">
      <div className="p-6 w-full max-w-md rounded-lg shadow-md" style={{ backgroundColor: '#F4F0E5' }}> 
        <div className="flex justify-center mb-6">
          <div className="w-12 h-12 rounded-full bg-orange-500 flex items-center justify-center">
            <FontAwesomeIcon icon={faUser} className="text-white text-xl" />
          </div>
        </div>

        <h1 className="text-xl font-bold text-center text-gray-800 mb-2">Register</h1>

        <form onSubmit={handleSubmit(onSubmit)} className="flex flex-col gap-4">
          <div>
            <input
              {...register('firstName')}
              placeholder="First name *"
              className={`w-full px-4 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-orange-500 ${
                errors.firstName ? 'border-red-500' : 'border-gray-300'
              }`}
              defaultValue={data?.firstName}
            />
            {errors.firstName && (
              <span className="mt-1 text-red-500 text-xs">
                {errors.firstName.message}
              </span>
            )}
          </div>

          <div>
            <input
              {...register('lastName')}
              placeholder="Last name"
              className={`w-full px-4 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-orange-500 ${
                errors.lastName ? 'border-red-500' : 'border-gray-300'
              }`}
              defaultValue={data?.lastName ?? undefined}
            />
            {errors.lastName && (
              <span className="mt-1 text-red-500 text-xs">
                {errors.lastName.message}
              </span>
            )}
          </div>

          <div>
            <input
              {...register('email')}
              placeholder="Email *"
              type="email"
              className={`w-full px-4 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-orange-500 ${
                errors.email ? 'border-red-500' : 'border-gray-300'
              }`}
              defaultValue={data?.email ?? undefined}
            />
            {errors.email && (
              <span className="mt-1 text-red-500 text-xs">
                {errors.email.message}
              </span>
            )}
          </div>

          <div>
            <input
              {...register('phoneNumber')}
              placeholder="Phone number"
              type="tel"
              className={`w-full px-4 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-orange-500 ${
                errors.phoneNumber ? 'border-red-500' : 'border-gray-300'
              }`}
            />
            {errors.phoneNumber && (
              <span className="mt-1 text-red-500 text-xs">
                {errors.phoneNumber.message}
              </span>
            )}
          </div>

          <div>
            <input
              {...register('password')}
              placeholder="Password *"
              type="password"
              className={`w-full px-4 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-orange-500 ${
                errors.password ? 'border-red-500' : 'border-gray-300'
              }`}
            />
            {errors.password && (
              <span className="mt-1 text-red-500 text-xs">
                {errors.password.message}
              </span>
            )}
          </div>

          <div>
            <input
              {...register('confirmPassword')}
              placeholder="Confirm password *"
              type="password"
              className={`w-full px-4 py-2 border rounded-md focus:outline-none focus:ring-2 focus:ring-orange-500 ${
                errors.confirmPassword ? 'border-red-500' : 'border-gray-300'
              }`}
            />
            {errors.confirmPassword && (
              <span className="mt-1 text-red-500 text-xs">
                {errors.confirmPassword.message}
              </span>
            )}
          </div>

          <div className="flex items-start gap-2">
            <Checkbox.Root
              className="w-5 h-5 border border-gray-300 rounded flex items-center justify-center focus:outline-none focus:ring-2 focus:ring-orange-500"
              id="terms"
            >
              <Checkbox.Indicator>
                <FontAwesomeIcon icon={faCheck} className="text-orange-500 text-sm" />
              </Checkbox.Indicator>
            </Checkbox.Root>
            <label htmlFor="terms" className="text-xs text-gray-600">
              I agree to the Terms of Use and Privacy Policy of ChudoMarket. I would like to receive updates and promotional information.
            </label>
          </div>

          <button
            type="submit"
            disabled={isSubmitting}
            className="mt-4 w-full bg-gray-800 text-white py-3 rounded-md hover:bg-gray-900 disabled:bg-gray-600"
          >
            {isSubmitting ? 'Registering...' : 'Register'}
          </button>

          <p className="mt-4 text-gray-600 text-center text-sm">
            Already have an account?{' '}
            <a
              href="/login"
              className="text-orange-500 hover:underline"
            >
              Sign in
            </a>
          </p>
        </form>
      </div>
    </div>
  );
};

export default Register;
