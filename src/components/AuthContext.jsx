import { createContext, useContext, useEffect, useState } from "react";

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [auth, setAuth] = useState(false);

  // при старте приложения читаем токен
  useEffect(() => {
    setAuth(!!localStorage.getItem("AccessToken"));
  }, []);

  const loginSuccess = (accessToken, refreshToken) => {
    localStorage.setItem("AccessToken", accessToken);
    localStorage.setItem("RefreshToken", refreshToken);
    setAuth(true);
  };

  const logout = () => {
    localStorage.removeItem("AccessToken");
    localStorage.removeItem("RefreshToken");
    setAuth(false);
  };

  return (
    <AuthContext.Provider value={{ auth, loginSuccess, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  return useContext(AuthContext);
}