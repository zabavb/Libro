import { LoginFormData, loginSchema } from "@/utils";
import { zodResolver } from "@hookform/resolvers/zod";
import { GoogleLogin, GoogleOAuthProvider } from "@react-oauth/google";
import { useMemo, useState } from "react";
import { useForm } from "react-hook-form";
import {icons} from '@/lib/icons'
interface loginProps {
    setIsRegistration: (isRegistration: boolean) => void;
    onOAuth: (token: string | undefined) => Promise<void>;
    onSubmit: (userData: LoginFormData) => Promise<void>;
}

const Login: React.FC<loginProps> = ({ setIsRegistration, onOAuth, onSubmit }) => {
    const {
        register,
        handleSubmit,
        formState: { errors, isSubmitting },
        watch,
    } = useForm<LoginFormData>({
        resolver: zodResolver(loginSchema),
    });

    const clientId = import.meta.env.VITE_GOOGLE_CLIENT_ID;

    const [showPassword, setShowPassword] = useState<boolean>(false);

    const watchedFields = watch(['identifier', 'password']);
    const formFilled = useMemo(() => {
        return watchedFields.every(val => val?.toString().trim() !== '');
    }, [watchedFields]);

    return (
        <>
            <div>
                <h1 className="text-xl font-semibold text-center">Account login</h1>
                <p className="font-normal text-center">To track order status and <br />receive personalized recommendations</p>
            </div>
            <div>
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
            </div>
            <p className="text-center">or</p>
            <form onSubmit={handleSubmit(onSubmit)} className="flex flex-col gap-4">
                <div className="auth-input-row">
                    <p>Email or Phone Number*</p>
                    <input
                        {...register('identifier')}
                        placeholder="Email or Phone Number"
                        className="auth-input-field"
                    />
                    {errors.identifier && (
                        <span className="mt-1 text-red-500 text-xs block">
                            {errors.identifier.message}
                        </span>
                    )}
                </div>

                <div className="auth-input-row">
                    <div className="flex justify-between">
                        <p>Password*</p>
                        <span className="mt-2 text-right text-blue-600 text-xs cursor-pointer hover:underline block">
                            Forgot Password?
                        </span>
                    </div>
                    <div className="w-full relative">
                    <input
                        {...register('password')}
                        type={showPassword ? 'text' :'password'}
                        placeholder="Password"
                        className="auth-input-field"
                    />
                      <button
                            type="button"
                            onClick={() => setShowPassword((prev) => !prev)}
                            className="absolute right-6 top-1/2 transform -translate-y-1/2 text-gray-500 text-sm"
                            tabIndex={-1} // avoids tab focus
                        >
                            <img src={icons.gVisibility}/>
                        </button>
                    </div>
                    {errors.password && (
                        <span className="mt-1 text-red-500 text-xs block">
                            {errors.password.message}
                        </span>
                    )}

                </div>

                <button
                    type="submit"
                    disabled={!formFilled || isSubmitting}
                    className={`submit-btn ${!formFilled || isSubmitting ? 'opacity-50 cursor-not-allowed' : ''}`}
                >
                    {isSubmitting ? 'Logging in...' : 'Login'}
                </button>

                <p className="mt-4 text-center text-sm text-gray-700">
                    Don't have an account yet?{' '}
                    <a onClick={() => setIsRegistration(true)} className="text-blue-600 cursor-pointer hover:underline font-medium">
                        Register
                    </a>
                </p>
            </form>
        </>
    )
}

export default Login;