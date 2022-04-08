import useAuth from "./useAuth";
import { variables } from "../Variables";
import axios from "../api/axios";

const useRefreshToken = () => {
  const { setAuth } = useAuth();

  const refresh = async () => {
    const response = await axios.get(variables.IDENTITY_API_URL + "refresh", {
      headers: {'Content-Type': 'application/json'},
      withCredentials: true
    });

    const accessToken = response?.data?.accessToken;

    setAuth(prev => {
      console.log(`Previous auth ${JSON.stringify(prev)}`);
      console.log(`New token ${JSON.stringify(response.data.accessToken)}`);

      const roles = response?.data?.roles?.split('');
      return {
        ...prev,
        email: localStorage.getItem('email'),
        accessToken: accessToken,
        roles: roles
      }
    });

    return accessToken;
  }

  return refresh;
};

export default useRefreshToken;