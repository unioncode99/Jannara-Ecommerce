import PaymentMethodItem from "./PaymentMethodItem";

const PaymentMethodList = ({
  paymentMethods,
  selectedPaymentMethodId,
  setSelectedPaymentMethodId,
}) => {
  return (
    <div className="radio-inputs-container">
      {paymentMethods.map((method) => (
        <PaymentMethodItem
          key={method.id}
          method={method}
          selectedPaymentMethodId={selectedPaymentMethodId}
          setSelectedPaymentMethodId={setSelectedPaymentMethodId}
        />
      ))}
    </div>
  );
};
export default PaymentMethodList;
