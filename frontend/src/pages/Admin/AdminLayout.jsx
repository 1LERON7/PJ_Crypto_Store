import { Link, Outlet } from "react-router-dom";
import ThemeToggle from "../../components/ThemeToggle";
import BackHeader from "../../components/HeaderBack";
import "./admin.css";
import { NavLink } from "react-router-dom";

export default function AdminLayout() {

    
  return (
    <>
    <BackHeader/>

    <div className="container-fluid">
      
      <div className="row">

        <div className="col-2 bg-dark text-white vh-100 p-3 admin-sidebar">
          <h5 className="mb-4">Admin Panel</h5>

          <div className="nav flex-column gap-2">
            <NavLink
              to="products"
              className={({ isActive }) =>
                `btn text-start ${isActive ? "btn-success" : "btn-outline-light"}`
              }
            >
              📦 Products
            </NavLink>

            <NavLink
              to="users"
              className={({ isActive }) =>
                `btn text-start ${isActive ? "btn-success" : "btn-outline-light"}`
              }
            >
              👤 Users
            </NavLink>

            <NavLink
              to="payments"
              className={({ isActive }) =>
                `btn text-start ${isActive ? "btn-success" : "btn-outline-light"}`
              }
            >
              💲 Payments
            </NavLink>

          </div>
        </div>

        <div className="col-10 p-4">
          <div className="d-flex justify-content-end p-3">
            <ThemeToggle />
          </div>

          <Outlet />

        </div>

      </div>
    </div>
    </>
  );
}