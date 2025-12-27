import Button from "../ui/Button";
import Input from "../ui/Input";
import { useLanguage } from "../../hooks/useLanguage";
import { useEffect, useState } from "react";
import Select from "../ui/Select";

const AddUpdateAddressForm = ({
  form,
  setForm,
  onSubmit,
  states,
  errors,
  cancel,
}) => {
  console.log("states", states);
  console.log("form", form);

  const { language, translations } = useLanguage();
  const [options, setOptions] = useState([]);

  const {
    state,
    city,
    locality,
    street,
    building_number,
    phone,
    save_address,
    cancel_text,
  } = translations.general.pages.checkout;

  useEffect(() => {
    const options = states.map((state) => ({
      value: state.id,
      label: language === "en" ? state.nameEn : state.nameAr,
    }));
    setOptions(options);
  }, [language, states]);

  const handlePhoneChange = (e) => {
    if (/^\d*$/.test(e.target.value)) {
      setForm({ ...form, phone: e.target.value });
    }
  };

  return (
    <div>
      <Select
        options={options}
        label={state}
        value={form.stateId ?? ""}
        onChange={(e) => setForm({ ...form, stateId: Number(e.target.value) })}
        errorMessage={errors.stateId}
        required={true}
      />
      <Input
        label={city}
        placeholder={city}
        value={form?.city}
        onChange={(e) => setForm({ ...form, city: e.target.value })}
        errorMessage={errors.city}
        required
      />
      <Input
        label={locality}
        placeholder={locality}
        value={form?.locality}
        onChange={(e) => setForm({ ...form, locality: e.target.value })}
        errorMessage={errors.locality}
        required
      />
      <Input
        label={street}
        placeholder={street}
        value={form?.street}
        onChange={(e) => setForm({ ...form, street: e.target.value })}
        errorMessage={errors.street}
        required
      />
      <Input
        label={building_number}
        placeholder={building_number}
        value={form?.buildingNumber}
        onChange={(e) => setForm({ ...form, buildingNumber: e.target.value })}
        errorMessage={errors.buildingNumber}
        required
      />
      <Input
        label={phone}
        placeholder={phone}
        value={form?.phone}
        onChange={(e) => handlePhoneChange(e)}
        errorMessage={errors.phone}
        required
      />
      <Button
        style={{ marginBottom: "0.5rem" }}
        className="btn btn-primary btn-block"
        onClick={onSubmit}
      >
        {save_address}
      </Button>
      <Button
        style={{ marginBottom: "0.5rem" }}
        className="btn btn-primary btn-block"
        onClick={cancel}
      >
        {cancel_text}
      </Button>
    </div>
  );
};
export default AddUpdateAddressForm;
