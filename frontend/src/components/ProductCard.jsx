import { useState } from "react";
import "./ProductCard.css";
import { Heart, ShoppingCart, Star, Trash2 } from "lucide-react";
import { useLanguage } from "../hooks/useLanguage";
import { useNavigate } from "react-router-dom";
import { formatMoney } from "../utils/utils";

const ProductCard = ({
  product,
  onToggleFavorite,
  actionType = "toggleFavorite",
}) => {
  const [isFavorite, setIsFavorite] = useState(product.isFavorite || false);
  const { translations, language } = useLanguage();
  const navigate = useNavigate();
  const handleAction = (e) => {
    e.stopPropagation();
    if (actionType === "toggleFavorite") {
      const newValue = !isFavorite;
      setIsFavorite(newValue);
      onToggleFavorite?.(product.id, newValue);
    } else if (actionType === "delete") {
      onToggleFavorite?.(product.id);
    }
  };

  const goToProductPage = () => {
    navigate(`/product/${product?.publicId}`);
  };

  return (
    <div onClick={() => goToProductPage()} className="product-card">
      <div className="product-image-wrapper">
        <img src={product.defaultImageUrl} alt={product.name} />

        <button
          className={`favorite-btn ${isFavorite ? "active" : ""}`}
          onClick={handleAction}
        >
          {actionType == "toggleFavorite" ? (
            <Heart fill={isFavorite ? "red" : "none"} />
          ) : (
            <Trash2 color="red" />
          )}
        </button>
      </div>

      <div className="product-info">
        <h3 className="product-name">
          {language == "en" ? product.nameEn : product.nameAr}
        </h3>
        <div className="product-rating">
          {!product.averageRating || !product.ratingCount ? (
            <span>{translations.general.pages.home.no_ratings_yet}</span>
          ) : (
            <>
              <Star />
              {/* <span>4.8 (124)</span> */}
              <span>
                {Number(product.averageRating).toFixed(1)} (
                {product.ratingCount})
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
                  {formatMoney(product.minPrice)}
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
