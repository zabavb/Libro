const GATEWAY = `https://localhost:7102/gateway`;
const AUTH = `${GATEWAY}/auth`;
const USERS = `${GATEWAY}/users`;
const SUBSCRIPTIONS = `${GATEWAY}/subscriptions`;
const PASSWORD = `${GATEWAY}/password`;
const ORDERS = `${GATEWAY}/orders`;
const DELIVERY = `${GATEWAY}/deliverytypes`;
const BOOKS = `${GATEWAY}/books`;

export const API_ROUTES = {
  GRAPHQL: `${GATEWAY}/graphql`,
  AUTH: {
    LOGIN: `${AUTH}/login`,
    REGISTER: `${AUTH}/register`,
    OAUTH: `${AUTH}/callback`,
  },
  USERS: {
    BASE: USERS,
    PAGINATED: (pageNumber: number, pageSize: number) =>
      `${USERS}?pageNumber=${pageNumber}&pageSize=${pageSize}`,
    BY_ID: (id: string) => `${USERS}/${id}`,
  },
  SUBSCRIPTIONS: {
    BASE: SUBSCRIPTIONS,
    PAGINATED: (pageNumber: number, pageSize: number) =>
      `${SUBSCRIPTIONS}?pageNumber=${pageNumber}&pageSize=${pageSize}`,
    BY_ID: (id: string) => `${SUBSCRIPTIONS}/${id}`,
    SUBSCRIBE: `${SUBSCRIPTIONS}/subscribe`,
    UNSUBSCRIBE: `${SUBSCRIPTIONS}/unsubscribe`,
    FOR_FILTERING: `${SUBSCRIPTIONS}/filter`,
  },
  PASSWORD: {
    BASE: (userId: string) => `${PASSWORD}/${userId}`,
  },
  ORDERS: {
    BASE: ORDERS,
    PAGINATED: (pageNumber: number, pageSize: number) =>
      `${ORDERS}?pageNumber=${pageNumber}&pageSize=${pageSize}`,
    BY_ID: (id: string) => `${ORDERS}/${id}`,
  },
  DELIVERY: {
    BASE: DELIVERY,
    PAGINATED: (pageNumber: number, pageSize: number) =>
      `${DELIVERY}?pageNumber=${pageNumber}&pageSize=${pageSize}`,
    BY_ID: (id: string) => `${DELIVERY}/${id}`,
  },
  BOOKS: {
    BASE: BOOKS,
    PAGINATED: (pageNumber: number, pageSize: number) =>
      `${BOOKS}?pageNumber=${pageNumber}&pageSize=${pageSize}`,
    BY_ID: (id: string) => `${BOOKS}/${id}`,
  },
};
