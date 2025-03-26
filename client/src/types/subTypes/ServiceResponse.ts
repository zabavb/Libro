export type ServiceResponse<T> = {
  data: T | null;
  loading: boolean;
  error: string | null;
};
