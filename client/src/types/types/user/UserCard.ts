import { Role } from '../../subTypes/user/Role';

export interface UserCard {
  id: string;
  fullName: string;
  email: string;
  phoneNumber: string;
  role: Role;

  order: {
    ordersCount: number;
    lastOrder: string;
  };
}
