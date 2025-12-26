import React, { useEffect, useState } from "react";
import "./ShippingMethodForm.css";
import Button from "../ui/Button";
import { read } from "../../api/apiWrapper";
import { useCart } from "../../contexts/CartContext";
import { formatMoney } from "../../utils/utils";
import { useLanguage } from "../../hooks/useLanguage.jsx";
import ShippingCartSummary from "./ShippingMethodForm/ShippingCartSummary.jsx";
import SelectedShippingSummary from "./ShippingMethodForm/SelectedShippingSummary.jsx";
import ShippingMethodList from "./ShippingMethodForm/ShippingMethodList.jsx";

const ShippingMethodForm = ({ onNext, onBack, checkoutData }) => {
  const [shippingMethods, setShippingMethods] = useState([]);
  const [selectedShippingMethodId, setSelectedShippingMethodId] =
    useState(null);
  const { cart } = useCart();
  const { translations } = useLanguage();
  const { shipping_method_title, back, next } =
    translations.general.pages.shipping_method;

  console.log("cart", cart);
  console.log("checkoutData", checkoutData);
  console.log("selectedShippingMethodId", selectedShippingMethodId);

  const handleSubmit = (e) => {
    e.preventDefault();
    onNext({
      shippingMethodId: selectedShippingMethodId,
      ...shippingMethods.find(
        (method) => method.id === selectedShippingMethodId
      ),
    });
  };

  async function fetchShippingMethods() {
    try {
      const data = await read(`shipping-methods`);
      setShippingMethods(data?.data);
      console.log("data -> ", data);
    } catch (err) {
      console.error("Failed to load addresses", err);
      // setError("Failed to load addresses", err);
    }
  }

  useEffect(() => {
    fetchShippingMethods();
  }, []);

  const calculateShippingCost = (method, stateFee) => {
    // SIMPLE CALCULATION: Base Price + (Weight × Price Per Kg) + (state fee)
    const weightKg = cart?.totalWeight || 0;
    const cartTotal = cart?.subTotal || 0;

    // Check for free shipping
    // if (method.freeThreshold && cartTotal >= method.freeThreshold) {
    if (method.freeOver && cartTotal >= method.freeOver) {
      return 0; // Free shipping
    }

    // Calculate cost
    let cost = method.basePrice;
    if (method.pricePerKg && weightKg > 0) {
      cost += weightKg * method.pricePerKg;
    }

    // Add per item cost if exists
    if (method.pricePerItem) {
      const itemCount = cart?.itemsCount || 0;
      cost += itemCount * method.pricePerItem;
    }

    // add state fee
    if (stateFee && method.basePrice) {
      cost += stateFee;
    }

    return cost;
  };

  return (
    <form className="form shipping-method-form" onSubmit={handleSubmit}>
      <h2>{shipping_method_title}</h2>

      <ShippingCartSummary
        cartSubTotal={cart?.subTotal}
        cartTotalWeight={cart?.totalWeight}
      />
      <ShippingMethodList
        methods={shippingMethods}
        calculateShippingCost={calculateShippingCost}
        selectedShippingMethodId={selectedShippingMethodId}
        setSelectedShippingMethodId={setSelectedShippingMethodId}
        cartSubTotal={cart?.subTotal}
        cartTotalWeight={cart?.totalWeight}
        stateFee={checkoutData?.extraFeeForShipping}
      />

      {selectedShippingMethodId && (
        <SelectedShippingSummary
          method={shippingMethods.find(
            (m) => m.id === selectedShippingMethodId
          )}
          cost={calculateShippingCost(
            shippingMethods.find((m) => m.id === selectedShippingMethodId)
          )}
        />
      )}

      <div className="checkout-navigate-buttons-container">
        <Button className="btn btn-primary" onClick={onBack}>
          {back}
        </Button>
        <Button
          disabled={!selectedShippingMethodId}
          className="btn btn-primary"
          type="submit"
        >
          {next}
        </Button>
      </div>
    </form>
  );
};

export default ShippingMethodForm;
