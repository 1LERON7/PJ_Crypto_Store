import { Navigate } from "react-router-dom";
{/* библиотека, из токена в объект */}
import { jwtDecode } from "jwt-decode";

export default function AdminRoute({ children }) {
  const token = localStorage.getItem("AccessToken");

  if (!token) {
    return <Navigate to="/" />;
  }

  try {
    const decoded = jwtDecode(token);

    const role = decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    // console.log(decoded);
    
    if (role !== "admin") {
      return <Navigate to="/" />;
    }

    return children;

  } catch {
    return <Navigate to="/" />;
  }
}