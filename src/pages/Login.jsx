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

    const emailParts = email.split("@");

    if (emailParts[0].length < 3) {
      setError("Email must have at least 3 characters before @");
      return;
    }
    
    if(!email.trim()){
      setError("Email is required");
      return;
    }

    if (!/\S+@\S+\.\S+/.test(email)) {
    setError("Invalid email format");
    return;
  }

  if (password.length < 6) {
    setError("Password must be at least 6 characters");
    return;
  }
  
  if(email.length > 100){
    setError("Email is so long");
  }

  if (!/[A-Za-z]/.test(password) || !/[0-9]/.test(password)) {
    setError("Password must contain letters and numbers");
    return;
  }

    // e.preventDefault();
    setError(null);

    try {
      const result = await login({ email, password });

      if (result?.accessToken && result?.refreshToken) {
        loginSuccess(result.accessToken, result.refreshToken);

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
              type="email"
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