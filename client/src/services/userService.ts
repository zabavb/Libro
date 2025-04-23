import {
  User,
  PaginatedResponse,
  UserFilter,
  UserSort,
  UserCard,
  UserForm,
  ServiceResponse,
  Bool,
} from '../types';
import {
  getAllUsers,
  getUserById,
  createUser,
  updateUser,
  deleteUser,
} from '../api/repositories/userRepository';

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
    const defaultFilter: UserFilter = {
      email: null,
      roleFilter: null,
      subscriptionId: null,
    };

    const defaultSort = {
      alphabetical: Bool.NULL,
      youngest: Bool.NULL,
      roleSort: Bool.NULL,
    } as UserSort;

    const body = {
      query: `
        query GetAllUsers(
          $pageNumber: Int!,
          $pageSize: Int!,
          $searchTerm: String,
          $filter: UserFilterInput!,
          $sort: UserSortInput!
        ) {
          allUsers(
            pageNumber: $pageNumber,
            pageSize: $pageSize,
            searchTerm: $searchTerm,
            filter: $filter,
            sort: $sort
          ) {
            items {
              id
              fullName
              email
              phoneNumber
              role
              order {
                ordersCount
                lastOrder
              }
            }
            pageNumber
            pageSize
            totalCount
            totalPages
          }
        }`,
      variables: {
        pageNumber,
        pageSize,
        searchTerm: searchTerm ?? null,
        filter: {
          ...defaultFilter,
          ...filters,
        },
        sort: {
          ...defaultSort,
          ...sort,
        },
      },
    };

    const graphQLResponse = await getAllUsers(body);
    if (graphQLResponse.errors)
      throw new Error(
        `${graphQLResponse.errors[0].message} Status code: ${graphQLResponse.errors[0].extensions?.status}`,
      );

    response.data = graphQLResponse.data
      ?.allUsers as PaginatedResponse<UserCard>;
  } catch (error) {
    console.error(error instanceof Error ? error.message : String(error));
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
    const body = {
      query: `query(
        $id: UUID!
      ) {
        user(
          id: $id
        ) {
          id
          lastName
          firstName
          email
          phoneNumber
          dateOfBirth
          role
          imageUrl
          orders {
            orderUiId
            bookNames
            price
          }
          feedbacksCount
          feedbacks {
            headLabel
            rating
            comment
            date
          }
          subscriptions {
            title
            description
            imageUrl
          }
        }
      }`,
      variables: {
        id: id,
      },
    };

    const graphQLResponse = await getUserById(body);
    if (graphQLResponse.errors)
      throw new Error(
        `${graphQLResponse.errors[0].message} Status code: ${graphQLResponse.errors[0].extensions?.status}`,
      );

    response.data = graphQLResponse.data?.user as UserForm;
  } catch (error) {
    console.error(error instanceof Error ? error.message : String(error));
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
    console.log('Creating user:', user);
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
