import { useLanguage } from "../../hooks/useLanguage";
import { formatMoney } from "../../utils/utils";
import "./ProductPrice.css";

export default function ProductPrice({ minPrice }) {
  const { translations } = useLanguage();
  return (
    <div className="product-price-container">
      {minPrice ? (
        <>
          <span>{translations.general.pages.home.price_start_from} </span>
          <span className="product-price" dir="ltr">
            {formatMoney(minPrice)}
          </span>
        </>
      ) : (
        <span className="product-price">
          {translations.general.pages.home.soming_soon}
        </span>
      )}
    </div>
  );
}
