import "./Button.css";

const Button = ({
  onClick,
  children,
  className = "",
  disabled = false,
  type = "button",
  icon = null,
  loading = false,
  ...props
}) => {
  const handleClick = (e) => {
    if (typeof onClick === "function") {
      onClick(e);
    }
  };

  return (
    <button
      type={type}
      onClick={handleClick}
      className={`btn ${className} ${disabled ? "btn-disabled" : ""} ${
        loading ? "btn-loading" : ""
      }`}
      disabled={disabled || loading} // Disable the button if it's in a loading state
      {...props}
    >
      {loading && <span className="spinner"></span>}{" "}
      {icon && <span className="btn-icon">{icon}</span>} {children}
    </button>
  );
};

export default Button;
