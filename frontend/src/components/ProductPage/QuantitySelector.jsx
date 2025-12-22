import { Minus, Plus } from "lucide-react";
import "./QuantitySelector.css";
import { useLanguage } from "../../hooks/useLanguage";

function QuantitySelector({ quantity, setQuantity, selectedSeller }) {
  const { translations } = useLanguage();
  if (!selectedSeller) {
    return null; // hide if no seller selected
  }

  const handleIncrease = () => {
    setQuantity((q) => Math.min(q + 1, selectedSeller.stockQuantity));
  };

  const handleDecrease = () => {
    setQuantity((q) => Math.max(1, q - 1));
  };

  const {
    quantity: quantity_text,
    available,
    out_of_stock,
  } = translations.general.pages.product_details;

  return (
    <div className="product-quantity">
      <span>{quantity_text}:</span>
      <div className="quantity-selector">
        <button
          onClick={handleIncrease}
          disabled={selectedSeller.stockQuantity === 0}
          style={{ padding: "4px 8px" }}
        >
          <Plus />
        </button>
        <span>{quantity}</span>
        <button
          onClick={handleDecrease}
          disabled={selectedSeller.stockQuantity === 0}
          style={{ padding: "4px 8px" }}
        >
          <Minus />
        </button>
      </div>
      <span style={{ marginLeft: 12 }}>
        {selectedSeller.stockQuantity > 0
          ? `${selectedSeller.stockQuantity} ${available}`
          : out_of_stock}
      </span>
    </div>
  );
}

export default QuantitySelector;
