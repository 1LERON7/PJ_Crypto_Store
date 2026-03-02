import { Navigate } from "react-router-dom";

export default function ProtectedRoute({ children }) {
  const token = localStorage.getItem("AccessToken");

  if (!token) {
    return <Navigate to="/auth/login" replace />;
  }

  return children;
}