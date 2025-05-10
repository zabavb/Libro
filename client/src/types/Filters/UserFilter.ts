import { EmailDomen } from '../subTypes/user/EmailDomen';
import { Role } from '../subTypes/user/Role';

export interface UserFilter {
  email?: EmailDomen | null;
  roleFilter?: Role | null;
  subscriptionId?: string | null; 
}
