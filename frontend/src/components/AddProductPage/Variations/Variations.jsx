import { Plus, Tags, Trash2 } from "lucide-react";
import "./Variations.css";
import { useState } from "react";
import VariationForm from "./VariationForm";
import VariationItem from "./VariationItem";

const Variations = ({ productData, setProductData, isModeUpdate }) => {
  // Store new option inputs per variation
  const [newOptions, setNewOptions] = useState({});

  // Add a new variation
  const addVariation = (nameEn, nameAr) => {
    if (!nameEn.trim() || !nameAr.trim()) {
      return false;
    }
    const newVariation = {
      nameEn: nameEn,
      nameAr: nameAr,
      variationOptions: [],
    };
    setProductData({
      ...productData,
      variations: [...(productData.variations || []), newVariation],
    });
    return true;
  };

  // Remove a variation
  const removeVariation = (index) => {
    const updatedVariations = [...productData.variations];
    updatedVariations.splice(index, 1);
    setProductData({ ...productData, variations: updatedVariations });

    // Remove corresponding new option input
    const updatedNewOptions = { ...newOptions };
    delete updatedNewOptions[index];
    setNewOptions(updatedNewOptions);
  };

  // Update variation name
  const updateVariationName = (index, field, value) => {
    const updatedVariations = [...productData.variations];
    updatedVariations[index][field] = value;
    setProductData({ ...productData, variations: updatedVariations });
  };

  // Update option value
  const updateOption = (variationIndex, optionIndex, field, value) => {
    const updatedVariations = [...productData.variations];
    updatedVariations[variationIndex].variationOptions[optionIndex][field] =
      value;
    setProductData({ ...productData, variations: updatedVariations });
  };

  // Update new option input
  const updateOptionInput = (variationIndex, field, value) => {
    setNewOptions({
      ...newOptions,
      [variationIndex]: {
        ...newOptions[variationIndex],
        [field]: value,
      },
    });
  };

  // Add new option to variation
  const addOption = (variationIndex) => {
    const optionEn = newOptions[variationIndex]?.valueEn || "";
    const optionAr = newOptions[variationIndex]?.valueAr || "";
    if (!optionEn.trim() || !optionAr.trim()) return;

    const updatedVariations = [...productData.variations];
    updatedVariations[variationIndex].variationOptions.push({
      valueEn: optionEn,
      valueAr: optionAr,
    });
    setProductData({ ...productData, variations: updatedVariations });

    // Clear input
    setNewOptions({
      ...newOptions,
      [variationIndex]: { valueEn: "", valueAr: "" },
    });
  };

  // Remove an option
  const removeOption = (variationIndex, optionIndex) => {
    const updatedVariations = [...productData.variations];
    updatedVariations[variationIndex].variationOptions.splice(optionIndex, 1);
    setProductData({ ...productData, variations: updatedVariations });
  };

  return (
    <div className="add-variations-container">
      <VariationForm addVariation={addVariation} />

      {productData?.variations?.map((variation, idx) => (
        <VariationItem
          key={idx}
          variation={variation}
          variationIndex={idx}
          removeVariation={removeVariation}
          updateVariationName={updateVariationName}
          updateOption={updateOption}
          removeOption={removeOption}
          addOption={addOption}
          newOption={newOptions[idx] || { valueEn: "", valueAr: "" }}
          updateOptionInput={updateOptionInput}
        />
      ))}
    </div>
  );
};

export default Variations;
