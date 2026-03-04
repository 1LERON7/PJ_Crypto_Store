import logo from "../assets/ss4.png";
import "./style.css";
import { Link, useNavigate } from "react-router-dom";
// import { useEffect, useState } from "react";
import { useAuth } from "./AuthContext";
import ThemeToggle from "./ThemeToggle";

export default function Header() {
  const { auth, logout } = useAuth();
  const navigate = useNavigate();
  // const [auth, setAuth] = useState(false);

  // useEffect(() => {
  //   setAuth(!!localStorage.getItem("AccessToken"));
  // }, []);

  return (
    <nav className="navbar navbar-light bg-color px-4">
      
      <div className="d-flex align-items-center">
        <img
          src={logo}
          alt="Store logo"
          style={{ height: 60 }}
        />
      </div>

      <div className="txt">
        <h3>STREAM STORE</h3>
      </div>

          

 {/* тернарный оператор, на визуализацию UI */}
    <div>
    {auth ? (
      
      
       <div className="d-flex gap-2">
       
            <ThemeToggle />

          <Link to="/profile" className="btn btn-outline-light">Profile</Link>

          <button
            className="btn btn-outline-danger"
            onClick={() => {
              logout();
              navigate("/auth/login", { replace: true });
            }}
          >
            Logout
          </button>
        </div>

    ) : (
      <div className="d-flex gap-2">

        <Link to="/auth/login" className="btn btn-outline-primary">
          Login
        </Link>

        <Link to="/auth/register" className="btn btn-primary">
          Register
        </Link>
          
      </div>
    )}
    
      

      </div>
    </nav>
  );
}