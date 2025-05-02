export interface GraphQLResponse<T> {
  errors: Array<{
    message: string;
    locations?: Array<{ line: number; column: number }>;
    path?: Array<string | number>;
    extensions?: {
      code: string;
      status: number;
    };
  }> | null;
  data: T | null;
}
