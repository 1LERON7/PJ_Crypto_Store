import logo from "../assets/ss4.png";
import "./style.css";
import { Link } from "react-router-dom";

export default function Header() {
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

      <div className="d-flex gap-2">

        <Link
          to="/"
          className="btn btn-link p-0 text-decoration-none d-inline-flex align-items-center"
          aria-label="Back to home"
        >
          <span className="me-2" style={{ fontSize: 22, lineHeight: 1 }}>←</span>
          <span className="fw-semibold">Back</span>
        </Link>
          

      </div>
    </nav>
  );
}