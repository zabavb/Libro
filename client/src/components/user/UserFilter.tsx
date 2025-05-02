import React from 'react';
import { EmailViewDomen, RoleView, UserViewFilter } from '../../types';
import SubscriptionDropdown from './SubscriptionDropdown';

interface UserFilterProps {
  filters: UserViewFilter;
  onFilterChange: (filters: UserViewFilter) => void;
}

const UserFilter: React.FC<UserFilterProps> = ({ onFilterChange, filters }) => {
  return (
    <div>
      <h3>Filters</h3>
      <select
        value={filters.email || ''}
        onChange={(e) =>
          onFilterChange({
            ...filters,
            email:
              e.target.value === '' ? null : (e.target.value as EmailViewDomen),
          })
        }
      >
        <option value=''>Filter by email</option>
        {Object.entries(EmailViewDomen).map(([key, value]) => (
          <option key={key} value={key}>
            {value}
          </option>
        ))}
      </select>
      <select
        value={filters.role || ''}
        onChange={(e) =>
          onFilterChange({
            ...filters,
            role: e.target.value === '' ? null : (e.target.value as RoleView),
          })
        }
      >
        <option value=''>Filter by role</option>
        {Object.entries(RoleView).map(([key, value]) => (
          <option key={key} value={key}>
            {value}
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
