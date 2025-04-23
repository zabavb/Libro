import React from 'react';
import { Bool, type UserSort } from '../../types';

interface UserSortProps {
  onSortChange: (updatedSort: UserSort) => void;
  sort: UserSort;
}

const cycleSortValue = (current: Bool | undefined): Bool => {
  switch (current) {
    case Bool.ASCENDING:
      return Bool.DESCENDING;
    case Bool.DESCENDING:
      return Bool.NULL;
    default:
      return Bool.ASCENDING;
  }
};

const getArrow = (value?: Bool) => {
  if (value === Bool.ASCENDING) return '↑';
  if (value === Bool.DESCENDING) return '↓';
  return '';
};

const UserSort: React.FC<UserSortProps> = ({ onSortChange, sort }) => {
  const handleClick = (field: keyof UserSort) => {
    const newSortValue = cycleSortValue(sort[field]);
    onSortChange({ ...sort, [field]: newSortValue });
  };

  return (
    <div>
      <button onClick={() => handleClick('alphabetical')}>
        Alphabetical {getArrow(sort.alphabetical)}
      </button>
      <button onClick={() => handleClick('youngest')}>
        Age {getArrow(sort.youngest)}
      </button>
      <button onClick={() => handleClick('roleSort')}>
        Role {getArrow(sort.roleSort)}
      </button>
    </div>
  );
};

export default UserSort;
