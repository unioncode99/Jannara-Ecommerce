import { useEffect, useState } from "react";
import Select from "./ui/Select";
import { useLanguage } from "../hooks/useLanguage";
import { read } from "../api/apiWrapper";

const ProductCategoriesDropdown = ({
  value,
  onChange,
  required = false,
  disabled = false,
  errorMessage = "",
  showLabel = false,
  label,
  name,
}) => {
  const [productCategories, setProductCategories] = useState([]);
  const { language } = useLanguage();

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const data = await read("product-categories");
        const options = data.map((cat) => ({
          value: cat.id,
          label: language === "en" ? cat.nameEn : cat.nameAr,
        }));
        setProductCategories(options);
        console.log("options", options);
      } catch (error) {
        console.error("Failed to fetch categories:", error);
      }
    };

    fetchCategories();
  }, [language]);

  return (
    <Select
      options={productCategories}
      label={label ? label : language == "en" ? "Category" : "التصنيف"}
      value={value}
      onChange={onChange}
      required={required}
      disabled={disabled}
      showLabel={showLabel}
      name={name}
      errorMessage={errorMessage}
    />
  );
};
export default ProductCategoriesDropdown;
