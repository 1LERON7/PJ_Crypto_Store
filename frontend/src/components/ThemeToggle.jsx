import { useEffect, useState } from "react";
import "../components/style.css";

export default function ThemeToggle() {
  const [theme, setTheme] = useState(localStorage.getItem("theme") || "dark");

  useEffect(() => {
    document.body.setAttribute("data-bs-theme", theme);
    localStorage.setItem("theme", theme);
  }, [theme]);

  return (
    <div className="btn-group theme-toggle">
  <button
    className={`btn ${theme === "dark" ? "btn-dark" : "btn-outline-secondary"}`}
    onClick={() => setTheme("dark")}
  >
    🌙 Dark
  </button>

  <button
    className={`btn ${theme === "light" ? "btn-light" : "btn-outline-secondary"}`}
    onClick={() => setTheme("light")}
  >
    ☀ Light
  </button>
</div>
  );
}