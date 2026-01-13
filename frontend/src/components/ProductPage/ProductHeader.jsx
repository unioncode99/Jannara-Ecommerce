import { Heart, Share2 } from "lucide-react";
import "./ProductHeader.css";
import { useLanguage } from "../../hooks/useLanguage";

export default function ProductHeader({ product, onToggleFavorite }) {
  const { language } = useLanguage();
  return (
    <div className="product-header">
      <div>
        <p>
          {language === "en" ? product?.brand?.nameEn : product?.brand?.nameAr}
        </p>
        <h1>{language === "en" ? product?.nameEn : product?.nameAr}</h1>
      </div>
      <div>
        <button
          className={`fav-btn ${product?.isFavorite ? "active" : ""}`}
          onClick={onToggleFavorite}
        >
          <Heart />
        </button>
        <button>
          <Share2 />
        </button>
      </div>
    </div>
  );
}
