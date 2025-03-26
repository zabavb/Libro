import {
  User,
  PaginatedResponse,
  UserFilter,
  UserSort,
  UserCard,
  UserForm,
  ServiceResponse,
} from '../types';
import {
  getAllUsers,
  getUserById,
  createUser,
  updateUser,
  deleteUser,
} from '../api/repositories/userRepository';
import { roleEnumToNumber } from '../api/adapters/userAdapter';

/**
 * Fetch a paginated list of users with optional search term, filters, and sorting.
 */
export const fetchUsersService = async (
  pageNumber: number = 1,
  pageSize: number = 10,
  searchTerm?: string,
  filters?: UserFilter,
  sort?: UserSort,
): Promise<ServiceResponse<PaginatedResponse<UserCard>>> => {
  const response: ServiceResponse<PaginatedResponse<UserCard>> = {
    data: null,
    loading: true,
    error: null,
  };

  try {
    const formattedSort = Object.fromEntries(
      Object.entries(sort ?? {}).map(([key, value]) => [key, value ? 1 : 2]),
    );

    const params = {
      searchTerm,
      ...filters,
      role:
        filters?.role !== undefined
          ? roleEnumToNumber(filters.role).toString()
          : undefined,
      ...formattedSort,
    };

    response.data = await getAllUsers(pageNumber, pageSize, params);
  } catch (error) {
    console.error('Failed to fetch users', error);
    response.error =
      'An error occurred while fetching users. Please try again later.';
  } finally {
    response.loading = false;
  }

  return response;
};

/**
 * Fetch a single user by their ID.
 */
export const fetchUserByIdService = async (
  id: string,
): Promise<ServiceResponse<UserForm>> => {
  const response: ServiceResponse<UserForm> = {
    data: null,
    loading: true,
    error: null,
  };

  try {
    response.data = await getUserById(id);
  } catch (error) {
    console.error(`Failed to fetch user ID [${id}]`, error);
    response.error =
      'An error occurred while fetching the user. Please try again later.';
  } finally {
    response.loading = false;
  }

  return response;
};

/**
 * Create a new user.
 */
export const addUserService = async (
  user: Partial<User>,
): Promise<ServiceResponse<User>> => {
  const response: ServiceResponse<User> = {
    data: null,
    loading: true,
    error: null,
  };

  try {
    response.data = await createUser(user);
  } catch (error) {
    console.error('Failed to create user', error);
    response.error =
      'An error occurred while adding the user. Please try again later.';
  } finally {
    response.loading = false;
  }

  return response;
};

/**
 * Update an existing user by ID.
 */
export const editUserService = async (
  id: string,
  user: Partial<User>,
): Promise<ServiceResponse<User>> => {
  const response: ServiceResponse<User> = {
    data: null,
    loading: true,
    error: null,
  };

  try {
    response.data = await updateUser(id, user);
  } catch (error) {
    console.error(`Failed to update user ID [${id}]`, error);
    response.error =
      'An error occurred while updating the user. Please try again later.';
  } finally {
    response.loading = false;
  }

  return response;
};

/**
 * Delete a user by ID.
 */
export const removeUserService = async (
  id: string,
): Promise<ServiceResponse<string>> => {
  const response: ServiceResponse<string> = {
    data: null,
    loading: true,
    error: null,
  };

  try {
    await deleteUser(id);
    response.data = id;
  } catch (error) {
    console.error(`Failed to delete user ID [${id}]`, error);
    response.error =
      'An error occurred while deleting the user. Please try again later.';
  } finally {
    response.loading = false;
  }

  return response;
};
