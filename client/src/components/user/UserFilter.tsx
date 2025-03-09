import React, { useState } from "react";
import userRepository from "../../api/repositories/userRepository";
import { debounce } from "lodash";
import type { UserFilter, User } from "../../types";
import { Role } from "../../types";

interface UserFilterProps {
  filters: UserFilter;
  onFilterChange: (filters: UserFilter) => void;
  onSelectUser: (userId: string) => void;
}

const UserFilter: React.FC<UserFilterProps> = ({ filters, onFilterChange, onSelectUser }) => {
  const [query, setQuery] = useState("");
  const [users, setUsers] = useState<User[]>([]);

  const searchUsers = debounce(async (name: string) => {
    if (name.length > 2) {
      const results = await userRepository.searchByName(name);
      setUsers(results);
    }
  }, 500);

  const handleSearchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setQuery(e.target.value);
    searchUsers(e.target.value);
  };

  return (
    <div>
      <h3>Filters</h3>
      <label>
        Date of Birth (Start):
        <input
          type="date"
          value={filters.dateOfBirthStart ? filters.dateOfBirthStart : ""}
          onChange={(e) =>
            onFilterChange({
              ...filters,
              dateOfBirthStart: e.target.value,
            })
          }
        />
      </label>
      <label>
        Date of Birth (End):
        <input
          type="date"
          value={filters.dateOfBirthEnd ? filters.dateOfBirthEnd : ""}
          onChange={(e) =>
            onFilterChange({
              ...filters,
              dateOfBirthEnd: e.target.value,
            })
          }
        />
      </label>
      <label>
        Email:
        <select
          value={filters.email || ""}
          onChange={(e) => onFilterChange({ ...filters, email: e.target.value })}>
          <option value="">All</option>
          <option value="@gmail.com">Gmail</option>
          <option value="@outlook.com">Outlook</option>
          <option value="@hotmail.com">Hotmail</option>
          <option value="@icloud.com">iCloud</option>
          <option value="@yahoo.com">Yahoo</option>
          <option value="@msn.com">MSN</option>
          <option value="@aol.com">AOL</option>
          <option value="@comcast.com">Comcast</option>
        </select>
      </label>
      <label>
        Role:
        <select
          value={filters.role || ""}
          onChange={(e) => onFilterChange({ ...filters, role: e.target.value as Role })}>
          <option value="">All</option>
          {Object.values(Role).map((role) => (
            <option key={role} value={role}>
              {role}
            </option>
          ))}
        </select>
      </label>
      <label>
        Has Subscription:
        <input
          type="checkbox"
          checked={filters.hasSubscription ?? false}
          onChange={(e) => onFilterChange({ ...filters, hasSubscription: e.target.checked })}
        />
      </label>

      <h3>Search Users</h3>
      <input
        type="text"
        value={query}
        onChange={handleSearchChange}
        placeholder="Search for a user..."
      />
      <ul>
        {users.map((user) => (
          <li key={user.id} onClick={() => onSelectUser(user.id)}>
            {user.name}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default UserFilter;
