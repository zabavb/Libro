import axios from 'axios';
import { USERS, USER_BY_ID, GRAPHQL } from '../index';
import {
  User,
  PaginatedResponse,
  UserCard,
  UserForm,
  UserFilter,
  UserSort,
  GraphQLResponse,
} from '../../types';
import { getAuthHeaders } from './common';

/**
 * Fetch a paginated list of users with optional filters.
 */
export const getAllUsers = async (body: {
  query: string;
  variables: {
    pageNumber: number;
    pageSize: number;
    searchTerm: string | null;
    filter: UserFilter;
    sort: UserSort;
  };
}): Promise<GraphQLResponse<{ allUsers: PaginatedResponse<UserCard> }>> => {
  const response = await axios.post<
    GraphQLResponse<{ allUsers: PaginatedResponse<UserCard> }>
  >(GRAPHQL, body, {
    headers: getAuthHeaders(),
  });
  return response.data;
};

/**
 * Fetch a single user by their ID.
 */
export const getUserById = async (body: {
  query: string;
  variables: {
    id: string;
  };
}): Promise<GraphQLResponse<{ user: UserForm }>> => {
  const response = await axios.post<GraphQLResponse<{ user: UserForm }>>(
    GRAPHQL,
    body,
    {
      headers: getAuthHeaders(),
    },
  );
  return response.data;
};

/**
 * Create a new user.
 */
export const createUser = async (user: Partial<User>): Promise<User> => {
  const response = await axios.post<User>(USERS, user);
  return response.data;
};

/**
 * Update an existing user by ID.
 */
export const updateUser = async (
  id: string,
  user: Partial<User>,
): Promise<User> => {
  const response = await axios.put<User>(USER_BY_ID(id), user, {
    headers: getAuthHeaders(),
  });
  return response.data;
};

/**
 * Delete a user by ID.
 */
export const deleteUser = async (id: string): Promise<void> => {
  const url = USER_BY_ID(id);
  const response = await axios.delete(url, {
    headers: getAuthHeaders(),
  });
  return response.data;
};
