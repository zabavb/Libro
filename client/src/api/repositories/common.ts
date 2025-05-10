export const getAuthHeaders = (contentType: string) => {
  const token = localStorage.getItem('token');
  if (!token) {
    console.error('No token found!');
    return {};
  }
  return {
    Authorization: `Bearer ${token}`,
    'Content-Type': contentType,
  };
};
