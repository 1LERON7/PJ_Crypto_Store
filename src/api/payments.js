import axios from "axios";

export const createPayment = async (orderId) => {
  const token = localStorage.getItem("AccessToken");
  const { data } = await axios.post(
    `/api/payments/create/${orderId}`,
    {},
    {headers: { Authorization: `Bearer ${token}` }}
  );
  return data;
};

export const confirmPayment = async (productId, txHash) => {
  const token = localStorage.getItem("AccessToken");

  const { data } = await axios.post(
    `/api/payments/confirm`,
    { productId, txHash },
    {headers: { Authorization: `Bearer ${token}` }}
  );
  return data;
};

export const getPayment = async () => {
  const response = await axios.get("/payments");
    return response.data;
}