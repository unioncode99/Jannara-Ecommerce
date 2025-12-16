import { useState } from "react";
import ProductCard from "./ProductCard";
import "./ProductList.css";

const products_seed = [
  {
    id: 1,
    name: "iPhone 15",
    price: 1200,
    image: "https://picsum.photos/400/400?random=1",
    isFavorite: false,
  },
  {
    id: 2,
    name: "Samsung Galaxy S24",
    price: 1100,
    image: "https://picsum.photos/400/400?random=2",
    isFavorite: false,
  },
  {
    id: 3,
    name: "MacBook Pro M3",
    price: 2500,
    image: "https://picsum.photos/400/400?random=3",
    isFavorite: false,
  },
  {
    id: 4,
    name: "Sony WH-1000XM5",
    price: 400,
    image: "https://picsum.photos/400/400?random=4",
    isFavorite: false,
  },
  {
    id: 5,
    name: "Apple Watch Series 9",
    price: 500,
    image: "https://picsum.photos/400/400?random=5",
    isFavorite: false,
  },
  {
    id: 6,
    name: "iPad Pro 12.9",
    price: 1300,
    image: "https://picsum.photos/400/400?random=6",
    isFavorite: false,
  },
];

const ProductList = () => {
  const [products, setProducts] = useState(products_seed);

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
      {products.map((product) => (
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
