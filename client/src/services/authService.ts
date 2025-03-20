import axios from 'axios';
import { LOGIN, OAUTH, REGISTER } from '../api/index';
import { JwtResponse, User } from '../types';
import { LoginFormData, RegisterFormData } from '../utils';

/**
 * Generic function to handle API requests with error handling.
 */
const apiCall = async (
  method: 'get' | 'post',
  url: string,
  data?: object,
  token?: string,
) => {
  try {
    const headers = token ? { Authorization: `Bearer ${token}` } : {};
    const response = await axios({ method, url, data, headers });
    return response.data;
  } catch (error) {
    console.error(`API Call Failed: ${method.toUpperCase()} ${url}`, error);
    throw new Error('An error occurred. Please try again later.');
  }
};

/**
 * Logs in the user and stores the token.
 */
export const loginService = async (data: LoginFormData): Promise<JwtResponse> =>
  await apiCall('post', LOGIN, data);

/**
 * Registers a new user.
 */
export const registerService = async (
  data: RegisterFormData,
): Promise<void> => {
  if (data.phoneNumber === '') delete data.phoneNumber;

  const response = await apiCall('post', REGISTER, data);
  return response;
};

/**
 * Logs in the user using OAuth (Google Authentication).
 */
export const oAuthService = async (
  token: string,
): Promise<JwtResponse | User> => await apiCall('post', OAUTH, { token });
