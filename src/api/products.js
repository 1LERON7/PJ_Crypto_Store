import axios from "./axios";

export const getProducts = async () => {
  const response = await axios.get("/products");
  return response.data;
};

export const getProductById = async (id) => {
  const response = await axios.get(`/products/${id}`);
  return response.data;
};

export const createProduct = async (p) => {
  const response =await axios.post("/products", p);
  return response.data;
}