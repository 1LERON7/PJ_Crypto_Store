import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { register, login } from "../api/auth";
import HeaderAuth from "../components/HeaderAuth";
import "../components/style.css";

export default function Register() {
  const navigate = useNavigate();

  const [email, setEmail] = useState("");
  // const [username, setUsername] = useState("");
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
  <HeaderAuth />

  <div className="container d-flex justify-content-center align-items-center vh-100">
    <div className="col-12 col-md-6 col-lg-4">

      <form
        onSubmit={handleSubmit}
        className="card shadow-lg p-4 border-0"
      >
        <h3 className="text-center mb-4">Register</h3>

        {error && (
          <div className="alert alert-danger">
            {error}
          </div>
        )}

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

        <button className="btn btn-primary w-100 mb-3">
          Register
        </button>

        <p className="text-center mb-0">
          Already have an account?{" "}
          
          <Link to="/auth/login">
          Login
          </Link>
            
          
        </p>

      </form>

    </div>
  </div>
</>
  );
}