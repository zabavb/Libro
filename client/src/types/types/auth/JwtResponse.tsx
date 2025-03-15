import { SetStateAction } from 'react';
import { User } from '../user/User';

export interface JwtResponse {
  token: string;
  expiresIn: number;
  user: SetStateAction<User | null>;
}
