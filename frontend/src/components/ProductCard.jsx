import { useEffect, useState } from "react";
import "./ProductCard.css";
import { Heart, ShoppingCart, Star } from "lucide-react";

const ProductCard = ({ product, onToggleFavorite }) => {
  const [isFavorite, setIsFavorite] = useState(product.isFavorite || false);

  useEffect(() => {
    setIsFavorite(product.isFavorite);
  }, [product.isFavorite]);

  const handleFavorite = (e) => {
    e.stopPropagation();
    const newValue = !isFavorite;
    setIsFavorite(newValue);
    // if (onToggleFavorite) {
    //   onToggleFavorite(product.id, newValue);
    // }
    // Call the function only if it exists
    onToggleFavorite?.(product.id, newValue);
  };

  return (
    <div className="product-card">
      <div className="product-image-wrapper">
        <img src={product.image} alt={product.name} />

        <button
          className={`favorite-btn ${isFavorite ? "active" : ""}`}
          onClick={handleFavorite}
        >
          <Heart fill={isFavorite ? "red" : "none"} />
        </button>
      </div>

      <div className="product-info">
        <h3 className="product-name">{product.name}</h3>
        <div className="product-rating">
          <Star />
          <span>4.8 (124)</span>
        </div>
        <div>
          <span className="product-price">${product.price}</span>
          <button>
            <ShoppingCart />
          </button>
        </div>
      </div>
    </div>
  );
};
export default ProductCard;
