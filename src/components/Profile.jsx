import { Link, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import { profile } from "../api/auth";
import BackHeader from "../components/HeaderBack";
import { useFavorites } from "./FavoritesContext";
import ProductCard from "./ProductsCard";
import axios from "../api/axios";
import { connectWallet } from "./MetaMask";
import {connectAddress, updateProfile } from "../api/users";
import "../components/style.css";

import { Toast } from "react-bootstrap";
// console.log("TOKEN:", localStorage.getItem("AccessToken"));

// пропс продуктов
export default function Profile() {

  const { favoriteIds } = useFavorites();
  const [products, setProducts] = useState([]);
  const navigate = useNavigate();

  const [showToast, setShowToast] = useState(false);
  const [showToastMessage, setShowToastMessage] = useState(false);

  useEffect(() => {
    axios.get("/products").then(res=> setProducts(res.data.items));
  }, [])

    const favoriteProducts = products.filter(p =>
    favoriteIds.has(p.id)
  );

    const [user, setUser] = useState(null);
    const [bio, setBio] = useState("");
    const [tag, setTag] = useState("");

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
            setUser(data);
            setBio(data.bio || "");
            setTag(data.tag || "");
            
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

   if (loading || !user)
  return (
    <div className="container mt-5 text-center">
      <div className="spinner-border text-success" role="status"></div>
      <p className="mt-3 text-muted">Loading profile...</p>
    </div>
  );

  const handleConnectWallet = async () => {
    const address = await connectWallet();

    if(!address) return;

    console.log("Wallet:", address);

    const resAddress = await connectAddress(address);
    if(resAddress != null)
      setShowToast(true);
  }

   const handleSaveProfile = async () => {
    try {
      await updateProfile(bio, tag);

      setUser({
        ...user, bio, tag
      });

      setShowToastMessage(true);
    }catch (err) {
      console.error("Profile update failed", err);
    }
   }

  return (
    <>
    <BackHeader/>
    
    <div className="container mt-5" style={{ maxWidth: "600px" }}>
      <div className="card profile-card shadow-sm p-4">

        <h4 className="mb-4 text-center">Profile</h4>

      <h3 className="mb-1">{user.name}</h3>

        <div className="mb-3">
          <small className="text-muted">Bio</small>

          <textarea
            className="form-control profile-input"
            maxLength={300}
            rows={3}
            value={bio}
            onChange={(e) => setBio(e.target.value)}
            placeholder="Tell something about yourself..."
          />

          <div className="text-end text-muted small">
            {(bio?.length || 0)}/300
          </div>

        </div>

        <div className="mb-3">
          <small className="text-muted">Game Tag</small>

          <input
            type="text"
            className="form-control profile-input"
            maxLength={35}
            value={tag}
            onChange={(e) => setTag(e.target.value)}
            placeholder="Your gaming nickname"
          />

          <div className="text-end text-muted small">
            {tag.length}/35
          </div>

          <div className="d-flex justify-content-end mt-3">
            <button
              className="btn btn-outline-success px-4 py-2 fw-semibold"
              onClick={handleSaveProfile}
              disabled={bio === user.bio && tag === user.tag}
            >
              Save
            </button>
          </div>

          <hr className="my-4"/>
        </div>

        <div className="mb-2">
          <small className="text-muted">Email</small>
          <div>{user.email}</div>
        </div>


{/* ТОЛЬКО ДЛЯ АДМИНА ВИДНА РОЛЬ И КНОПКА */}
    {user.role === "admin" && (
  <div className="mb-3">
    <small className="text-muted">Role</small>

    <div className="mb-2">
      <span className="badge bg-danger">{user.role}</span>
    </div>

    <Link to="/admin" className="btn btn-danger btn-sm">
      Admin Panel
    </Link>
  </div>
)}

        <div className="mb-4">
          <small className="text-muted">Account created</small>
          <div>
            {createdAt
                ? new Date(user.createdAt).toLocaleString()
                : "—"}
            </div>
        </div>

        <div className="d-flex justify-content-between align-items-center">

          <button
            className="btn btn-warning"
            onClick={handleConnectWallet}
          >
            Connect Wallet
          </button>

          <button
            className="btn btn-outline-danger"
            onClick={handleLogout}
          >
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
            
            <Link to={`/products/${product.id}`}className="d-block h-100 text-decoration-none">
                      <ProductCard product={product} />
            </Link>

          </div>
        ))}
      </div>
    </div>
        
      </div>
    </div>
    <Toast
      onClose={() => setShowToastMessage(false)}
      show={showToastMessage}
      delay={2000}
      autohide
      style={{
        position: "fixed",
        bottom: 30,
        right: 30,
        zIndex: 9999,
        minWidth: "260px",
        border: "2px solid #22c55e",
        background: "#1e2329",
        color: "#22c55e",
        fontSize: "16px"
      }}
    >
      <Toast.Body className="d-flex align-items-center gap-2">
    
        <span style={{fontSize:18}}>✔</span>
    
        Save
    
      </Toast.Body>
    </Toast>


    <Toast
      onClose={() => setShowToast(false)}
      show={showToast}
      delay={2000}
      autohide
      style={{
        position: "fixed",
        bottom: 30,
        right: 30,
        zIndex: 9999,
        minWidth: "260px",
        border: "2px solid #22c55e",
        background: "#1e2329",
        color: "#22c55e",
        fontSize: "16px"
      }}
    >
      <Toast.Body className="d-flex align-items-center gap-2">
    
        <span style={{fontSize:18}}>✔</span>
    
        The wallet is connected
    
      </Toast.Body>
    </Toast>
    </>
    
  );
}