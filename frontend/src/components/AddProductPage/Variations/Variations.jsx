import { Plus, Tags, Trash2 } from "lucide-react";
import "./Variations.css";
import { useState } from "react";
import VariationForm from "./VariationForm";
import VariationItem from "./VariationItem";

const Variations = ({ productData, setProductData }) => {
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
      options: [],
    };
    setProductData({
      ...productData,
      Variations: [...(productData.Variations || []), newVariation],
    });
    return true;
  };

  // Remove a variation
  const removeVariation = (index) => {
    const updatedVariations = [...productData.Variations];
    updatedVariations.splice(index, 1);
    setProductData({ ...productData, Variations: updatedVariations });

    // Remove corresponding new option input
    const updatedNewOptions = { ...newOptions };
    delete updatedNewOptions[index];
    setNewOptions(updatedNewOptions);
  };

  // Update variation name
  const updateVariationName = (index, field, value) => {
    const updatedVariations = [...productData.Variations];
    updatedVariations[index][field] = value;
    setProductData({ ...productData, Variations: updatedVariations });
  };

  // Update option value
  const updateOption = (variationIndex, optionIndex, field, value) => {
    const updatedVariations = [...productData.Variations];
    updatedVariations[variationIndex].options[optionIndex][field] = value;
    setProductData({ ...productData, Variations: updatedVariations });
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
    const optionEn = newOptions[variationIndex]?.ValueEn || "";
    const optionAr = newOptions[variationIndex]?.ValueAr || "";
    if (!optionEn.trim() || !optionAr.trim()) return;

    const updatedVariations = [...productData.Variations];
    updatedVariations[variationIndex].options.push({
      ValueEn: optionEn,
      ValueAr: optionAr,
    });
    setProductData({ ...productData, Variations: updatedVariations });

    // Clear input
    setNewOptions({
      ...newOptions,
      [variationIndex]: { ValueEn: "", ValueAr: "" },
    });
  };

  // Remove an option
  const removeOption = (variationIndex, optionIndex) => {
    const updatedVariations = [...productData.Variations];
    updatedVariations[variationIndex].options.splice(optionIndex, 1);
    setProductData({ ...productData, Variations: updatedVariations });
  };

  return (
    <div className="add-variations-container">
      <VariationForm addVariation={addVariation} />

      {productData?.Variations?.map((variation, idx) => (
        <VariationItem
          key={idx}
          variation={variation}
          variationIndex={idx}
          removeVariation={removeVariation}
          updateVariationName={updateVariationName}
          updateOption={updateOption}
          removeOption={removeOption}
          addOption={addOption}
          newOption={newOptions[idx] || { ValueEn: "", ValueAr: "" }}
          updateOptionInput={updateOptionInput}
        />
      ))}
    </div>
  );
};

export default Variations;
