import React, { useEffect, useState } from "react";
import Input from "../ui/Input";
import Button from "../ui/Button";
import { create, read } from "../../api/apiWrapper";
import NewAddressForm from "./NewAddressForm";

const AddressForm = ({ onNext }) => {
  const [addresses, setAddresses] = useState([]);
  const [selectedAddressId, setSelectedAddressId] = useState(null);
  const [showForm, setShowForm] = useState(false);
  const [form, setForm] = useState({
    personId: 1, // for test
    state: "",
    city: "",
    locality: "",
    street: "",
    buildingNumber: "",
    phone: "",
  });
  // const personId = 1; // for test
  const [personId, setPersonId] = useState(1);

  async function fetchPersonAddress(personId) {
    try {
      const data = await read(`addresses?personId=${personId}`);
      setAddresses(data?.data);
      console.log("data -> ", data);
      console.log("addresses -> ", addresses);
    } catch (err) {
      console.error("Failed to load addresses", err);
      // setError("Failed to load addresses", err);
    }
  }

  useEffect(() => {
    if (!personId) {
      return;
    }
    fetchPersonAddress(personId);
  }, [personId]);

  const handleSubmit = (e) => {
    e.preventDefault();
    // onNext(form);
    onNext({
      shippingAddressId: selectedAddressId,
      ...addresses.find((addr) => addr.id === selectedAddressId),
    });
  };

  const submitNewAddress = async () => {
    const address = await create(`addresses`, form);
    console.log("New address created:", address);
    // setAddresses((prev) => [...prev, address]);
    await fetchPersonAddress(personId);
    setSelectedAddressId(address.id);
    setShowForm(false);
    setForm({
      personId: 1, // for test
      state: "",
      city: "",
      locality: "",
      street: "",
      buildingNumber: "",
      phone: "",
    });
  };

  return (
    <form className="form" onSubmit={handleSubmit}>
      <h2>Shipping Address</h2>

      <div className="radio-inputs-container">
        {addresses?.map((address) => (
          <>
            <input
              type="radio"
              id={`address-${address.id}`}
              name="shippingAddress"
              value={address.id}
              checked={selectedAddressId === address.id}
              onChange={() => setSelectedAddressId(address.id)}
            />
            <label htmlFor={`address-${address.id}`}>
              {address.street}, {address.city}, {address.state}
            </label>
          </>
        ))}
      </div>

      {addresses.length < 5 && (
        <Button className="btn btn-primary" onClick={() => setShowForm(true)}>
          Add New Address
        </Button>
      )}

      {showForm && (
        <NewAddressForm
          form={form}
          setForm={setForm}
          submitNewAddress={submitNewAddress}
        />
      )}
      <div className="next-btn-container">
        <Button
          disabled={!selectedAddressId}
          className="btn btn-primary"
          type="submit"
        >
          Next
        </Button>
      </div>
    </form>
  );
};

export default AddressForm;
