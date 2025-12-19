import { useEffect, useState } from "react";
import "./ProductCard.css";
import { Heart, ShoppingCart, Star } from "lucide-react";
import { useLanguage } from "../hooks/useLanguage";
import { useNavigate } from "react-router-dom";

const ProductCard = ({ product, onToggleFavorite }) => {
  const [isFavorite, setIsFavorite] = useState(product.isFavorite || false);
  const { translations } = useLanguage();
  const navigate = useNavigate();
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

  const goToProductPage = () => {
    navigate("/product");
  };

  return (
    <div onClick={() => goToProductPage()} className="product-card">
      <div className="product-image-wrapper">
        <img src={product.defaultImageUrl} alt={product.name} />

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
          {!product.averageRating || !product.ratingCount ? (
            <span>{translations.general.pages.home.no_ratings_yet}</span>
          ) : (
            <>
              <Star />
              {/* <span>4.8 (124)</span> */}
              <span>
                {product.averageRating} ({product.ratingCount})
              </span>
            </>
          )}
        </div>
        <div>
          <span className="product-price-container">
            {product.minPrice ? (
              <>
                <span className="text-small">
                  {translations.general.pages.home.price_start_from}{" "}
                </span>
                <span className="product-price" dir="ltr">
                  ${product.minPrice}
                </span>
              </>
            ) : (
              <span className="product-price">
                {translations.general.pages.home.soming_soon}
              </span>
            )}
          </span>
          <button>
            <ShoppingCart />
          </button>
        </div>
      </div>
    </div>
  );
};
export default ProductCard;
