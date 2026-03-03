import { Link, Outlet } from "react-router-dom";

export default function AdminLayout() {

    
  return (
    <div className="container-fluid">
      <div className="row">

        <div className="col-2 bg-dark text-white vh-100 p-3">
          <h5>Admin Panel</h5>
          <hr />
          <Link to="/admin" className="d-block text-white mb-2">Dashboard</Link>
          <Link to="/admin/products" className="d-block text-white mb-2">Products</Link>
          <Link to="/admin/users" className="d-block text-white">Users</Link>
        </div>

        <div className="col-10 p-4">
          <Outlet />
        </div>

      </div>
    </div>
  );
}