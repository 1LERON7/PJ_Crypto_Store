import axios from "./axios";

export const login = async (data) => {
  const response = await axios.post("/auth/login", data);
  return response.data;
};

export const register = async (data) => {
  const response = await axios.post("/auth/register", data);
  return response.data;
};

export const profile = async () => {
  const response = await axios.get("/users/profile");
  return response.data;
}

export const logout = async () => {
  const response = await axios.post("/auth/logout");
  return response.data;
}