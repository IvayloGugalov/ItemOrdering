import { useNavigate } from "react-router-dom";
import useAxiosPrivate from './useAxiosPrivate';
import useAuth from "./useAuth";
import { variables } from "../Variables";

const useLogOut = () => {
  const axiosPrivate = useAxiosPrivate();
  const { setAuth } = useAuth();
  const navigate = useNavigate();

  const logOut = async () => {
    try {
      const response = await axiosPrivate.delete(variables.IDENTITY_API_URL + "logout", {
        headers: { 'Content-Type': 'application/json' },
        withCredentials: true
      });

      setAuth({});
      localStorage.removeItem('email');
    }
    catch (err) {
      console.error(err);
      navigate('/login');
      setAuth({});
    }
  }

  return logOut;
}

export default useLogOut;