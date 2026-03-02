import { Link, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import { profile } from "../api/auth";
import BackHeader from "../components/HeaderBack";


console.log("TOKEN:", localStorage.getItem("AccessToken"));

export default function Profile() {
    const navigate = useNavigate();

    const [email, setEmail] = useState(null);
    const [role, setRole] = useState("user");
    const [createdAt, setCreatedAt] = useState(null);
    const [loading, setLoading] = useState(true);

    const handleLogout = () => {
    localStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
    navigate("/auth/login", { replace: true });
  };

    // Эффект - хук, выполняется когда меняються зависимости
    useEffect(() => {
        // Api запрос к Бэку
        profile().then(data => {
            // данные кладем в стан
            setEmail(data.email);
            setRole(data.role);
            console.log("createdAt from backend:", data.createdAt);
            setCreatedAt(data.createdAt);
        })
        .catch(err => {
            console.error("Not authorized", err);
            localStorage.removeItem("accessToken");
            localStorage.removeItem("refreshToken");
            navigate("/auth/login");
        })
        // пока идет ответ от бд, будет "загрузка"
        .finally(() => setLoading(false));
    }, [navigate]);

    if (loading) {
    return <div className="container mt-5">Loading...</div>;
  }

  return (
    <>
    <BackHeader/>
    <div className="container mt-5" style={{ maxWidth: "600px" }}>
      <div className="card shadow-sm p-4">

        <h4 className="mb-4 text-center">Profile</h4>

        <div className="mb-3">
          <small className="text-muted">Email</small>
          <div className="fs-5">{email}</div>
        </div>

        <div className="mb-3">
          <small className="text-muted">Role</small>
          <div>
            <span className="badge bg-primary">{role}</span>
          </div>
        </div>

        <div className="mb-4">
          <small className="text-muted">Account created</small>
          <div>
            {createdAt
                ? new Date(createdAt).toLocaleString()
                : "—"}
            </div>
        </div>

        <div className="d-flex justify-content-end">


          <button className="btn btn-outline-danger" onClick={handleLogout}>
            Logout
          </button>

        </div>

      </div>
    </div>
    </>
  );
}