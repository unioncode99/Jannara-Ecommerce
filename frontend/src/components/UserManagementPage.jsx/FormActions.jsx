import { Loader2 } from "lucide-react";
import { useLanguage } from "../../hooks/useLanguage";
import Button from "../ui/Button";

const FormActions = ({ isLoading, clearRegisterForm, cancel }) => {
  const { translations } = useLanguage();
  const { clear_button, save, cancel: cancelText } = translations.general.form;
  return (
    <div>
      <Button
        className="btn btn-primary btn-block clear-btn"
        onClick={() => cancel()}
      >
        {cancelText}
      </Button>
      <Button
        className="btn btn-primary btn-block clear-btn"
        onClick={() => clearRegisterForm()}
      >
        {clear_button}
      </Button>
      <Button
        disabled={isLoading}
        className={`btn-primary btn-block ${isLoading ? "btn-disabled" : ""} `}
        type="submit"
      >
        {isLoading ? <Loader2 className="animate-spin" /> : save}
      </Button>
    </div>
  );
};
export default FormActions;
