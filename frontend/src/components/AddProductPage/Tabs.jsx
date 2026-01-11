import Button from "../ui/Button";
import "./Tabs.css";

const Tabs = ({ steps, selectedStep, setStep }) => {
  return (
    <div className="steps-container">
      {steps?.map((step) => (
        <Button
          key={step.id}
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
