import React, { useEffect, useState } from "react";
import "./PaymentForm.css";
import CheckoutNavigationButtons from "./CheckoutNavigationButtons";
import { useLanguage } from "../../hooks/useLanguage";
import { create, read } from "../../api/apiWrapper";
import PaymentMethodList from "./PaymentMethodForm/PaymentMethodList";
import {
  CardElement,
  useStripe,
  useElements,
  Elements,
} from "@stripe/react-stripe-js";
import { loadStripe } from "@stripe/stripe-js";
import { useCart } from "../../contexts/CartContext";
import { toast } from "../../components/ui/Toast";
import { useNavigate } from "react-router-dom";

const stripePromise = loadStripe(import.meta.env.VITE_STRIPE_PUBLIC_KEY);

const PaymentFormInner = ({ onNext, onBack, checkoutData }) => {
  const [paymentMethods, setPaymentMethods] = useState([]);
  const [selectedPaymentMethodId, setSelectedPaymentMethodId] = useState(null);
  const [cardReady, setCardReady] = useState(false);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const { cart } = useCart();
  console.log("cart -> ", cart);

  // stripe
  const stripe = useStripe();
  const elements = useElements();

  const placeOrder = async (e) => {
    e.preventDefault();

    if (!selectedPaymentMethodId || loading) {
      return;
    }

    // CARD PAYMENT
    if (selectedPaymentMethodId == 2) {
      if (!stripe || !elements) {
        console.error("Stripe not ready");
        return;
      }

      const cardElement = elements.getElement(CardElement);
      if (!cardElement) {
        console.error("CardElement not found");
        return;
      }
    }

    setLoading(true);

    const orderDto = {
      cartId: cart?.id,
      customerId: 1, // for test
      paymentMethodId: selectedPaymentMethodId,
      shippingAddressId: checkoutData?.shippingAddress?.id,
      shippingMethodId: checkoutData?.shippingMethod?.id,
      grandTotal: checkoutData?.totalCost || 0,
      payNow: false,
      // taxRate: 0.15,
    };

    // to test stripe
    // Card number -> 4242 4242 4242 4242
    // Expiration date -> Any future date (e.g. 12 / 34)
    // CVC -> Any 3 digits (e.g. 123)
    // ZIP / Postal code -> Any (e.g. 12345)

    try {
      const data = await create(`orders/place`, orderDto);

      // CASH ON DELIVERY
      if (selectedPaymentMethodId == 1) {
        navigate(`/order-success/${data.order.publicOrderId}`);
        console.log("Order placed, pay later");
        return;
      }

      // CARD PAYMENT
      const cardElement = elements.getElement(CardElement);
      const result = await stripe.confirmCardPayment(data.clientSecret, {
        payment_method: {
          card: cardElement,
        },
      });
      if (result.error) {
        console.error(result.error.message);
        return;
      }
      if (result.paymentIntent.status === "succeeded") {
        // alert("✅ Payment successful");
        console.log("Payment successful");
      } else {
        toast.show("Payment failed. Please try again", "error");
      }

      // After successful payment
      if (
        selectedPaymentMethodId === 1 ||
        (selectedPaymentMethodId === 2 &&
          result.paymentIntent.status === "succeeded")
      ) {
        navigate(`/order-success/${data.order.publicOrderId}`);
      }
    } catch (err) {
      console.error("Failed to load payment methods", err);
    } finally {
      setLoading(false);
    }
  };

  const { translations } = useLanguage();
  const { back, pay_now, payment_method_title } =
    translations.general.pages.checkout;

  async function fetchPaymentMethods() {
    setLoading(true);
    try {
      const data = await read(`payment-methods`);
      setPaymentMethods(data?.data);
      console.log("data -> ", data);
    } catch (err) {
      console.error("Failed to load payment methods", err);
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    fetchPaymentMethods();
  }, []);

  return (
    <form className="form payment-from">
      <h2>{payment_method_title}</h2>

      <PaymentMethodList
        paymentMethods={paymentMethods}
        selectedPaymentMethodId={selectedPaymentMethodId}
        setSelectedPaymentMethodId={setSelectedPaymentMethodId}
      />

      {/* {selectedPaymentMethodId == 2 && (
        <div className="card-box">
          <CardElement
            onReady={() => setCardReady(true)}
            options={{ style: { base: { fontSize: "16px" } } }}
          />
        </div>
      )} */}

      <div
        className="card-box"
        style={{ display: selectedPaymentMethodId == 2 ? "block" : "none" }}
      >
        <CardElement onReady={() => setCardReady(true)} />
      </div>

      {/* <CheckoutNavigationButtons
        onBack={onBack}
        onNext={handleSubmit}
        backLabel={back}
        nextLabel={next}
        nextDisabled={!selectedPaymentMethodId}
      /> */}
      <CheckoutNavigationButtons
        onBack={onBack}
        onNext={placeOrder}
        backLabel={back}
        nextType="submit"
        // nextLabel={next}
        nextLabel={pay_now}
        nextDisabled={
          !selectedPaymentMethodId ||
          (selectedPaymentMethodId === 2 && !cardReady)
        }
      />
    </form>
  );
};

const PaymentForm = (props) => {
  return (
    <Elements stripe={stripePromise}>
      <PaymentFormInner {...props} />
    </Elements>
  );
};

export default PaymentForm;
