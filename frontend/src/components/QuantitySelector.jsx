import { Minus, Plus } from "lucide-react";
import "./QuantitySelector.css";
const QuantitySelector = ({ quantity, setQuantity, stockQuantity }) => {
  const handleIncrease = () => {
    setQuantity((q) => Math.min(q + 1, stockQuantity));
  };

  const handleDecrease = () => {
    setQuantity((q) => Math.max(1, q - 1));
  };
  return (
    <div className="quantity-selector">
      <button
        onClick={handleIncrease}
        disabled={stockQuantity === 0}
        style={{ padding: "4px 8px" }}
      >
        <Plus />
      </button>
      <span>{quantity}</span>
      <button
        onClick={handleDecrease}
        disabled={stockQuantity === 0}
        style={{ padding: "4px 8px" }}
      >
        <Minus />
      </button>
    </div>
  );
};
export default QuantitySelector;
