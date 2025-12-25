import React, { useState } from "react";
import Input from "../ui/Input";
import Button from "../ui/Button";

const AddressForm = ({ onNext }) => {
  const [form, setForm] = useState({
    state: "",
    city: "",
    locality: "",
    street: "",
    buildingNumber: "",
  });

  const handleSubmit = (e) => {
    e.preventDefault();
    onNext(form);
  };

  return (
    <form className="form" onSubmit={handleSubmit}>
      <h2>Shipping Address</h2>
      <Input
        label="State"
        placeholder="State"
        value={form.state}
        onChange={(e) => setForm({ ...form, state: e.target.value })}
        required
      />
      <Input
        label="City"
        placeholder="City"
        value={form.city}
        onChange={(e) => setForm({ ...form, city: e.target.value })}
        required
      />
      <Input
        label="Locality"
        placeholder="Locality"
        value={form.locality}
        onChange={(e) => setForm({ ...form, locality: e.target.value })}
        required
      />
      <Input
        label="Street"
        placeholder="Street"
        value={form.street}
        onChange={(e) => setForm({ ...form, street: e.target.value })}
        required
      />
      <Input
        label="Building Number"
        placeholder="Building Number"
        value={form.buildingNumber}
        onChange={(e) => setForm({ ...form, buildingNumber: e.target.value })}
        required
      />
      <div className="next-btn-container">
        <Button className="btn btn-primary" type="submit">
          Next
        </Button>
      </div>
    </form>
  );
};

export default AddressForm;
