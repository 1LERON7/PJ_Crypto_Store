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

export const confirmPayment = async (paymentId, txHash) => {
  const token = localStorage.getItem("AccessToken");

  const { data } = await axios.post(
    `/api/payments/confirm`,
    { paymentId, txHash },
    {headers: { Authorization: `Bearer ${token}` }}
  );
  return data;
};