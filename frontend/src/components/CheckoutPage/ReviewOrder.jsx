import React from "react";
import { useCart } from "../../contexts/CartContext";
import { formatMoney } from "../../utils/utils";
import "./ReviewOrder.css";
import Button from "../ui/Button";
import { useLanguage } from "../../hooks/useLanguage";
import CheckoutNavigationButtons from "../CheckoutPage/CheckoutNavigationButtons";

const ReviewOrder = ({ checkoutData, onBack }) => {
  const { cart } = useCart();
  console.log("checoutdat -> ", checkoutData);
  console.log("cart -> ", cart);
  const { translations } = useLanguage();
  const { back, place_order } = translations.general.pages.checkout;

  const handleSubmit = () => {
    console.log("Order Placed! 🎉");
  };

  return (
    <div className="review-order-container">
      <h2>Review Order</h2>
      {/* <p>
        <strong>Name:</strong> {checkoutData.fullName}
      </p>
      <p>
        <strong>Email:</strong> {checkoutData.email}
      </p>
      <p>
        <strong>Phone:</strong> {checkoutData.phone}
      </p> */}
      <p>
        <strong>Address:</strong>{" "}
        <span>
          {checkoutData.state}, {checkoutData.city}, {checkoutData.locality},{" "}
          {checkoutData.street}, {checkoutData.buildingNumber}
        </span>
      </p>
      <p>
        <strong>Shipping Method:</strong>{" "}
        <span>{checkoutData.shippingMethod}</span>
      </p>
      <p>
        <strong>Payment Method:</strong>{" "}
        <span>{checkoutData.paymentMethod}</span>
      </p>
      {/* <p>
        <strong>Order ID:</strong> {checkoutData.orderId}
      </p> */}
      <p>
        <strong>Grand Total:</strong>{" "}
        <span>{formatMoney(cart?.grandTotal)}</span>
      </p>

      <CheckoutNavigationButtons
        onBack={onBack}
        onNext={handleSubmit}
        backLabel={back}
        nextLabel={place_order}
        nextDisabled={!checkoutData}
      />

      {/* <button onClick={() => alert("Order Placed! 🎉")}>Place Order</button> */}
    </div>
  );
};

export default ReviewOrder;
