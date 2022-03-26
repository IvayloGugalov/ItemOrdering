import useAuth from "./useAuth";
import { variables } from "../Variables";
import axios from "../api/axios";

const useRefreshToken = () => {
  const { setAuth } = useAuth();
  

  const refresh = async () => {
    const cookies = document.cookie.replace('access_token=', '');
    console.log(cookies);
    const response = await axios.post(variables.IDENTITY_API_URL + "refresh", {
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${cookies}` }
    });

    setAuth(prev => {
      console.log(JSON.stringify(prev));
      console.log(JSON.stringify(response.data.accessToken));
      return {
        ...prev,
        accessToken: response.data.accessToken,
        roles: response.data.roles}
    });

    return response.data.accessToken;
  }

  return refresh;
};

export default useRefreshToken;