import { useLanguage } from "../../hooks/useLanguage";
import "./Select.css";

const Select = ({
  label,
  name,
  value,
  onChange,
  options = [],
  className = "",
  required = false,
  disabled = false,
  errorMessage = "",
  ...props
}) => {
  const { language } = useLanguage();
  return (
    <div className={`form-row ${className} ${errorMessage ? "error" : ""}`}>
      {label && (
        <label htmlFor={name} className="form-label">
          <span className="visually-hidden">{label}</span>
        </label>
      )}
      <select
        id={name}
        name={name}
        value={value}
        onChange={(e) => onChange(e)}
        required={required}
        disabled={disabled}
        className="form-input"
        {...props}
      >
        <option value="" disabled>
          {language === "en" ? "Select an option" : "اختر خيارًا"}
        </option>
        {options.map((option, index) => (
          <option key={index} value={option.value}>
            {option.label}
          </option>
        ))}
      </select>
      {errorMessage && <div className="form-alert">{errorMessage}</div>}
    </div>
  );
};

export default Select;
