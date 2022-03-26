import axios from "axios";
import { variables } from "../Variables";

export default axios.create({
  baseURL: variables.IDENTITY_API_URL
});

export const axiosPrivate = axios.create({
  baseURL: variables.IDENTITY_API_URL,
  headers: { 'Content-Type': 'application/json' },
  withCredentials: true
});