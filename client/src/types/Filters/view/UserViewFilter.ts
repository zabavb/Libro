import { EmailViewDomen, RoleView } from '@/types';

export interface UserViewFilter {
  email: EmailViewDomen | null;
  role: RoleView | null;
  subscriptionId: string | null;
}
