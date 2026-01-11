import Input from "../../ui/Input";
import Button from "../../ui/Button";
import { Trash2 } from "lucide-react";
import "./OptionItem.css";
import { useLanguage } from "../../../hooks/useLanguage";

const OptionItem = ({
  option,
  variationIndex,
  optionIndex,
  updateOption,
  removeOption,
}) => {
  const { translations } = useLanguage();
  const { option_name_en, option_name_ar } =
    translations.general.pages.add_product;

  const handleNonEmptyUpdate = (value, currentValue, fieldName) => {
    const trimmed = value.trim();
    if (trimmed === "" || trimmed === currentValue) return; // prevent empty or no-change
    updateOption(variationIndex, optionIndex, fieldName, value);
  };

  return (
    <div className="option-container">
      <Input
        placeholder={option_name_en}
        value={option.nameEn}
        onChange={(e) =>
          handleNonEmptyUpdate(e.target.value, option.nameEn, "nameEn")
        }
      />
      <Input
        placeholder={option_name_ar}
        value={option.nameAr}
        onChange={(e) =>
          handleNonEmptyUpdate(e.target.value, option.nameAr, "nameAr")
        }
      />
      <Button
        className="delete-btn"
        onClick={() => removeOption(variationIndex, optionIndex)}
      >
        <Trash2 />
      </Button>
    </div>
  );
};

export default OptionItem;
