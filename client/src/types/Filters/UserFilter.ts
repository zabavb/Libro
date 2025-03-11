import { Role } from "../subTypes/Role"
import { Subscription } from "../../types/types/order/Subscription";
export interface UserFilter {
	dateOfBirthStart?: string
	dateOfBirthEnd?: string
	email?: string
	role?: Role
	hasSubscription?: boolean
}
export const filterSubscriptions = (subscriptions: Subscription[], status: string) => {
	return subscriptions.filter((sub) => sub.status === status);
  };
