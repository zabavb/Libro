export const getAuthHeaders = () => {
  const token = localStorage.getItem('token');
  if (!token) {
    console.error('No token found!');
    return {};
  }
  return {
    Authorization: `Bearer ${token}`,
    'Content-Type': 'application/json',
  };
};
