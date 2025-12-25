import React, { useState } from "react";
import "./ShippingMethodForm.css";
import Button from "../ui/Button";

const ShippingMethodForm = ({ onNext, onBack }) => {
  const [method, setMethod] = useState("Standard");

  const handleSubmit = (e) => {
    e.preventDefault();
    onNext({ shippingMethod: method });
  };

  return (
    <form className="form radio-input-form" onSubmit={handleSubmit}>
      <h2>Shipping Method</h2>
      <input
        type="radio"
        id="standard"
        name="shippingMethod"
        value="Standard"
        checked={method === "Standard"}
        onChange={() => setMethod("Standard")}
      />
      <label htmlFor="standard">Standard (5$)</label>
      <input
        type="radio"
        id="express"
        name="shippingMethod"
        value="Express"
        checked={method === "Express"}
        onChange={() => setMethod("Express")}
      />
      <label htmlFor="express">Express (20$)</label>
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

export default ShippingMethodForm;
