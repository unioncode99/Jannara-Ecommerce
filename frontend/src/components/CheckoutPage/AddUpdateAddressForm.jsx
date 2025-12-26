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

  const { language } = useLanguage();
  const [options, setOptions] = useState([]);

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
        label={language == "en" ? "State" : "الولاية"}
        value={form.stateId ?? ""}
        onChange={(e) => setForm({ ...form, stateId: Number(e.target.value) })}
        errorMessage={errors.stateId}
        required={true}
      />
      <Input
        label="City"
        placeholder="City"
        value={form?.city}
        onChange={(e) => setForm({ ...form, city: e.target.value })}
        errorMessage={errors.city}
        required
      />
      <Input
        label="Locality"
        placeholder="Locality"
        value={form?.locality}
        onChange={(e) => setForm({ ...form, locality: e.target.value })}
        errorMessage={errors.locality}
        required
      />
      <Input
        label="Street"
        placeholder="Street"
        value={form?.street}
        onChange={(e) => setForm({ ...form, street: e.target.value })}
        errorMessage={errors.street}
        required
      />
      <Input
        label="Building Number"
        placeholder="Building Number"
        value={form?.buildingNumber}
        onChange={(e) => setForm({ ...form, buildingNumber: e.target.value })}
        errorMessage={errors.buildingNumber}
        required
      />
      <Input
        label="Phone"
        placeholder="Phone"
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
        Save Address
      </Button>
      <Button
        style={{ marginBottom: "0.5rem" }}
        className="btn btn-primary btn-block"
        onClick={cancel}
      >
        Cancel
      </Button>
    </div>
  );
};
export default AddUpdateAddressForm;
