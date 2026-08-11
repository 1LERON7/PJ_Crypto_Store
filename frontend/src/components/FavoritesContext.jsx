import { createContext, useContext, useEffect, useState } from "react";
import { getFavorites, addFavorite, removeFavorite } from "../api/favorite";

// контейнер данных для всего сайта
const FavoritesContext = createContext(null); 

export const FavoritesProvider = ({ children }) => {
  // стан избранных продуктов
  const [favoriteIds, setFavoriteIds] = useState(new Set());

  
    

  useEffect(() => {
    const loadFavorites = async () => {
      try {
        const ids = await getFavorites();
        setFavoriteIds(new Set(ids));
      } catch (e) {
        console.error("Failed to load favorites", e);
      }
    };

    if (!localStorage.getItem("AccessToken")) {
    setFavoriteIds(new Set()); // чистка при отсутствии токена
    return;
  }

  // const token = localStorage.getItem("AccessToken");

  // if (!token) return;

  loadFavorites();
}, []);


  // описываю в бд и проверка
  const toggleFavorite = async (productId) => {
    const isFav = favoriteIds.has(productId);

    setFavoriteIds(prev => {
      const next = new Set(prev);
      isFav ? next.delete(productId) : next.add(productId);
      return next;
    });

    try {
      isFav
        ? await removeFavorite(productId)
        : await addFavorite(productId);
    } catch {
      setFavoriteIds(prev => {
        const next = new Set(prev);
        isFav ? next.add(productId) : next.delete(productId);
        return next;
      });
    }
  };

  return (
    <FavoritesContext.Provider value={{ favoriteIds, toggleFavorite }}>
      {children}
    </FavoritesContext.Provider>
  );
};

// хук для других компонентов
export const useFavorites = () => {
  const ctx = useContext(FavoritesContext);
  if (!ctx) throw new Error("useFavorites must be used inside FavoritesProvider");
  return ctx;
};