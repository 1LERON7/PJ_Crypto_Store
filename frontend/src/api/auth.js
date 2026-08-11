import axios from "./axios";

export const login = async (data) => {
  const response = await axios.post("/auth/login", data);

localStorage.removeItem("accessToken");
localStorage.removeItem("refreshToken");

localStorage.setItem("AccessToken", response.data.accessToken);
localStorage.setItem("RefreshToken", response.data.refreshToken);
  return response.data;
};

export const register = async (data) => {
  const response = await axios.post("/auth/register", data);

localStorage.removeItem("accessToken");
localStorage.removeItem("refreshToken");

localStorage.setItem("AccessToken", response.data.accessToken);
localStorage.setItem("RefreshToken", response.data.refreshToken);
  return response.data;
};

export const profile = async () => {
  const response = await axios.get("/users/profile");
  return response.data;
}

export const logout = async () => {
  const response = await axios.post("/auth/logout");
  
  localStorage.removeItem("AccessToken");
  localStorage.removeItem("RefreshToken");
  localStorage.removeItem("accessToken");
  localStorage.removeItem("refreshToken");
  return response.data;
}