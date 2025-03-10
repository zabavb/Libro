import axios from "axios";
import { USERS_PAGINATED, USERS, USER_BY_ID } from "../index";
import { User, PaginatedResponse } from "../../types";

const API_URL = "/api/users";

interface UserQueryParams {
  role?: string;
  dateOfBirthStart?: string;
  dateOfBirthEnd?: string;
  hasSubscription?: boolean;
  searchTerm?: string;
}

/**
 * Fetch a paginated list of users with optional filters.
 */
export const getAllUsers = async (
  pageNumber: number = 1,
  pageSize: number = 10,
  params: UserQueryParams = {}
): Promise<PaginatedResponse<User>> => {
  const url = USERS_PAGINATED(pageNumber, pageSize);
  const response = await axios.get<PaginatedResponse<User>>(url, { params });
  return response.data;
}

/**
 * Fetch a single user by their ID.
 */
export const getUserById = async (id: string): Promise<User> => {
  const response = await axios.get<User>(USER_BY_ID(id));
  return response.data;
}

/**
 * Create a new user.
 */
export const createUser = async (user: Partial<FormData>): Promise<FormData> => {
  const response = await axios.post<FormData>(USERS, user);
  return response.data;
}

/**
 * Update an existing user by ID.
 */
export const updateUser = async (id: string, user: Partial<FormData>): Promise<FormData> => {
  const response = await axios.put<FormData>(USER_BY_ID(id), user);
  return response.data;
}

/**
 * Delete a user by ID.
 */
export const deleteUser = async (id: string, imageUrl: string): Promise<void> => {
  await axios.delete(`${USER_BY_ID(id)}?imageUrl=${encodeURIComponent(imageUrl)}`);
}

const userRepository = {
  /**
   * Fetch a user by their ID.
   */
  async getById(id: string): Promise<User> {
    try {
      const response = await axios.get(`${API_URL}/${id}`);
      return response.data as User;
    } catch (error) {
      console.error("Error fetching user:", error);
      throw error;
    }
  },

  /**
   * Search for users by name.
   */
  async searchByName(name: string): Promise<User[]> {
    try {
      const response = await axios.get(`${API_URL}?name=${name}`);
      return response.data as User[];
    } catch (error) {
      console.error("Error searching for users:", error);
      throw error;
    }
  },
};

export default userRepository;
