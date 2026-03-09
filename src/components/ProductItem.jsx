import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { getProductById } from "../api/products";
import BackHeader from "../components/HeaderBack";
import Footer from "./Footer";

import { buyProduct } from "../web3/contract";
import { confirmPayment } from "../api/payments";

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


const handleBuy = async () => {

  console.log(product);

  const txHash = await buyProduct(product.id, product.price);

  // await confirmPayment(product.id, txHash)

  console.log("TX:", txHash);

};

  if (loading) return <div className="container mt-5">Loading...</div>;
  if (!product) return <div className="container mt-5">Product not found</div>;

  return (
    <>
<BackHeader />

<div className="container mt-5">

  <h2 className="mb-4">{product.title}</h2>

  <img
    src={product.imageUrl}
    alt={product.title}
    className="w-100"
    style={{ height: "450px", objectFit: "cover" }}
  />
  <p className="text-muted">{product.description}</p>

  <div className="d-flex justify-content-between align-items-center mt-4">

    <h3 className="text-success fw-bold">
      ETH {product.price}
    </h3>

    <button
      className="btn btn-success btn-lg"
      onClick={handleBuy}
    >
      Buy
    </button>

  </div>

</div>

<Footer />
</>
  );
}