import { Star } from "lucide-react";
import "./ProductRating.css";
import { useLanguage } from "../../hooks/useLanguage";

export default function ProductRating({ averageRating, ratingCount }) {
  const { translations } = useLanguage();
  const { reviews } = translations.general.pages.product_details;
  return (
    <div className="product-rating">
      <div>
        {Array.from({ length: 5 }).map((_, index) => (
          <Star
            className={index + 1 <= averageRating ? "active" : ""}
            key={index}
          />
        ))}
      </div>
      <span>{Number(averageRating).toFixed(1)}</span>
      <span>
        ({ratingCount} {reviews})
      </span>
    </div>
  );
}
