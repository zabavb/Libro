import { updatePassword } from '@/api/repositories/passwordRepository';
import { ServiceResponse } from '@/types';

/**
 * Update an existing password by user ID.
 */
export const editPasswordService = async (
  userId: string,
  password: string,
): Promise<ServiceResponse<string>> => {
  const response: ServiceResponse<string> = {
    data: null,
    loading: true,
    error: null,
  };

  try {
    response.data = await updatePassword(userId, password);
  } catch (error) {
    console.error(`Failed to update password by user ID [${userId}]`, error);
    response.error =
      'An error occurred while updating the password. Please try again later.';
  } finally {
    response.loading = false;
  }

  return response;
};
