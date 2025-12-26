import ShippingMethodItem from "./ShippingMethodItem";

const ShippingMethodList = ({
  methods,
  selectedShippingMethodId,
  setSelectedShippingMethodId,
  calculateShippingCost,
  cartSubTotal,
  cartTotalWeight,
  stateFee,
}) => {
  return (
    <div className="radio-inputs-container">
      {methods?.map((method) => {
        const cost = calculateShippingCost(method, stateFee);
        return (
          <ShippingMethodItem
            key={method.id}
            method={method}
            selectedShippingMethodId={selectedShippingMethodId}
            setSelectedShippingMethodId={setSelectedShippingMethodId}
            cost={cost}
            isFree={cost === 0}
            cartSubTotal={cartSubTotal}
            cartTotalWeight={cartTotalWeight}
          />
        );
      })}
    </div>
  );
};

export default ShippingMethodList;
