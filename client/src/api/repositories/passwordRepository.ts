import axios from 'axios';
import { PASSWORD } from '..';
import { getAuthHeaders } from './common';

/**
 * Update an existing password by user ID.
 */
export const updatePassword = async (
  userId: string,
  password: string,
): Promise<string> => {
  const response = await axios.put<string>(PASSWORD(userId), password, {
    headers: getAuthHeaders('application/json'),
  });
  return response.data;
};
