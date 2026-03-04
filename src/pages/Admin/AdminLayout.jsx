import { Link, Outlet } from "react-router-dom";
import ThemeToggle from "../../components/ThemeToggle";
export default function AdminLayout() {

    
  return (
    
    <div className="container-fluid">
      
      <div className="row">

        <div className="col-2 bg-dark text-white vh-100 p-3">
          <h5>Admin Panel</h5>
          <hr />
          <Link to="products" className="d-block text-white mb-2">Products</Link>
          <Link to="users" className="d-block text-white">Users</Link>
        </div>

        <div className="col-10 p-4">
          <div className="d-flex justify-content-end p-3">
            <ThemeToggle />
          </div>

          <Outlet />

        </div>

      </div>
    </div>
  );
}