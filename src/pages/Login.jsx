import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { login } from "../api/auth";
import HeaderAuth from "../components/HeaderAuth";
import { useAuth } from "../components/AuthContext";

export default function Login() {
  const navigate = useNavigate();
  const { loginSuccess } = useAuth();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState(null);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(null);

    try {
      const result = await login({ email, password });

      // 🔥 ВАЖНАЯ ПРОВЕРКА
      if (result?.accessToken && result?.refreshToken) {
        loginSuccess(result.accessToken, result.refreshToken);

        // лучше replace, чтобы BACK не возвращал на login
        navigate("/", { replace: true });
      } else {
        setError("Login failed");
      }
    } catch (err) {
      console.log(err);
      setError("Invalid email or password");
    }
  };

  return (
    <>
      <HeaderAuth />

      <div className="container d-flex justify-content-center align-items-center vh-100">
        <div className="col-12 col-md-6 col-lg-4">
          <form onSubmit={handleSubmit} className="p-4 shadow rounded bg-white">
            <h3 className="text-center mb-4">Login</h3>

            {error && <div className="alert alert-danger">{error}</div>}

            <input
              className="form-control mb-3"
              placeholder="Email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />

            <input
              type="password"
              className="form-control mb-3"
              placeholder="Password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />

            <button className="btn btn-primary w-100">
              Login
            </button>
          </form>
        </div>
      </div>
    </>
  );
}