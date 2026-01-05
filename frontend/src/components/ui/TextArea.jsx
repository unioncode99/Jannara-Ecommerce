import React from "react";
import "./TextArea.css"; // Import the CSS file for the TextArea component

const TextArea = ({
  label,
  name,
  value,
  onChange,
  placeholder = "",
  className = "",
  required = false,
  disabled = false,
  maxLength = null,
  errorMessage = "",
  rows = 4, // Default rows
  cols = 50, // Default columns
  ...props
}) => {
  return (
    <div className={`form-row ${className}`}>
      {label && (
        <label htmlFor={name} className="form-label">
          <span className="visually-hidden">{label}</span>
        </label>
      )}
      <textarea
        className={`${errorMessage ? "error" : ""}`}
        id={name}
        name={name}
        value={value}
        onChange={(e) => onChange(e)}
        placeholder={placeholder}
        required={required}
        disabled={disabled}
        maxLength={maxLength}
        rows={rows}
        cols={cols}
        {...props}
      />
      {errorMessage && <div className="form-alert">{errorMessage}</div>}
    </div>
  );
};

export default TextArea;
