import { useEffect, useState } from "react";
import { getProducts } from "../api/products";
import Header from "../components/Headers";
import ProductCard from "../components/ProductsCard";
import Footer from "../components/Footer";
import { Link } from "react-router-dom";
import api from "../api/api";

export default function Home() {
  const [products, setProducts] = useState([]);

  const [totalCount, setTotalCount] = useState(0);
  const [page, setPage] = useState(1);

  const pageSize = 12;

  useEffect(() => {
    const loadProducts = async () => {
    try {
      const { data } = await api.get(`/products?page=${page}&pageSize=${pageSize}`);

      setProducts(data.items ?? []);
      setTotalCount(data.totalCount ?? 0);
    } catch (err) {
      console.log(err);
    }
  };

  loadProducts();
  }, [page, pageSize]);

   const totalPages = Math.ceil(totalCount / pageSize);

   const pages = [];

for (let i = page - 2; i <= page + 2; i++) {
  if (i > 0 && i <= totalPages) {
    pages.push(i);
  }
}

  return (
    <>
    
      <Header />
      <div className="container mt-4">
  <div className="row g-4">
    {products.map(p => (
      <div className="col-md-4" key={p.id}>

 {/* стили из бутстрап для Линк, чтобы была сетка */}
        <Link to={`/products/${p.id}`} className="d-block h-100 text-decoration-none text-dark">
          <ProductCard product={p} />
        </Link>
      </div>
    ))}
  </div>

<div className="text-center text-muted mb-3">
  Showing page {page} of {totalPages}
</div>

<nav className="mt-5 d-flex justify-content-center">
  <ul className="pagination">

    <li className={`page-item ${page === 1 ? "disabled" : ""}`}>
      <button
        className="page-link"
        onClick={() => setPage(page - 1)}
      >
        ←
      </button>
    </li>

    {pages.map(p => (
      <li key={p} className={`page-item ${p === page ? "active" : ""}`}>
        <button
          className="page-link"
          onClick={() => setPage(p)}
        >
          {p}
        </button>
      </li>
    ))}

    <li className={`page-item ${page === totalPages ? "disabled" : ""}`}>
      <button
        className="page-link"
        onClick={() => setPage(page + 1)}
      >
        →
      </button>
    </li>

  </ul>
</nav>

</div>
          <Footer/>
    </>
  );
}