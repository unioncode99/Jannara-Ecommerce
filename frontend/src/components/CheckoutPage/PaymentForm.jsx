import React, { useEffect, useState } from "react";
import Button from "../ui/Button";
import "./PaymentForm.css";
import CheckoutNavigationButtons from "./CheckoutNavigationButtons";
import { useLanguage } from "../../hooks/useLanguage";
import { read } from "../../api/apiWrapper";
import PaymentMethodList from "./PaymentMethodForm/PaymentMethodList";
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

  const [paymentMethods, setPaymentMethods] = useState([]);
  const [selectedPaymentMethodId, setSelectedPaymentMethodId] = useState(null);

  const { translations } = useLanguage();
  const { back, next, payment_method_title } =
    translations.general.pages.checkout;

  async function fetchPaymentMethods() {
    try {
      const data = await read(`payment-methods`);
      setPaymentMethods(data?.data);
      console.log("data -> ", data);
    } catch (err) {
      console.error("Failed to load payment methods", err);
    }
  }

  useEffect(() => {
    fetchPaymentMethods();
  }, []);

  const handleSubmit = (e) => {
    e.preventDefault();
    onNext({
      paymentMethod: paymentMethods.find(
        (method) => method.id === selectedPaymentMethodId
      ),
    });
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
      <h2>{payment_method_title}</h2>

      <PaymentMethodList
        paymentMethods={paymentMethods}
        selectedPaymentMethodId={selectedPaymentMethodId}
        setSelectedPaymentMethodId={setSelectedPaymentMethodId}
      />

      <CheckoutNavigationButtons
        onBack={onBack}
        onNext={handleSubmit}
        backLabel={back}
        nextLabel={next}
        nextDisabled={!selectedPaymentMethodId}
      />
    </form>
  );
};

export default PaymentForm;
