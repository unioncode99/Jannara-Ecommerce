import Button from "../ui/Button";
import Input from "../ui/Input";

const NewAddressForm = ({ form, setForm, submitNewAddress }) => {
  return (
    <div>
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
      <Input
        label="Phone"
        placeholder="Phone"
        value={form.phone}
        onChange={(e) => setForm({ ...form, phone: e.target.value })}
        required
      />
      <Button
        style={{ marginBottom: "0.5rem" }}
        className="btn btn-primary btn-block"
        onClick={submitNewAddress}
      >
        Save Address
      </Button>
    </div>
  );
};
export default NewAddressForm;
