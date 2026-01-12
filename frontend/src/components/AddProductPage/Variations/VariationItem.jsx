import OptionItem from "./OptionItem";
import OptionForm from "./OptionForm";
import Input from "../../ui/Input";
import Button from "../../ui/Button";
import { Trash2 } from "lucide-react";
import { useLanguage } from "../../../hooks/useLanguage";

const VariationItem = ({
  variation,
  variationIndex,
  removeVariation,
  updateVariationName,
  updateOption,
  removeOption,
  addOption,
  newOption,
  updateOptionInput,
}) => {
  const { translations } = useLanguage();
  const { variation_name_en, variation_name_ar, options_header } =
    translations.general.pages.add_product;

  const handleNonEmptyUpdate = (value, currentValue, fieldName) => {
    const trimmed = value.trim();
    if (trimmed === "" || trimmed === currentValue) return; // prevent empty or no-change
    updateVariationName(variationIndex, fieldName, value);
  };

  return (
    <div className="variation-container">
      <Button
        className="delete-btn"
        onClick={() => removeVariation(variationIndex)}
      >
        <Trash2 />
      </Button>

      <div>
        <Input
          placeholder={variation_name_en}
          value={variation.nameEn}
          onChange={(e) =>
            handleNonEmptyUpdate(e.target.value, variation.nameEn, "nameEn")
          }
        />
        <Input
          placeholder={variation_name_ar}
          value={variation.nameAr}
          onChange={(e) =>
            handleNonEmptyUpdate(e.target.value, variation.nameAr, "nameAr")
          }
        />
      </div>

      <h4>{options_header}</h4>
      {variation.variationOptions?.map((option, idx) => (
        <OptionItem
          key={idx}
          option={option}
          variationIndex={variationIndex}
          optionIndex={idx}
          updateOption={updateOption}
          removeOption={removeOption}
        />
      ))}

      <OptionForm
        variationIndex={variationIndex}
        addOption={addOption}
        newOption={newOption}
        updateOptionInput={updateOptionInput}
      />
    </div>
  );
};

export default VariationItem;
