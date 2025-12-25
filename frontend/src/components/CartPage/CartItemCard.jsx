import "./CartItemCard.css";
import { useLanguage } from "../../hooks/useLanguage";
import { Link } from "react-router-dom";
import QuantitySelector from "../QuantitySelector";
import { useEffect, useState } from "react";
import { Trash2 } from "lucide-react";
import { formatMoney } from "../../utils/utils";
import { useCart } from "../../contexts/CartContext";

const CartItemCard = ({ cartItem }) => {
  const [quantity, setQuantity] = useState(cartItem.quantity || 0);
  const { language } = useLanguage();
  const { removeItem, addOrUpdateItem } = useCart();
  const customerId = 1; // for test
  const {
    id,
    sellerProductId,
    defaultProductImage,
    productNameEn,
    productNameAr,
    categoryNameEn,
    categoryNameAr,
    brandNameEn,
    brandNameAr,
    sku,
    selectedOptions,
    availableStock,
    subTotal,
  } = cartItem;

  async function updateCartQuantity() {
    await addOrUpdateItem({
      customerId: customerId,
      sellerProductId: sellerProductId,
      quantity: quantity,
    });
  }

  useEffect(() => {
    updateCartQuantity();
  }, [quantity]);

  return (
    <div className="cart-item-card">
      <div>
        <div className="cart-item-image">
          <img src={defaultProductImage} alt="Cart Item Image" />
        </div>
        <div className="cart-item-info">
          <Link className="product-name" to={`/product/${cartItem?.publicId}`}>
            {language == "en" ? productNameEn : productNameAr}
          </Link>
          <p className="text-small">
            {language == "en" ? brandNameEn : brandNameAr}
          </p>
          <p className="text-small">
            {language == "en" ? categoryNameEn : categoryNameAr}
          </p>
          <p className="text-small sku">SKU: {sku}</p>

          <div className="cart-item-variaions">
            {selectedOptions.map((option) => (
              <small key={option.valueEn}>
                {language == "en" ? option.valueEn : option.valueAr}
              </small>
            ))}
          </div>
        </div>
      </div>
      <div className="cart-item-actions">
        <span className="cart-item-subtotal">{formatMoney(subTotal)}</span>
        <div></div>
        <QuantitySelector
          quantity={quantity}
          setQuantity={setQuantity}
          stockQuantity={availableStock}
        />
        <button className="remove-form-cart-btn" onClick={() => removeItem(id)}>
          <Trash2 />
        </button>
      </div>
    </div>
  );
};
export default CartItemCard;
