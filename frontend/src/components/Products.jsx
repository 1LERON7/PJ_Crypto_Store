import { useEffect, useState } from "react";
import { getProducts } from "../api/products";

export default function Products() {
  const [products, setProducts] = useState([]);
  const [page, setPage] = useState(1);

  

const loadProducts = async () => {
  const data = await getProducts({ page, pageSize: 12 });
  setProducts(data.items);
};

useEffect(() => {
  loadProducts();
}, [page]);


  return (
    <div className="container">
      <h3>Products</h3>

      <div className="row g-4">
        {products.map(p => (
          <div className="col-md-3" key={p.id}>
            <div className="card h-100">
              <img src={p.image_URL} className="card-img-top" />
              <div className="card-body">
                <h6>{p.title}</h6>
                <p>${p.price}</p>
              </div>
            </div>
          </div>
        ))}
      </div>

      <button
        className="btn btn-outline-primary mt-3"
        onClick={() => setPage(p => p + 1)}
      >
        Next page
      </button>
    </div>
  );
}