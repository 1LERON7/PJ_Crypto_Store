import { Link, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import { profile } from "../api/auth";
import BackHeader from "../components/HeaderBack";
import { useFavorites } from "./FavoritesContext";
import ProductCard from "./ProductsCard";
import axios from "../api/axios";
import Footer from "./Footer";

console.log("TOKEN:", localStorage.getItem("AccessToken"));

// пропс продуктов
export default function Profile() {
  const { favoriteIds } = useFavorites();
  const [products, setProducts] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    axios.get("/products").then(res=> setProducts(res.data.items));
  }, [])

    const favoriteProducts = products.filter(p =>
    favoriteIds.has(p.id)
  );

    const [email, setEmail] = useState(null);
    const [role, setRole] = useState("user");
    const [createdAt, setCreatedAt] = useState(null);
    const [loading, setLoading] = useState(true);

 

    const handleLogout = () => {
    localStorage.removeItem("AccessToken");
    localStorage.removeItem("RefreshToken");
    navigate("/auth/login", { replace: true });
  };

    // Эффект - хук, выполняется когда меняються зависимости
    useEffect(() => {
        // Api запрос к Бэку
        profile().then(data => {
            // данные кладем в стан
            setEmail(data.email);
            setRole(data.role);
            console.log("createdAt from backend:", data.createdAt);
            setCreatedAt(data.createdAt);
        })
        .catch(err => {
            console.error("Not authorized", err);
            localStorage.removeItem("AccessToken");
            localStorage.removeItem("RefreshToken");
            navigate("/auth/login");
        })
        // пока идет ответ от бд, будет "загрузка"
        .finally(() => setLoading(false));
    }, [navigate]);

    if (loading) {
    return <div className="container mt-5">Loading...</div>;
  }

  return (
    <>
    <BackHeader/>
    <div className="container mt-5" style={{ maxWidth: "600px" }}>
      <div className="card shadow-sm p-4">

        <h4 className="mb-4 text-center">Profile</h4>

        <div className="mb-3">
          <small className="text-muted">Email</small>
          <div className="fs-5">{email}</div>
        </div>

        <div className="mb-3">
          <small className="text-muted">Role</small>
          <div>
            <span className="badge bg-primary">{role}</span>
          </div>
        </div>

        <div className="mb-4">
          <small className="text-muted">Account created</small>
          <div>
            {createdAt
                ? new Date(createdAt).toLocaleString()
                : "—"}
            </div>
        </div>

        <div className="d-flex justify-content-end">


          <button className="btn btn-outline-danger" onClick={handleLogout}>
            Logout
          </button>

        </div>

<div className="container">
      <h3 className="mb-3">Favorite</h3>

      {favoriteProducts.length === 0 && (
        <p>No favorite products</p>
      )}

      <div className="row">
        {favoriteProducts.map(product => (
          <div className="col-md-4 mb-3" key={product.id}>
            
            <Link to={`/products/${product.id}`}className="d-block h-100 text-decoration-none text-dark">
                      <ProductCard product={product} />
            </Link>

          </div>
        ))}
      </div>
    </div>
        
      </div>
      
    </div>
    <Footer/>
    </>
  );
}