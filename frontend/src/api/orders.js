import axios from "axios";

export const createOrder = async ({ productId }) => {
  const token = localStorage.getItem("AccessToken");

  const { data } = await axios.post(
    "/orders/create",
    { productId },
    { headers: { Authorization: `Bearer ${token}` } }
  );

  return data; 
};