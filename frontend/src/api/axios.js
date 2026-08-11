import axios from "axios";

// связь с БД
const instance = axios.create({
  baseURL: "https://localhost:7001/api",
  withCredentials: true,
});


// 
instance.interceptors.request.use(config => {
  const token = localStorage.getItem("AccessToken");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default instance;