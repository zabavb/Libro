import { Role, RoleView, User, UserFilter, UserViewFilter } from '../../types';
import { UserFormData } from '../../utils';

export const numberToRoleView = (roleNumber: number): RoleView => {
  const roleMap: { [key: number]: RoleView } = {
    0: RoleView.ADMIN,
    1: RoleView.MODERATOR,
    2: RoleView.USER,
  };

  return roleMap[roleNumber] ?? RoleView.USER;
};

export const roleViewToNumber = (role: RoleView): number => {
  const roleMap: { [key in RoleView]: number } = {
    [RoleView.ADMIN]: 0,
    [RoleView.MODERATOR]: 1,
    [RoleView.USER]: 2,
  };

  return roleMap[role] ?? 2;
};

export const roleEnumToNumber = (role: Role): number => {
  const roleMap: { [key in Role]: number } = {
    [Role.ADMIN]: 0,
    [Role.MODERATOR]: 1,
    [Role.USER]: 2,
  };

  return roleMap[role] ?? 2;
};

export const roleEnumToRoleView = (role: Role): RoleView => {
  const roleMap: { [key in Role]: RoleView } = {
    [Role.ADMIN]: RoleView.ADMIN,
    [Role.MODERATOR]: RoleView.MODERATOR,
    [Role.USER]: RoleView.USER,
  };

  return roleMap[role] ?? RoleView.USER;
};

export const UserFormDataToUser = (form: UserFormData, id?: string): User => ({
  ...form,
  id: id ?? '00000000-0000-0000-0000-000000000000',
  lastName: form.lastName ?? null,
  email: form.email ?? null,
  phoneNumber: form.phoneNumber ? form.phoneNumber : null,
  dateOfBirth: form.dateOfBirth ? new Date(form.dateOfBirth) : null,
  role: roleViewToNumber(form.role),
  imageUrl: '',
});

export const FromUserViewFilterToUserFilter = (
  view: UserViewFilter,
): UserFilter => {
  return {
    email: view.email ?? null,
    roleFilter: view.role ?? null,
    subscriptionId: view.subscriptionId ?? null,
  } as UserFilter;
};

export const FromUserFilterToUserViewFilter = (
  view: UserFilter,
): UserViewFilter => {
  return {
    email: view.email ?? null,
    role: view.roleFilter ?? null,
    subscriptionId: view.subscriptionId ?? null,
  } as UserViewFilter;
};
