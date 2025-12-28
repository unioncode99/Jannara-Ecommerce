import React from "react";
import { useCart } from "../../contexts/CartContext";
import { formatMoney } from "../../utils/utils";
import "./ReviewOrder.css";
import Button from "../ui/Button";
import { useLanguage } from "../../hooks/useLanguage";
import CheckoutNavigationButtons from "../CheckoutPage/CheckoutNavigationButtons";
import { SquarePen } from "lucide-react";

const ReviewOrder = ({ checkoutData, onBack, onNext, handleEditStep }) => {
  const { cart } = useCart();
  console.log("checoutdat -> ", checkoutData);
  console.log("cart -> ", cart);
  const { translations, language } = useLanguage();
  const {
    review_order_title,
    shipping_address,
    shipping_method,
    order_summary,
    subtotal,
    shipping,
    grand_total,
    secure_message,
    back,
    place_order,
    tax,
  } = translations.general.pages.checkout;

  const { shippingAddress, shippingMethod } = checkoutData;

  const handleSubmit = () => {
    console.log("checkoutData", checkoutData);
    onNext({ test: 1 });
    console.log("Order Placed! 🎉");
  };

  console.log("checkoutData", checkoutData);

  return (
    <div className="review-order-container">
      <h2>{review_order_title}</h2>

      {/* Address */}
      <div className="review-card">
        <div className="review-header">
          <h4>📍 {shipping_address}</h4>
          <button onClick={() => handleEditStep("address")}>
            <SquarePen />{" "}
          </button>
        </div>
        <p>
          {language == "en"
            ? shippingAddress.state.nameEn
            : shippingAddress.state.nameAr}
          , {shippingAddress.city}, {shippingAddress.locality},{" "}
          {shippingAddress.street}, {shippingAddress.buildingNumber}
        </p>
      </div>

      {/* Shipping */}
      <div className="review-card">
        <div className="review-header">
          <h4>🚚 {shipping_method}</h4>
          <button onClick={() => handleEditStep("shipping")}>
            <SquarePen />{" "}
          </button>
        </div>
        <p>
          {language == "en" ? shippingMethod.nameEn : shippingMethod.nameAr}
        </p>
      </div>

      {/* Summary */}
      <div className="review-summary">
        <h4>{order_summary}</h4>

        <div className="summary-row">
          <span>{subtotal}</span>
          <span>{formatMoney(cart?.subTotal)}</span>
        </div>

        <div className="summary-row">
          <span>
            {tax} ({cart.taxRate * 100}%)
          </span>
          <span>{formatMoney(cart?.taxPrice || 0)}</span>
        </div>

        <div className="summary-row">
          <span>{shipping}</span>
          <span>{formatMoney(checkoutData?.shippingCost || 0)}</span>
        </div>

        <hr />

        <div className="summary-row total">
          <strong>{grand_total}</strong>
          <strong>
            {formatMoney(
              (cart?.subTotal || 0) +
                (cart?.taxPrice || 0) +
                (shippingMethod?.shippingCost || 0)
            )}
          </strong>
        </div>
      </div>

      <p className="secure-text">🔒 {secure_message}</p>

      <CheckoutNavigationButtons
        onBack={onBack}
        onNext={handleSubmit}
        backLabel={back}
        nextLabel={place_order}
      />
    </div>
  );
};

export default ReviewOrder;
