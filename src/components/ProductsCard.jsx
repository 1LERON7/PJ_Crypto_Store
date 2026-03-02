import { useFavorites } from "../components/FavoritesContext";
import "./style.css";

export default function ProductCard({ product, onClick  }) {
  // знаем состояния избранного
   const { favoriteIds, toggleFavorite } = useFavorites();

  const isFav = favoriteIds.has(product.id);
// console.log(product.imageUrl);

  return (
    <div className="card h-100 shadow-sm" onClick={onClick}>
      <button
        className="fav-btn position-absolute top-0 end-0 m-2 bg-transparent border-0"
        onClick={(e) => {
          e.preventDefault();
          // сам клик по сердцу
          toggleFavorite(product.id);
        }}
      >
        <i className={`bi ${isFav ? "bi-heart-fill text-danger" : "bi-heart"}`} />
      </button>
      
<div>

   
  <img
  src={product.imageUrl}
  alt={product.title}
  className="card-img-top"
  style={{ height: "180px", objectFit: "cover", width: "100%" }}
/>
</div>

  <div className="card-body d-flex justify-content-between align-items-center">
    <span className="fw-semibold">{product.title}</span>
    <span className="fw-bold text-success">${product.price}</span>
  </div>
</div>
  );
}