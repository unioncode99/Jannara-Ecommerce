import { useLanguage } from "../../../hooks/useLanguage";

const PaymentMethodItem = ({
  method,
  selectedPaymentMethodId,
  setSelectedPaymentMethodId,
}) => {
  const { language } = useLanguage();

  return (
    <div>
      {" "}
      <input
        type="radio"
        id={`payment-method-${method.id}`}
        name="paymentMethod"
        value={method.id}
        checked={method.id === selectedPaymentMethodId}
        onChange={() => setSelectedPaymentMethodId(method.id)}
      />
      <label htmlFor={`payment-method-${method.id}`}>
        <span>{language == "en" ? method.nameEn : method.nameAr}</span>
        <small>
          {language == "en" ? method.descriptionEn : method.descriptionAr}
        </small>
      </label>
    </div>
  );
};
export default PaymentMethodItem;
