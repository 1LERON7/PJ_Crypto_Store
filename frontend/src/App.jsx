import { Routes, Route } from "react-router-dom";
import Home from "./pages/Home";
import ProductItem from "./components/ProductItem";
import Login from "./pages/Login";
import Register from "./pages/Register";
import Profile from "./components/Profile";
import ProtectedRoute from "./components/ProtectedRoute";
import AdminLayout from "./pages/Admin/AdminLayout";
import AdminProducts from "./pages/Admin/AdminProducts";
import AdminRoute from "./components/AdminRoute";
import AdminUsers from "./pages/Admin/AdminUsers";
import AdminPayments from "./pages/Admin/AdminPayments";

export default function App() {
  return (
    <Routes>
      <Route path="/" element={<Home/>} />
      <Route path="/auth/login" element={<Login />} />
      <Route path="/auth/register" element={<Register />} />
      <Route path="/products/:id" element={<ProductItem/>}/>
      <Route path="/profile" element={<ProtectedRoute><Profile /></ProtectedRoute>} />

      <Route path="/admin" element={<AdminRoute><AdminLayout /></AdminRoute>}>
      <Route index element={<AdminProducts/>}/>
      <Route path="products" element={<AdminProducts />} />
      <Route path="users" element={<AdminUsers />} />
      <Route path="payments" element={<AdminPayments />} />
    </Route>
    
    </Routes>
  );
}