import axios from "./axios";


export const getFavorites = async () => {
  const { data } = await axios.get("/favorites");
  return data;
};

export const addFavorite = async (productId) => {
  return axios.post("/favorites/add", { productId });
};

export const removeFavorite = async (productId) => {
  return axios.delete(`/favorites/remove/${productId}`);
};