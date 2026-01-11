import { Plus } from "lucide-react";
import Input from "../../ui/Input";
import Button from "../../ui/Button";
import "./OptionForm.css";
import { useLanguage } from "../../../hooks/useLanguage";

const OptionForm = ({
  variationIndex,
  addOption,
  newOption,
  updateOptionInput,
}) => {
  const { translations } = useLanguage();
  const { option_name_en, option_name_ar } =
    translations.general.pages.add_product;

  return (
    <div className="add-option-inputs">
      <Input
        placeholder={option_name_en}
        value={newOption.nameEn}
        onChange={(e) =>
          updateOptionInput(variationIndex, "nameEn", e.target.value)
        }
      />
      <Input
        placeholder={option_name_ar}
        value={newOption.nameAr}
        onChange={(e) =>
          updateOptionInput(variationIndex, "nameAr", e.target.value)
        }
      />
      <Button
        className="btn btn-primary"
        onClick={() => addOption(variationIndex)}
      >
        <Plus />
      </Button>
    </div>
  );
};

export default OptionForm;
