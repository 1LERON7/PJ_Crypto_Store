import React from "react";
import ReactDOM from "react-dom/client";
import { BrowserRouter } from "react-router-dom";
import App from "./App";
import { FavoritesProvider } from "./components/FavoritesContext.jsx";

import "bootstrap/dist/css/bootstrap.min.css";
import { AuthProvider } from "./components/AuthContext";

ReactDOM.createRoot(document.getElementById("root")).render(
  <React.StrictMode>
    <BrowserRouter>
    <AuthProvider>
      <FavoritesProvider>
      <App />
      </FavoritesProvider>
      </AuthProvider>
    </BrowserRouter>
  </React.StrictMode>
);