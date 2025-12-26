import { useState } from "react";
import "./CheckoutPage.css";
import AddressForm from "../components/CheckoutPage/AddressForm";
import ShippingMethodForm from "../components/CheckoutPage/ShippingMethodForm";
import PaymentForm from "../components/CheckoutPage/PaymentForm";
import ReviewOrder from "../components/CheckoutPage/ReviewOrder";

const CheckoutPage = () => {
  const [step, setStep] = useState(1);
  const [checkoutData, setCheckoutData] = useState({});

  const handleNextStep = (data) => {
    setCheckoutData((prev) => ({ ...prev, ...data }));
    setStep((prev) => prev + 1);
  };

  const handlePrevStep = () => setStep((prev) => prev - 1);

  return (
    <div className="checkout-wizard">
      <div className="steps-indicator">
        <div className="step-container active-step-container">
          <span>{step}</span>
          <span>
            {step === 4
              ? "Review"
              : step === 3
              ? "Payment"
              : step === 2
              ? "Shipping"
              : "Address"}
          </span>
        </div>
        {step <= 3 && (
          <div className="step-container">
            <span>{step + 1}</span>
            {step == 3 ? "Review" : step == 2 ? "Payment" : "Shipping"}
          </div>
        )}
      </div>
      {step === 1 && <AddressForm onNext={handleNextStep} />}
      {step === 2 && (
        <ShippingMethodForm
          checkoutData={checkoutData}
          onNext={handleNextStep}
          onBack={handlePrevStep}
        />
      )}
      {step === 3 && (
        <PaymentForm
          onNext={handleNextStep}
          onBack={handlePrevStep}
          checkoutData={checkoutData}
        />
      )}
      {step === 4 && (
        <ReviewOrder onBack={handlePrevStep} checkoutData={checkoutData} />
      )}
    </div>
  );
};
export default CheckoutPage;
