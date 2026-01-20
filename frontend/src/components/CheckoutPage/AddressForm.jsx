import React, { useEffect, useState } from "react";
import Input from "../ui/Input";
import Button from "../ui/Button";
import { create, read, update } from "../../api/apiWrapper";
import AddUpdateAddressForm from "./AddUpdateAddressForm";
import { SquarePen } from "lucide-react";
import "./AddressForm.css";
import { useLanguage } from "../../hooks/useLanguage";
import { toast } from "../ui/Toast";
import CheckoutNavigationButtons from "./CheckoutNavigationButtons";
import { useAuth } from "../../hooks/useAuth";

const AddressForm = ({ onNext }) => {
  const [addresses, setAddresses] = useState([]);
  const [states, setStates] = useState([]);
  const [selectedAddressId, setSelectedAddressId] = useState(null);
  const [showForm, setShowForm] = useState(false);
  const { translations, language } = useLanguage();
  const { person } = useAuth();

  const [form, setForm] = useState({
    personId: person?.id,
    stateId: states[0]?.id || undefined,
    city: "",
    locality: "",
    street: "",
    buildingNumber: "",
    phone: "",
  });
  const [isUpdateMode, setIsUpdateMode] = useState(false);
  const [errors, setErrors] = useState({});
  const { next, shipping_address_title, add_new_address } =
    translations.general.pages.checkout;

  async function fetchPersonAddress(personId) {
    try {
      const data = await read(`addresses?personId=${personId}`);
      setAddresses(data?.data?.addresses);
      setStates(data?.data?.states);
      console.log("data -> ", data);
      console.log("addresses -> ", addresses);
      console.log("states -> ", states);
    } catch (err) {
      console.error("Failed to load addresses", err);
      // setError("Failed to load addresses", err);
    }
  }

  useEffect(() => {
    if (!person) {
      return;
    }
    fetchPersonAddress(person?.id);
  }, [person]);

  const handleSubmit = (e) => {
    e.preventDefault();

    const selectedAddress = addresses.find(
      (addr) => addr.id === selectedAddressId,
    );
    const selectedState = states.find(
      (state) => state.id === selectedAddress.stateId,
    );

    // onNext(form);
    onNext({
      shippingAddress: {
        ...selectedAddress,
        state: {
          ...selectedState,
        },
      },
    });
  };

  const handleAddUpdateAddress = async () => {
    if (!validateFormData()) {
      toast.show(translations.general.form.messages.general_error, "error");
      return;
    }

    let address;
    if (isUpdateMode) {
      await update(`addresses/${selectedAddressId}`, form);
      console.log("Address updated:", form);
    } else {
      address = await create(`addresses`, form);
      console.log("New address created:", address);
    }
    // setAddresses((prev) => [...prev, address]);
    await fetchPersonAddress(person?.id);
    setSelectedAddressId(address?.id);
    setShowForm(false);
    setIsUpdateMode(false);
    setForm({
      personId: person?.id,
      stateId: states[0]?.id || undefined,
      city: "",
      locality: "",
      street: "",
      buildingNumber: "",
      phone: "",
    });
  };

  function handleEditAddress(address) {
    setIsUpdateMode(true);
    setForm(address);
    setShowForm(true);
  }

  function handleAddAddress() {
    setIsUpdateMode(false);
    setShowForm(true);
    setForm({
      personId: person?.id,
      stateId: states[0]?.id || undefined,
      city: "",
      locality: "",
      street: "",
      buildingNumber: "",
      phone: "",
    });
  }

  const validateFormData = () => {
    let temp = {};
    const msgs = translations.general.form.errors;

    if (!form.stateId) {
      temp.stateId = msgs.required;
    }
    if (!form.city.trim()) {
      temp.city = msgs.required;
    }
    if (!form.locality.trim()) {
      temp.locality = msgs.required;
    }
    if (!form.street.trim()) {
      temp.street = msgs.required;
    }
    if (!form.buildingNumber.trim()) {
      temp.buildingNumber = msgs.required;
    }

    if (!form.phone.trim() || form.phone.length < 10) {
      temp.phone = msgs.invalid_phone;
    }

    setErrors(temp);

    return Object.keys(temp).length === 0; // true = valid
  };

  function handelCancel() {
    setIsUpdateMode(false);
    setShowForm(false);
    setForm({
      personId: person?.id,
      stateId: states[0]?.id || undefined,
      city: "",
      locality: "",
      street: "",
      buildingNumber: "",
      phone: "",
    });
  }

  return (
    <form className="form shipping-address-form" onSubmit={handleSubmit}>
      <h2>{shipping_address_title}</h2>

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
              <span>
                {address.street}, {address.city},{" "}
                {language == "en"
                  ? states?.find((s) => s.id === address.stateId)?.nameEn
                  : states?.find((s) => s.id === address.stateId)?.nameAr}
              </span>{" "}
              <SquarePen onClick={() => handleEditAddress(address)} />
            </label>
          </>
        ))}
      </div>

      {(addresses?.length < 5 || !addresses) && (
        <Button className="btn btn-primary" onClick={handleAddAddress}>
          {add_new_address}
        </Button>
      )}

      {showForm && (
        <AddUpdateAddressForm
          states={states}
          form={form}
          setForm={setForm}
          errors={errors}
          cancel={handelCancel}
          onSubmit={handleAddUpdateAddress}
        />
      )}

      <CheckoutNavigationButtons
        onNext={handleSubmit}
        nextLabel={next}
        nextDisabled={!selectedAddressId}
      />

      {/* <div className="next-btn-container">
        <Button
          disabled={!selectedAddressId}
          className="btn btn-primary"
          type="submit"
        >
          Next
        </Button>
      </div> */}
    </form>
  );
};

export default AddressForm;
