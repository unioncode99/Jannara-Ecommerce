import "./ProductQuantity.css";
import { useLanguage } from "../../hooks/useLanguage";
import QuantitySelector from "../QuantitySelector";

function ProductQuantity({ quantity, setQuantity, stockQuantity }) {
  const { translations } = useLanguage();

  const {
    quantity: quantity_text,
    available,
    out_of_stock,
  } = translations.general.pages.product_details;

  return (
    <div className="product-quantity">
      <span>{quantity_text}:</span>
      <QuantitySelector
        quantity={quantity}
        setQuantity={setQuantity}
        stockQuantity={stockQuantity}
      />
      <span style={{ marginLeft: 12 }}>
        {stockQuantity > 0 ? `${stockQuantity} ${available}` : out_of_stock}
      </span>
    </div>
  );
}

export default ProductQuantity;
