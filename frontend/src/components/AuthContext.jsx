import { createContext, useContext, useEffect, useState } from "react";
import { jwtDecode } from "jwt-decode";

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [auth, setAuth] = useState(false);
  const [role, setRole] = useState(null);

  // при старте приложения читаем токен
  useEffect(() => {
    const token = localStorage.getItem("AccessToken")

    if(!token) return;
    
    try {
    const decoed = jwtDecode(token);

      setAuth(true);
      setRole(decoed.role);
    } catch (err) {
      console.error("Invalid token", err);          //

      localStorage.removeItem("AccessToken");
      localStorage.removeItem("RefreshToken");

      setAuth(false);
      setRole(null);
    }
  }, []);

  const loginSuccess = (accessToken, refreshToken) => {
    localStorage.setItem("AccessToken", accessToken);
    localStorage.setItem("RefreshToken", refreshToken);


    try {
    const decoed = jwtDecode(accessToken);

    setAuth(true);
    setRole(decoed.role);
    } catch  {
        console.error("Invalid login token");       //
    }
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