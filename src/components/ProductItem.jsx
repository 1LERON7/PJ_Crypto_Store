import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { getProductById } from "../api/products";
import BackHeader from "../components/HeaderBack";
import Footer from "./Footer";
import { pay } from "../api/crypto";
import { createOrder } from "../api/orders";
import { createPayment } from "../api/payments";


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


 const handlePay = async () => {
  try {
    // запрос на создание заказ
    const orderResponse = await createOrder({ productId: id });
    const orderId = orderResponse.orderId;

    // запрос на создания payments
    const {paymentId, amount} = await createPayment(orderId);


    // запуск маски
    await pay(paymentId, amount, orderId);

  } catch (err) {
    console.error(err);
  }
};

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
      <h4 className="text-success fw-bold">ETH {product.price}</h4>

      <button type="button" className="btn btn-success" onClick={handlePay}>Buy</button>
    </div>
    </div>
    
    <Footer/>
    </>
  );
}