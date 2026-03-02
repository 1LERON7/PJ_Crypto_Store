import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { register, login } from "../api/auth";
import HeaderAuth from "../components/HeaderAuth";


export default function Register() {
  const navigate = useNavigate();

  const [email, setEmail] = useState("");
  // const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState(null);

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      // ждем ответ от БД что мы зарегались
      await register({ email, password });

      // после рега сразу логинимся
      const result = await login({email, password});

      // после лога дают токен, мы его присваиваем 
      localStorage.setItem("AccessToken", result.accessToken);
      localStorage.setItem("RefreshToken", result.refreshToken);

      navigate("/"); 
      
    } catch (err) {
      console.log(err);
      setError("Registration failed");
    }
  };

  return (
    <>
    <HeaderAuth/>
    <div className="container d-flex justify-content-center align-items-center vh-100">
      <div className="col-12 col-md-6 col-lg-4">
        <form onSubmit={handleSubmit} className="p-4 shadow rounded bg-white">
          <h3 className="text-center mb-4">Register</h3>

          {error && <div className="alert alert-danger">{error}</div>}

          {/* <input
            className="form-control mb-3"
            placeholder="Username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          /> */}

          <input
            className="form-control mb-3"
            placeholder="Email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />

          <input
            type="password"
            className="form-control mb-3"
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />

          <button className="btn btn-primary w-100">
            Register
          </button>

          <p className="text-center mt-3">
            Already have an account? <a href="/auth/login">Login</a>
          </p>
        </form>
      </div>
    </div>
    </>
  );
}