import { Plus, Tags, Trash2 } from "lucide-react";
import "./Variations.css";
import { useState } from "react";
import VariationForm from "./VariationForm";
import VariationItem from "./VariationItem";
import { create, remove, update } from "../../../api/apiWrapper";
import { toast } from "../../ui/Toast";
import { useLanguage } from "../../../hooks/useLanguage";

const Variations = ({ productData, setProductData, isModeUpdate }) => {
  // Store new option inputs per variation
  const [newOptions, setNewOptions] = useState({});

  const { translations, language } = useLanguage();

  const {
    product_variation_create_success,
    product_variation_create_failed,
    product_variation_update_success,
    product_variation_update_failed,
    product_variation_delete_success,
    product_variation_delete_failed,
    product_variation_option_create_success,
    product_variation_option_create_failed,
    product_variation_option_update_success,
    product_variation_option_update_failed,
    product_variation_option_delete_success,
    product_variation_option_delete_failed,
  } = translations.general.pages.products;

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

    if (isModeUpdate) {
      createVariation(newVariation);
    } else {
      setProductData({
        ...productData,
        variations: [...(productData.variations || []), newVariation],
      });
    }

    return true;
  };

  // Remove a variation
  const removeVariation = (index) => {
    if (!productData.variations?.[index]) {
      return;
    }

    const variation = productData.variations[index];

    if (isModeUpdate) {
      deleteVariation(variation.id);
    } else {
      setProductData((prev) => ({
        ...prev,
        variations: prev.variations.filter((_, i) => i !== index),
      }));
    }

    // Remove corresponding new option input
    setNewOptions((prev) => {
      const updated = { ...prev };
      delete updated[index];
      return updated;
    });
  };

  // Update variation name
  const updateVariationName = (index, field, value) => {
    if (!productData.variations?.[index]) {
      return;
    }

    const updatedVariation = {
      ...productData.variations[index],
      [field]: value,
    };

    if (isModeUpdate) {
      updateVariation(updatedVariation);
    } else {
      setProductData((prev) => ({
        ...prev,
        variations: prev.variations.map((v, i) =>
          i === index ? updatedVariation : v
        ),
      }));
    }
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

  async function createVariation(variation) {
    try {
      const payload = {
        productId: productData.id,
        nameEn: variation?.nameEn,
        nameAr: variation?.nameAr,
      };
      const result = await create(`variations`, payload);

      const newVariation = result.data || result;

      console.log("result -> ", result);

      // const updatedVariations = productData.variations.map((variation) => {
      //   return {
      //     ...variation,
      //     nameEn: result.nameEn,
      //     nameAr: result.nameAr,
      //     createdAt: result.createdAt,
      //     updatedAt: result.updatedAt,
      //   };
      // });

      // setProductData({
      //   ...productData,
      //   variations: updatedVariations,
      // });

      setProductData((prev) => ({
        ...prev,
        variations: [...(prev.variations || []), newVariation],
      }));

      if (translations.general.server_messages[result?.message?.message]) {
        toast.show(
          translations.general.server_messages[result?.message?.message],
          "success"
        );
      } else {
        toast.show(product_variation_create_success, "success");
      }
    } catch (error) {
      console.error(error);
      if (translations.general.server_messages[error.message]) {
        toast.show(
          translations.general.server_messages[error.message],
          "error"
        );
      } else {
        toast.show(product_variation_create_failed, "error");
      }
    }
  }

  async function updateVariation(variation) {
    try {
      const payload = {
        id: variation.id,
        productId: productData.id,
        nameEn: variation?.nameEn,
        nameAr: variation?.nameAr,
      };
      const result = await update(`variations/${payload.id}`, payload);
      console.log("result -> ", result);

      // Merge backend result with existing variation to preserve options
      const existingVariation = productData.variations.find(
        (v) => v.id === variation.id
      );
      const updatedVariation = {
        ...existingVariation,
        ...result, // overwrite only fields returned from API
      };

      setProductData((prev) => ({
        ...prev,
        variations: prev.variations.map((v) =>
          v.id === updatedVariation.id ? updatedVariation : v
        ),
      }));

      if (translations.general.server_messages[result?.message?.message]) {
        toast.show(
          translations.general.server_messages[result?.message?.message],
          "success"
        );
      } else {
        toast.show(product_variation_update_success, "success");
      }
    } catch (error) {
      console.error(error);
      if (translations.general.server_messages[error.message]) {
        toast.show(
          translations.general.server_messages[error.message],
          "error"
        );
      } else {
        toast.show(product_variation_update_failed, "error");
      }
    }
  }

  async function deleteVariation(variationId) {
    try {
      const result = await remove(`variations/${variationId}`);
      console.log("result -> ", result);

      setProductData((prev) => ({
        ...prev,
        variations: prev.variations.filter((v) => v.id !== variationId),
      }));

      if (translations.general.server_messages[result?.message?.message]) {
        toast.show(
          translations.general.server_messages[result?.message?.message],
          "success"
        );
      } else {
        toast.show(product_variation_delete_success, "success");
      }
    } catch (error) {
      console.error(error);
      if (translations.general.server_messages[error.message]) {
        toast.show(
          translations.general.server_messages[error.message],
          "error"
        );
      } else {
        toast.show(product_variation_delete_failed, "error");
      }
    }
  }

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
