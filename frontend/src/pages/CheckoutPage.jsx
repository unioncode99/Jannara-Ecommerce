import { useState } from "react";
import "./CheckoutPage.css";
import AddressForm from "../components/CheckoutPage/AddressForm";
import ShippingMethodForm from "../components/CheckoutPage/ShippingMethodForm";
import PaymentForm from "../components/CheckoutPage/PaymentForm";
import ReviewOrder from "../components/CheckoutPage/ReviewOrder";
import { useLanguage } from "../hooks/useLanguage";

const CheckoutPage = () => {
  const [step, setStep] = useState(1);
  const [checkoutData, setCheckoutData] = useState({});
  const { translations } = useLanguage();

  const { address, shipping, payment, review } =
    translations.general.pages.checkout;

  const handleNextStep = (data) => {
    setCheckoutData((prev) => ({ ...prev, ...data }));
    setStep((prev) => prev + 1);
  };

  const handlePrevStep = () => setStep((prev) => prev - 1);

  const STEP_MAP = {
    address: 1,
    shipping: 2,
    payment: 3,
  };

  const handleEditStep = (section) => {
    const targetStep = STEP_MAP[section];
    if (targetStep) {
      setStep(targetStep);
      window.scrollTo({ top: 0, behavior: "smooth" });
    }
  };

  const stepLabels = {
    1: address,
    2: shipping,
    3: payment,
    4: review,
  };

  return (
    <div className="checkout-wizard">
      <div className="steps-indicator">
        <div className="step-container active-step-container">
          <span>{step}</span>
          <span>{stepLabels[step]}</span>
        </div>
        {step <= 3 && (
          <div className="step-container">
            <span>{step + 1}</span>
            {stepLabels[step + 1]}
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
        <ReviewOrder
          onBack={handlePrevStep}
          handleEditStep={handleEditStep}
          checkoutData={checkoutData}
        />
      )}
    </div>
  );
};
export default CheckoutPage;
