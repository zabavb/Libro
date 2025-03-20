import { createContext, useContext, useState } from 'react';
import { loginService, oAuthService, registerService } from '../../services';
import { JwtResponse, NotificationData, User } from '../../types';
import { LoginFormData, RegisterFormData } from '../../utils';

interface AuthContextProps {
  user: User | null;
  token: string | null;
  login: (data: LoginFormData) => Promise<NotificationData>;
  register: (data: RegisterFormData) => Promise<NotificationData>;
  oAuth: (credential: string | undefined) => Promise<NotificationData | User>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextProps | undefined>(undefined);

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [user, setUser] = useState<User | null>(null);
  const [token, setToken] = useState<string | null>(
    localStorage.getItem('token'),
  );

  const login = async (userData: LoginFormData): Promise<NotificationData> => {
    try {
      const data = await loginService(userData);
      authenticate(data);
      return { type: 'success', message: 'Login successful!' };
    } catch (error) {
      console.error('Login failed:', error);
      return {
        type: 'error',
        message: 'Incorrect email/phone number or password.',
      };
    }
  };

  const register = async (
    userData: RegisterFormData,
  ): Promise<NotificationData> => {
    try {
      await registerService(userData);
      return {
        type: 'success',
        message:
          'You have successfully register! Now, login in to your account.',
      };
    } catch (error) {
      console.error('Register failed:', error);
      return {
        type: 'error',
        message: 'Invalid credentials. Please try again.',
      };
    }
  };

  const oAuth = async (
    token: string | undefined,
  ): Promise<NotificationData | User> => {
    try {
      const data = await oAuthService(token as string);

      // If it is new user
      const user = data as User;
      if (user.id === '00000000-0000-0000-0000-000000000000') return user;

      authenticate(data as JwtResponse);
      return { type: 'success', message: 'Login successful!' };
    } catch (error) {
      console.error('Google authentication failed:', error);
      return {
        type: 'error',
        message: 'Google authentication failed.',
      };
    }
  };

  const logout = async () => {
    localStorage.removeItem('token');
    localStorage.removeItem('token_expiry');
    localStorage.removeItem('user');
    setUser(null);
    setToken(null);
  };

  const authenticate = async (data: JwtResponse) => {
    localStorage.setItem('token', data.token);
    localStorage.setItem(
      'token_expiry',
      String(Date.now() + data.expiresIn * 60 * 1000),
    );
    localStorage.setItem('user', JSON.stringify(data.user));

    setToken(data.token);
    setUser(data.user);
  };

  return (
    <AuthContext.Provider
      value={{ user, token, login, register, oAuth, logout }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) throw new Error('useAuth must be used within an AuthProvider');
  return context;
};
