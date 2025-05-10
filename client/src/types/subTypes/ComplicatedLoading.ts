export interface ComplicatedLoading<FormData> {
  isLoading: boolean;
  fieldName: 'all' | keyof FormData;
}
