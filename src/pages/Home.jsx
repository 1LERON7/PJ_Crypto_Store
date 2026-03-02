import { useEffect, useState } from "react";
import { getProducts } from "../api/products";
import Header from "../components/Headers";
import ProductCard from "../components/ProductsCard";
import Footer from "../components/Footer";
import { Link } from "react-router-dom";

export default function Home() {
  const [products, setProducts] = useState([]);

  useEffect(() => {
    const load = async () => {
      const data = await getProducts();
      setProducts(data.items);
    };
    load();
  }, []);

  return (
    <>
      <Header />
      <div className="container mt-4">
  <div className="row g-4">
    {products.map(p => (
      <div className="col-md-4" key={p.id}>

 {/* стили из бутстрап для Линк, чтобы была сетка */}
        <Link to={`/products/${p.id}`}className="d-block h-100 text-decoration-none text-dark">
          <ProductCard product={p} />
        </Link>

      </div>
    ))}
  </div>
</div>
          <Footer/>
    </>
  );
}