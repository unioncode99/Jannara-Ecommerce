import { RefreshCw, ShieldCheck, Truck } from "lucide-react";
import "./ProductFeatures.css";
import { useLanguage } from "../../hooks/useLanguage";

const ProductFeatures = () => {
  const { translations } = useLanguage();

  const { free_shipping, secure_payment, thirty_day_returns } =
    translations.general.pages.product_details;
  return (
    <div className="our-features">
      <div>
        <Truck />
        <span>{free_shipping}</span>
      </div>
      <div>
        <ShieldCheck />
        <span>{secure_payment}</span>
      </div>
      <div>
        <RefreshCw />
        <span>{thirty_day_returns}</span>
      </div>
    </div>
  );
};
export default ProductFeatures;
