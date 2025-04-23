import { EmailDomen } from '../subTypes/user/EmailDomen';
import { Role } from '../subTypes/user/Role';

export interface UserFilter {
  email?: EmailDomen;
  role?: Role;
  subscriptionId?: string;
}
