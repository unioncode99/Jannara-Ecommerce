import "./Checkbox.css";

const Checkbox = ({ value, onChange, label, disabled }) => {
  return (
    <div className="active-toggle">
      <label className={`checkbox-label ${disabled ? "disabled" : ""}`}>
        <input
          type="checkbox"
          checked={value}
          disabled={disabled}
          onChange={(e) => onChange(e.target.checked)}
        />
        <span>{label}</span>
      </label>
    </div>
  );
};
export default Checkbox;
