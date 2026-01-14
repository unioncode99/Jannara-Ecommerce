import { Loader2 } from "lucide-react";
import { useLanguage } from "../../hooks/useLanguage";
import Button from "../ui/Button";
import "./StepNavigation.css";

const StepNavigation = ({
  step,
  stepsLength,
  handlePrevious,
  handleNext,
  handleSubmit,
  loading,
}) => {
  const { translations } = useLanguage();
  const { previous, next, submit_product } =
    translations.general.pages.add_product;

  return (
    <div className={`step-navigation-container ${step == 1 ? "one" : ""}`}>
      {step > 1 && (
        <Button
          disabled={loading}
          onClick={handlePrevious}
          className="btn btn-primary"
        >
          {previous}
        </Button>
      )}
      {step < stepsLength && (
        <Button
          disabled={loading}
          onClick={handleNext}
          className="btn btn-primary"
        >
          {next}
        </Button>
      )}
      {step === stepsLength && (
        <Button
          disabled={loading}
          onClick={handleSubmit}
          className="btn btn-primary"
        >
          {loading ? <Loader2 className="animate-spin" /> : submit_product}
        </Button>
      )}
    </div>
  );
};
export default StepNavigation;
