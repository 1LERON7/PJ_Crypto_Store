import { Link } from "react-router-dom";

export default function BackHeader({ title = "" }) {
  return (
    <header className="border-bottom bg-dark">
      <div className="container py-3 d-flex align-items-center">
        <Link
          to="/"
          className="btn btn-link p-0 text-decoration-none d-inline-flex align-items-center"
          aria-label="Back to home"
        >
          <span className="me-2" style={{ fontSize: 22, lineHeight: 1 }}>←</span>
          <span className="fw-semibold">Back</span>
        </Link>

        {title ? <div className="ms-3 text-muted">{title}</div> : null}
      </div>
    </header>
  );
}