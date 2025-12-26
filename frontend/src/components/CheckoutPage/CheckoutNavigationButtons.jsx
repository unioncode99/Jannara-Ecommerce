import Button from "../ui/Button";

const CheckoutNavigationButtons = ({
  onBack,
  onNext,
  backLabel,
  nextLabel,
  nextDisabled = false,
  nextType = "submit",
}) => {
  return (
    <div
      className={`checkout-navigate-buttons-container ${
        !onBack ? "next-btn-container" : ""
      }`}
    >
      {onBack && (
        <Button className="btn btn-primary" onClick={onBack}>
          {backLabel}
        </Button>
      )}

      <Button
        disabled={nextDisabled}
        className="btn btn-primary"
        type={nextType}
        onClick={onNext}
      >
        {nextLabel}
      </Button>
    </div>
  );
};
export default CheckoutNavigationButtons;
