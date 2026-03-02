export default function ProductCard({ product }) {
  return (
    <div className="card h-100 shadow-sm">
      <img
        src={product.image_url}
        alt={product.title}
        className="card-img-top"
        style={{ height: 180, objectFit: "cover" }}
      />

      <div className="card-body d-flex justify-content-between align-items-center">
        <span className="fw-semibold">
          {product.title}
        </span>

        <span className="fw-bold text-success">
          ${product.price}
        </span>
      </div>
    </div>
  );
}