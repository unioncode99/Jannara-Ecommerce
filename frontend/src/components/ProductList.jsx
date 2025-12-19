import { useEffect, useState } from "react";
import ProductCard from "./ProductCard";
import "./ProductList.css";
import { read } from "../api/apiWrapper";

const ProductList = () => {
  const [products, setProducts] = useState([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const getProducts = async () => {
      try {
        const data = await read("products?pageNumber=1&pageSize=10");
        setProducts(data?.data?.items);
        console.log("data?.items -> ", data?.data?.items);
      } catch (error) {
        console.error("Failed to fetch categories:", error);
      }
    };
    getProducts();
  }, []);

  const handleToggleFavorite = async (productId, isFavorite) => {
    console.log("Favorite changed:", productId, isFavorite);
    // 1 Optimistic UI update
    setProducts((prev) =>
      prev.map((p) => (p.id === productId ? { ...p, isFavorite } : p))
    );
    try {
      // 2️ Call API
      // if (isFavorite) {
      //   await addToFavorites(productId);
      // } else {
      //   await removeFromFavorites(productId);
      // }
    } catch (error) {
      console.error("Favorite error:", error);

      // 3️ Rollback if API fails
      setProducts((prev) =>
        prev.map((p) =>
          p.id === productId ? { ...p, isFavorite: !isFavorite } : p
        )
      );
    }
  };

  return (
    <div className="product-grid">
      {products?.map((product) => (
        <ProductCard
          key={product.id}
          product={product}
          onToggleFavorite={handleToggleFavorite}
        />
      ))}
    </div>
  );
};
export default ProductList;
