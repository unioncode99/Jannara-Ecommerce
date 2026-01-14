import { useEffect, useState } from "react";
import Select from "./ui/Select";
import { useLanguage } from "../hooks/useLanguage";
import { read } from "../api/apiWrapper";

const BrandsDropdown = ({
  value,
  onChange,
  required = false,
  disabled = false,
  errorMessage = "",
  label = "Select",
  showLabel = false,
  name,
}) => {
  const [productCategories, setProductCategories] = useState([]);
  const { language } = useLanguage();

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const data = await read("brands");
        const options = data.map((brand) => ({
          value: brand.id,
          label: language === "en" ? brand.nameEn : brand.nameAr,
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
      name={name}
      showLabel={showLabel}
      options={productCategories}
      label={label}
      value={value}
      onChange={onChange}
      required={required}
      disabled={disabled}
      errorMessage={errorMessage}
    />
  );
};
export default BrandsDropdown;
