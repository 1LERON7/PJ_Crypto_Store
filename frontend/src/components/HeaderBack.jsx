import { Link } from "react-router-dom";
import "./style.css";

export default function BackHeader({ title = "" }) {
  return (

    <header className="custom-header">
  <div className="container py-3 d-flex align-items-center">

    <Link
      to="/"
      className="d-inline-flex align-items-center text-decoration-none text-light back-link"
    >
      <span className="me-2 fs-5">←</span>
      <span className="fw-semibold">Back</span>
    </Link>

    {title && (
      <>
        <span className="mx-3 text-secondary">/</span>
        <span className="text-secondary">{title}</span>
      </>
    )}

  </div>
</header>
  );
}