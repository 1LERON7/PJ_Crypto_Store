import { useEffect, useState } from "react";

export default function ThemeToggle() {
  const [theme, setTheme] = useState("dark");

  useEffect(() => {
    document.body.setAttribute("data-bs-theme", theme);
  }, [theme]);

  return (
    <div className="btn-group">
      <button
        className={`btn ${theme === "dark" ? "btn-dark" : "btn-outline-dark"}`}
        onClick={() => setTheme("dark")}
      >
        🌙 Dark
      </button>

      <button
        className={`btn ${theme === "light" ? "btn-light" : "btn-outline-light"}`}
        onClick={() => setTheme("light")}
      >
        ☀ Light
      </button>
    </div>
  );
}