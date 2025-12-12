import "./Input.css";

const Input = ({
  label,
  name,
  value,
  onChange,
  type = "text",
  placeholder = "",
  className = "",
  required = false,
  disabled = false,
  maxLength = null,
  errorMessage = "",
  icon = null,
  ...props
}) => {
  return (
    <div className={`form-row ${className}`}>
      {label && (
        <label htmlFor={name} className="form-label">
          <span className="visually-hidden">{label}</span>
        </label>
      )}
      <div className={`form-input ${errorMessage ? "error" : ""}`}>
        {icon && <span className="form-input-icon">{icon}</span>}
        <input
          id={name}
          name={name}
          value={value}
          onChange={(e) => onChange(e)}
          type={type}
          placeholder={placeholder}
          required={required}
          disabled={disabled}
          maxLength={maxLength}
          {...props}
        />
      </div>
      {errorMessage && <div className="form-alert">{errorMessage}</div>}
    </div>
  );
};

export default Input;
