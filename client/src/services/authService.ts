import axios from 'axios';
import { LOGIN, REGISTER, ME } from '../api/index';
import { JwtResponse } from '../types';
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
export const registerService = async (data: RegisterFormData): Promise<void> =>
  await apiCall('post', REGISTER, data);

/**
 * Fetches the current user.
 */
/* export const getMeService = async () => {
	const token = localStorage.getItem("token")
	if (!token) throw new Error("User is not authenticated.")
	return await apiCall("get", ME, undefined, token)
}
 */
export const getMeService = async (token?: string) => {
  return await apiCall('get', ME, undefined, token);
  /* try {
		const response = await axios.get(ME, {
			headers: { Authorization: `Bearer ${token}` },
		})
		return response.data
	} catch (error) {
		console.error(`getMeService: Failed to fetch current user`, error)
		throw new Error(`Failed to load your data. Please try again later.`)
	} */
};
