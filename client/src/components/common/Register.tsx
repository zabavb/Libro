import {icons} from '@/lib/icons'
import { User } from '@/types';
import { RegisterFormData } from '@/utils';

interface RegisterProps {
  data: User | undefined;
  onSubmit: (userData: RegisterFormData) => Promise<void>;
  setIsRegistration: (isRegistration: boolean) => void;
}

const Register: React.FC<RegisterProps> = ({ data, onSubmit, setIsRegistration }) => {

    return (
        <>
          <p onClick={() => setIsRegistration(false)}>LOGIN</p>
        </>
    )
}

export default Register;