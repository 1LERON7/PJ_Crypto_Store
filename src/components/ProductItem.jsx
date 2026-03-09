import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { getProductById } from "../api/products";
import { getPaymentStatus } from "../api/payments";

import BackHeader from "../components/HeaderBack";
import Footer from "./Footer";

import { buyProduct } from "../web3/contract";


export default function ProductDetails() {
  const { id } = useParams();
  const [product, setProduct] = useState(null);
  const [loading, setLoading] = useState(true);
  const [payment, setPayment] = useState("created");

  useEffect(() => {
    getProductById(id)
      .then(data => {
        setProduct(data);
        setLoading(false);
      })

    getPaymentStatus(id).then(pay => {
      setPayment(pay);
    })

      .catch(err => {
        console.error("Ошибка загрузки товара", err);
        setLoading(false);
      });
  }, [id]);



const handleBuy = async () => {

  console.log(product);

  const txHash = await buyProduct(product.id, product.price);

  console.log("TX:", txHash);

};
// console.log(payment);

  if (loading)
  return (
    <div className="container mt-5">

      <div className="card p-3">

        <div className="placeholder-glow">
          <div className="placeholder col-12" style={{height:"300px"}}></div>
        </div>

        <h4 className="placeholder-glow mt-3">
          <span className="placeholder col-6"></span>
        </h4>

        <p className="placeholder-glow">
          <span className="placeholder col-4"></span>
        </p>

        <span className="btn btn-success disabled placeholder col-3"></span>

      </div>

    </div>
  );

  if (!product)
  return (
    <div className="container mt-5 text-center" style={{ minHeight: "50vh" }}>
      <h2 className="text-danger mb-3">Product not found</h2>
      <p className="text-muted">This item may have been deleted.</p>

      <a href="/" className="btn btn-outline-light mt-3">
        Back to store
      </a>
    </div>
  );

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

  {payment?.status === "confirmed" ? (
  <button className="btn btn-secondary btn-lg">
    Already purchased
  </button>
) : (
  <button className="btn btn-success btn-lg" onClick={handleBuy}>
    Buy
  </button>
)}
    

  </div>

</div>

<Footer />
</>
  );
}