import React from 'react';
import type { UserFilter } from '../../types';
import { Role } from '../../types';
import { EmailDomen } from '../../types/subTypes/user/EmailDomen';
import SubscriptionDropdown from './SubscriptionDropdown';

interface UserFilterProps {
  filters: UserFilter;
  onFilterChange: (filters: UserFilter) => void;
}

const UserFilter: React.FC<UserFilterProps> = ({ onFilterChange, filters }) => {
  return (
    <div>
      <h3>Filters</h3>
      <select
        value={filters.email || ''}
        onChange={(e) =>
          onFilterChange({ ...filters, email: e.target.value as EmailDomen })
        }
      >
        <option value=''>Filter by email</option>
        {Object.values(EmailDomen).map((domen) => (
          <option key={domen} value={domen}>
            {domen}
          </option>
        ))}
      </select>
      <select
        value={filters.role || ''}
        onChange={(e) =>
          onFilterChange({ ...filters, role: e.target.value as Role })
        }
      >
        <option value=''>Filter by role</option>
        {Object.values(Role).map((role) => (
          <option key={role} value={role}>
            {role}
          </option>
        ))}
      </select>
      <SubscriptionDropdown
        onSelect={(value) =>
          onFilterChange({ ...filters, subscriptionId: value })
        }
      />
    </div>
  );
};

export default UserFilter;
