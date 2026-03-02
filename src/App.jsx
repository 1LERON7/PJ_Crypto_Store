import { Routes, Route } from "react-router-dom";
import Home from "./pages/Home";
import ProductItem from "./components/ProductItem";
import Login from "./pages/Login";
import Register from "./pages/Register";
import Profile from "./components/Profile";
import ProtectedRoute from "./components/ProtectedRoute";

export default function App() {
  return (
    <Routes>
      <Route path="/" element={<Home/>} />
      <Route path="/auth/login" element={<Login />} />
      <Route path="/auth/register" element={<Register />} />
      <Route path="/products/:id" element={<ProductItem/>}/>
      <Route path="/profile" element={<ProtectedRoute><Profile /></ProtectedRoute>} />
    </Routes>
  );
}