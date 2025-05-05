import React from "react";
import '@/assets/styles/layout/auth-panel.css';
import { icons } from "@/lib/icons"
import { LoginFormData, RegisterFormData } from "@/utils";
import Login from "../common/Login";
import Register from "../common/Register";
import { User } from "@/types";
interface AuthPanelProps {
    data: User | undefined;
    setIsAuthOpen: (isOpen: boolean) => void;
    isOpen: boolean;
    setIsRegistration: (isRegistration: boolean) => void;
    isRegistration: boolean;
    onOAuth: (token: string | undefined) => Promise<void>;
    onSubmit: (userData: LoginFormData) => Promise<void>;
    onRegisterSubmit: (userData: RegisterFormData) => Promise<void>;
}



const AuthPanel: React.FC<AuthPanelProps> = ({ data, setIsAuthOpen, isOpen, isRegistration, onOAuth, onSubmit, setIsRegistration, onRegisterSubmit }) => {


    return (
        <div>
            <div>

                <div
                    className={`dim ${isOpen && 'visible'}`}
                    aria-hidden={!isOpen}
                    onClick={() => setIsAuthOpen(false)}
                />

            </div>
            <div aria-hidden={!isOpen} aria-modal={true} className={`auth-panel ${isOpen && 'visible'}`}>
                <div className="user-icon-container">
                    <img src={icons.bUser} className="user-icon" />
                </div>
                <div className="flex flex-col gap-5 w-full">
                    {isRegistration ? (
                        <>
                            <Register
                                setIsRegistration={setIsRegistration}
                                onSubmit={onRegisterSubmit}
                                data={data}/>
                        </>
                    ) :
                        (
                            <>
                                <Login 
                                setIsRegistration={setIsRegistration}
                                onOAuth={onOAuth}
                                onSubmit={onSubmit}/>
                            </>
                        )
                    }
                </div>
            </div>
        </div>
    );
};

export default AuthPanel;
