import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { getProductById } from "../api/products";
import BackHeader from "../components/HeaderBack";
import Footer from "./Footer";

export default function ProductDetails() {
  const { id } = useParams();
  const [product, setProduct] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    getProductById(id)
      .then(data => {
        setProduct(data);
        setLoading(false);
      })
      .catch(err => {
        console.error("Ошибка загрузки товара", err);
        setLoading(false);
      });
  }, [id]);

  if (loading) return <div className="container mt-5">Loading...</div>;
  if (!product) return <div className="container mt-5">Product not found</div>;

  return (
    <>
    <BackHeader/>
    <div className="container mt-5" style={{ maxWidth: "1000px" }}>
      <h2>{product.title}</h2>

    
      <img
        src={product.imageURL}
        alt={product.title}
        className="img-fluid rounded mb-4"
        
      />

    
      <p>{product.description}</p>
      <div className="d-flex justify-content-between align-items-center">
      <h4 className="text-success fw-bold">${product.price}</h4>

      <button type="button" class="btn btn-success">Buy</button>
    </div>
    </div>
    
    <Footer/>
    </>
  );
}