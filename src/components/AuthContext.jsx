import { createContext, useContext, useEffect, useState } from "react";
import { jwtDecode } from "jwt-decode";

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [auth, setAuth] = useState(false);
  const [role, setRole] = useState(null);

  // при старте приложения читаем токен
  useEffect(() => {
    // setAuth(!!localStorage.getItem("AccessToken"));
    const token = localStorage.getItem("AccessToken")

    if(token){
      const decoed = jwtDecode(token);

      setAuth(true);
      setRole(decoed.role);
    }
  }, []);

  const loginSuccess = (accessToken, refreshToken) => {
    localStorage.setItem("AccessToken", accessToken);
    localStorage.setItem("RefreshToken", refreshToken);

    const decoed = jwtDecode(accessToken);

    setAuth(true);
    setRole(decoed.role);
  };

  const logout = () => {
    localStorage.removeItem("AccessToken");
    localStorage.removeItem("RefreshToken");
    setAuth(false);
    setRole(null);
  };

  return (
    <AuthContext.Provider value={{ auth, role, loginSuccess, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  return useContext(AuthContext);
}