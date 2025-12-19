import { useLanguage } from "../hooks/useLanguage";
import Select from "./ui/Select";

const ProductSortingFiltersDropdown = ({
  value,
  onChange,
  required = false,
  disabled = false,
  errorMessage = "",
}) => {
  const { language } = useLanguage();
  const sortingOptions =
    language === "en"
      ? [
          { value: "price_asc", label: "Price: Low to High" },
          { value: "price_desc", label: "Price: High to Low" },
          { value: "newest", label: "Newest" },
          { value: "oldest", label: "Oldest" },
        ]
      : [
          { value: "price_asc", label: "السعر: من الأقل إلى الأعلى" },
          { value: "price_desc", label: "السعر: من الأعلى إلى الأقل" },
          { value: "newest", label: "الأحدث" },
          { value: "oldest", label: "الأقدم" },
        ];

  return (
    <Select
      options={sortingOptions}
      label={language == "en" ? "Sort By" : "ترتيب حسب"}
      value={value}
      onChange={onChange}
      required={required}
      disabled={disabled}
      errorMessage={errorMessage}
    />
  );
};
export default ProductSortingFiltersDropdown;
