import Button from "../ui/Button";
import "./Tabs.css";

const Tabs = ({ steps, selectedStep, setStep, loading }) => {
  return (
    <div className="steps-container">
      {steps?.map((step) => (
        <Button
          key={step.id}
          disabled={loading}
          onClick={() => setStep(step.id)}
          className={`${step.id == selectedStep ? "active" : ""}`}
        >
          {step.name}
        </Button>
      ))}
    </div>
  );
};
export default Tabs;
