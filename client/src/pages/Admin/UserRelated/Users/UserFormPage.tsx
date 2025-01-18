import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import UserForm from "../../../../components/user/UserForm";
import { useSelector } from "react-redux";
import { RootState } from "../../../../state/redux/store";

const UserFormPage = () => {
  const { userId } = useParams<{ userId: string }>();
  const navigate = useNavigate();
  const users = useSelector((state: RootState) => state.users.data);
  const [existingUser, setExistingUser] = useState<
    typeof users[0] | undefined
  >(undefined);

  useEffect(() => {
    if (userId) {
      const user = users.find((u) => u.id === userId);
      if (user) {
        setExistingUser(user);
      }
    }
  }, [userId, users]);

  const handleGoBack = () => {
    navigate("/admin/users");
  };

  return (
    <div>
      <header>
        <h1>{userId ? "Edit User" : "Add User"}</h1>
        <button onClick={handleGoBack}>Back to User List</button>
      </header>
      <main>
        <UserForm existingUser={existingUser} />
      </main>
    </div>
  );
};

export default UserFormPage;
