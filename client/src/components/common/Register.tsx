import { icons } from '@/lib/icons'
import { User } from '@/types';
import { RegisterFormData, registerSchema } from '@/utils';
import { zodResolver } from '@hookform/resolvers/zod';
import * as Checkbox from '@radix-ui/react-checkbox';
import { useMemo, useState } from 'react';
import { useForm } from 'react-hook-form';

interface RegisterProps {
  data: User | undefined;
  onSubmit: (userData: RegisterFormData) => Promise<void>;
  setIsRegistration: (isRegistration: boolean) => void;
}

const Register: React.FC<RegisterProps> = ({ data, onSubmit, setIsRegistration }) => {

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    watch,
  } = useForm<RegisterFormData>({
    resolver: zodResolver(registerSchema),
  });

  const [agreementChecked, setAgreementChecked] = useState<boolean>(false);

  const handleCheckedChange = (value: boolean | 'indeterminate') => {
    setAgreementChecked(value === true);
  };


  const watchedFields = watch(['firstName', 'lastName', 'email', 'confirmPassword', 'password']);
  const formFilled = useMemo(() => {
    return watchedFields.every(val => val?.toString().trim() !== '') && agreementChecked;
  }, [watchedFields, agreementChecked]);

  const [showPassword, setShowPassword] = useState<boolean>(false);

  return (
    <>
            <div>
          <h1 className='font-semibold text-xl text-center'>Registration</h1>
        </div>
      <form onSubmit={handleSubmit(onSubmit)} className="flex flex-col gap-2">

        <div className="auth-input-row">
          <div className='flex justify-between'>
            <p>First name *</p>
            {errors.firstName && (
              <span className=" text-red-500 text-xs block">
                {errors.firstName.message}
              </span>
            )}
          </div>
          <input
            {...register('firstName')}
            placeholder="Enter your name"
            className={`auth-input-field ${errors.firstName ? "border-[#FF642E]" : "border-[#9C9C97]"}`}
            defaultValue={data?.firstName}
          />

        </div>

        <div className="auth-input-row">
          <div className='flex justify-between'>
            <p>Last name *</p>
            {errors.lastName && (
              <span className=" text-red-500 text-xs block">
                {errors.lastName.message}
              </span>
            )}
          </div>
          <input
            {...register('lastName')}
            placeholder="Enter your last name"
            className={`auth-input-field ${errors.lastName ? "border-[#FF642E]" : "border-[#9C9C97]"}`}
            defaultValue={data?.lastName ?? undefined}
          />

        </div>

        <div className="auth-input-row">
          <div className='flex justify-between'>
            <p>Email *</p>
            {errors.email && (
              <span className=" text-red-500 text-xs block">
                {errors.email.message}
              </span>
            )}
          </div>
          <input
            {...register('email')}
            placeholder="Enter your email"
            className={`auth-input-field ${errors.email ? "border-[#FF642E]" : "border-[#9C9C97]"}`}
            defaultValue={data?.email ?? undefined}
          />
        </div>

        <div className="auth-input-row">
          <div className='flex justify-between'>
            <p>Phone number</p>
            {errors.phoneNumber && (
              <span className=" text-red-500 text-xs block">
                {errors.phoneNumber.message}
              </span>
            )}
          </div>
          <input
            {...register('phoneNumber')}
            placeholder="Enter your phone number"
            className={`auth-input-field ${errors.phoneNumber ? "border-[#FF642E]" : "border-[#9C9C97]"}`}
          />
        </div>

        <div className='auth-input-row'>
          <div>
            <div className='flex justify-between'>
              <p>Password</p>
              {errors.password && (
                <span className=" text-red-500 text-xs">
                  {errors.password.message}
                </span>
              )}
            </div>
            <p className='text-gray text-xs'>
              At least eight characters, at least one letter and one number
            </p>
          </div>
          <div className="w-full relative">
            <input
              {...register('password')}
              type={showPassword ? 'text' : 'password'}
              placeholder="Password"
              className={`auth-input-field ${errors.password ? "border-[#FF642E]" : "border-[#9C9C97]"}`}
            />

            <button
              type="button"
              onClick={() => setShowPassword((prev) => !prev)}
              className="absolute right-6 top-1/2 transform -translate-y-1/2 text-gray-500 text-sm"
              tabIndex={-1}
            >
              <img src={icons.gVisibility} />
            </button>
          </div>
        </div>

        <div className='auth-input-row'>
          <div className='flex justify-between'>
            <p>Confirm password</p>
            {errors.confirmPassword && (
              <span className=" text-red-500 text-xs">
                {errors.confirmPassword.message}
              </span>
            )}
          </div>

          <input
            {...register('confirmPassword')}
            placeholder="Confirm password *"
            type={showPassword ? 'text' : 'password'}
            className={`auth-input-field ${errors.confirmPassword ? "border-[#FF642E]" : "border-[#9C9C97]"}`}
          />
        </div>

        <div className="flex items-start gap-2">
          <Checkbox.Root
            className="shrink-0 w-5 h-5 border border-gray-300 rounded flex items-center justify-center focus:outline-none focus:ring-2 focus:ring-orange-500"
            id="terms"
            checked={agreementChecked}
            onCheckedChange={handleCheckedChange}
          >
            <Checkbox.Indicator>
              <img className='w-5 h-5' src={icons.bCheck} />
            </Checkbox.Indicator>
          </Checkbox.Root>
          <div>
          <label htmlFor="terms" className="text-dark text-wrap font-semibold max-w-[430px]">
            I agree to the <span className='text-blue-600'>terms of use</span>
          </label>
          <p className="text-xs text-gray-600 text-wrap max-w-[430px]">
            By registering, you agree to the storage and use of the personal data provided by you by the company "Libro" in accordance with the current legislation of Ukraine on the inviolability of personal information.
          </p>
          </div>
        </div>


        <button
          type="submit"
          disabled={!formFilled || isSubmitting}
          className={`submit-btn ${!formFilled || isSubmitting ? 'opacity-50 cursor-not-allowed' : ''}`}
        >
          {isSubmitting ? 'Processing...' : 'Register'}
        </button>

        <p className="mt-4 text-gray-600 text-center text-sm">
          Already have an account?{' '}
          <a
            onClick={() => setIsRegistration(false)}
            className="text-blue-600 hover:underline"
          >
            Sign in
          </a>
        </p>
      </form>
    </>
  )
}

export default Register;