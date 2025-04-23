import { Role, User } from '../../types';
import { UserFormData } from '../../utils';

export const roleNumberToEnum = (roleNumber: number): Role => {
  const roleMap: { [key: number]: Role } = {
    0: Role.ADMIN,
    1: Role.MODERATOR,
    2: Role.USER,
  };

  return roleMap[roleNumber] ?? Role.USER;
};

export const roleEnumToNumber = (role: Role): number => {
  const roleMap: { [key in Role]: number } = {
    [Role.ADMIN]: 0,
    [Role.MODERATOR]: 1,
    [Role.USER]: 2,
  };

  return roleMap[role] ?? 2;
};

export const UserFormDataToUser = (form: UserFormData): User => ({
  ...form,
  id: '00000000-0000-0000-0000-000000000000',
  lastName: form.lastName ?? null,
  email: form.email ?? null,
  phoneNumber: form.phoneNumber ?? null,
  dateOfBirth: form.dateOfBirth ? new Date(form.dateOfBirth) : null,
  role: roleEnumToNumber(form.role),
  imageUrl: '',
});
