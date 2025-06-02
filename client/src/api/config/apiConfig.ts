import { PeriodType } from "@/types/types/order/PeriodType";

const GATEWAY = `https://localhost:7102/gateway`;
const AUTH = `${GATEWAY}/auth`;
const USERS = `${GATEWAY}/users`;
const SUBSCRIPTIONS = `${GATEWAY}/subscriptions`;
const PASSWORD = `${GATEWAY}/password`;
const ORDERS = `${GATEWAY}/orders`;
const DELIVERY = `${GATEWAY}/deliverytypes`;
const BOOKS = `${GATEWAY}/books`;
const AUTHORS = `${GATEWAY}/authors`
const PUBLISHERS = `${GATEWAY}/publishers`
const FEEDBACKS = `${GATEWAY}/feedbacks`
const CATEGORIES = `${GATEWAY}/categories`
const SUBCATEGORIES = `${GATEWAY}/subcategories`

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
    ORDER_COUNTS: (period: PeriodType) => `${ORDERS}/counts/${period}`,
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
  AUTHORS: {
    BASE: AUTHORS,
    PAGINATED: (pageNumber: number, pageSize: number) => 
      `${AUTHORS}?pageNumber=${pageNumber}&pageSize=${pageSize}`,
    BY_ID: (id:string) => `${AUTHORS}/${id}`,
  },
  PUBLISHERS: {
    BASE:PUBLISHERS,
    PAGINATED: (pageNumber: number, pageSize: number) => 
      `${PUBLISHERS}?pageNumber=${pageNumber}&pageSize=${pageSize}`,
    BY_ID: (id:string) => `${PUBLISHERS}/${id}`,
  },
  FEEDBACKS: {
    BASE:FEEDBACKS,
    PAGINATED: (pageNumber: number, pageSize: number) =>
      `${FEEDBACKS}?pageNumber=${pageNumber}&pageSize=${pageSize}`,
    BY_ID: (id:string) => `${FEEDBACKS}/${id}`,
  },
  CATEGORIES: {
    BASE: CATEGORIES,
    PAGINATED: (pageNumber: number, pageSize: number) =>
      `${CATEGORIES}?pageNumber=${pageNumber}&pageSize=${pageSize}`,
    BY_ID: (id:string) => `${CATEGORIES}/${id}`,
  },
  SUBCATEGORIES:{
    BASE: SUBCATEGORIES,
    PAGINATED: (pageNumber: number, pageSize: number) =>
      `${SUBCATEGORIES}?pageNumber=${pageNumber}&pageSize=${pageSize}`,
    BY_ID: (id:string) => `${SUBCATEGORIES}/${id}`,
  },
};
