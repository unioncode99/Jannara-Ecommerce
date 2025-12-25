import React, { useState } from "react";
import Button from "../ui/Button";
import "./PaymentForm.css";
// import { CardElement, useStripe, useElements } from "@stripe/react-stripe-js";

const PaymentForm = ({ onNext, onBack, checkoutData }) => {
  // const stripe = useStripe();
  // const elements = useElements();
  // const [loading, setLoading] = useState(false);
  // const [error, setError] = useState(null);

  // const handleSubmit = async (e) => {
  //   e.preventDefault();
  //   setLoading(true);
  //   setError(null);

  //   try {
  //     const res = await fetch("/api/checkout", {
  //       method: "POST",
  //       headers: { "Content-Type": "application/json" },
  //       body: JSON.stringify({
  //         ...checkoutData,
  //         paymentMethod: "card",
  //         cartId: 1, // Replace with actual cartId
  //         customerId: 1, // Replace with actual customerId
  //       }),
  //     });
  //     const data = await res.json();

  //     const result = await stripe.confirmCardPayment(data.clientSecret, {
  //       payment_method: { card: elements.getElement(CardElement) },
  //     });

  //     if (result.error) setError(result.error.message);
  //     else if (result.paymentIntent.status === "succeeded") {
  //       onNext({ orderId: data.orderId, grandTotal: data.grandTotal });
  //     }
  //   } catch (err) {
  //     setError("Payment failed");
  //   } finally {
  //     setLoading(false);
  //   }
  // };

  const [method, setMethod] = useState("Credit Card");

  const handleSubmit = (e) => {
    e.preventDefault();
    onNext({ paymentMethod: method });
  };

  // return (
  //   <form onSubmit={handleSubmit}>
  //     <h2>Payment</h2>
  //     <CardElement />
  //     {error && <p style={{ color: "red" }}>{error}</p>}
  //     <div>
  //       <button type="button" onClick={onBack}>
  //         Back
  //       </button>
  //       <button type="submit" disabled={!stripe || loading}>
  //         {loading ? "Processing..." : "Pay Now"}
  //       </button>
  //     </div>
  //   </form>
  // );

  return (
    <form className="form payment-from" onSubmit={handleSubmit}>
      <h2>Payment Method</h2>
      <div className="radio-inputs-container">
        <input
          type="radio"
          id="creditCard"
          name="paymentMethod"
          value="Credit Card"
          checked={method === "Credit Card"}
          onChange={() => setMethod("Credit Card")}
        />
        <label htmlFor="creditCard">
          <span>Credit Card</span>
          <small>Visa, Mastercard</small>
        </label>
        <input
          type="radio"
          id="cod"
          name="paymentMethod"
          value="COD"
          checked={method === "COD"}
          onChange={() => setMethod("COD")}
        />
        <label htmlFor="cod">
          <span>COD</span>
          <small>Pay on delivery</small>
        </label>
      </div>

      <div className="checkout-navigate-buttons-container">
        <Button className="btn btn-primary" onClick={onBack}>
          Back
        </Button>
        <Button className="btn btn-primary" type="submit">
          Next
        </Button>
      </div>
    </form>
  );
};

export default PaymentForm;
