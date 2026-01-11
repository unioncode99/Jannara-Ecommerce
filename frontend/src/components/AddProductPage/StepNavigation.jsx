import { useLanguage } from "../../hooks/useLanguage";
import Button from "../ui/Button";
import "./StepNavigation.css";

const StepNavigation = ({
  step,
  stepsLength,
  handlePrevious,
  handleNext,
  handleSubmit,
}) => {
  const { translations } = useLanguage();
  const { previous, next, submit_product } =
    translations.general.pages.add_product;

  return (
    <div className={`step-navigation-container ${step == 1 ? "one" : ""}`}>
      {step > 1 && (
        <Button onClick={handlePrevious} className="btn btn-primary">
          {previous}
        </Button>
      )}
      {step < stepsLength && (
        <Button onClick={handleNext} className="btn btn-primary">
          {next}
        </Button>
      )}
      {step === stepsLength && (
        <Button onClick={handleSubmit} className="btn btn-primary">
          {submit_product}
        </Button>
      )}
    </div>
  );
};
export default StepNavigation;
