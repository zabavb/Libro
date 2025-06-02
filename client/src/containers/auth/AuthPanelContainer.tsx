import React, { useCallback, useEffect, useState } from "react";
import '@/assets/styles/layout/auth-panel.css';
import { AppDispatch } from "@/state/redux";
import { useDispatch } from "react-redux";
import { useAuth } from "@/state/context";
import { useLocation, useNavigate } from "react-router-dom";
import { NotificationData, User } from "@/types";
import { LoginFormData, RegisterFormData } from "@/utils";
import { addNotification } from "@/state/redux/slices/notificationSlice";
import AuthPanel from "@/components/layouts/AuthPanel";

interface AuthPanelContainerProps {
    setIsAuthOpen: (isOpen: boolean) => void;
    isOpen: boolean;
}

const AuthPanelContainer: React.FC<AuthPanelContainerProps> = ({ setIsAuthOpen, isOpen }) => {

    const [isRegistration, setIsRegistration] = useState<boolean>(false);

    const dispatch = useDispatch<AppDispatch>();
    const { oAuth, login, register } = useAuth();
    const navigate = useNavigate();

    const location = useLocation();
    const user = location.state?.user as User | undefined;
    
    useEffect(() => {
        setIsRegistration(location.state?.isRegister === true);
    },[location.state])
    const handleOAuth = async (token: string | undefined) => {
        const data = await oAuth(token);

        // If it is new user
        if (data as User) navigate('/', { state: { authOpen:true,isRegister:true, user: data as User }, replace: true, });

        if ((data as NotificationData).type === 'success')
            handleSuccess({ type: 'success', message: 'Login successful!' });
        else handleError(data as NotificationData);
    };

    const handleSubmit = async (userData: LoginFormData) => {
        const data = await login(userData);
        if (data.type === 'success') handleSuccess(data);
        else handleError(data);
    };

    const handleRegisterSubmit = async (userData: RegisterFormData) => {
        const data = await register(userData);
        if (data.type === 'success') handleSuccess(data);
        else handleError(data);
      };
    const handleError = useCallback(
        (data: NotificationData) => dispatch(addNotification(data)),
        [dispatch],
    );


    const handleSuccess = useCallback(
        (data: NotificationData) => {
            dispatch(addNotification(data));
            if (isRegistration) navigate('/login')
            else navigate('/profile');
        },
        [dispatch, navigate, isRegistration],
    );

    return (
        <AuthPanel
            data={user}
            isOpen={isOpen}
            setIsAuthOpen={setIsAuthOpen}
            isRegistration={isRegistration}
            setIsRegistration={setIsRegistration}
            onOAuth={handleOAuth}
            onSubmit={handleSubmit}
            onRegisterSubmit={handleRegisterSubmit}
        />
    );
};

export default AuthPanelContainer;
