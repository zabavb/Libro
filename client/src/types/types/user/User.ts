export interface User {
  id: string;
  lastName: string | null;
  firstName: string;
  email: string | null;
  phoneNumber: string | null;
  dateOfBirth: Date | null;
  role: number;
  imageUrl: string;
}
