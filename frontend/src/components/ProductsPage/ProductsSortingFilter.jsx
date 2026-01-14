import { useLanguage } from "../../hooks/useLanguage";
import Select from "../ui/Select";

const ProductsSortingFilter = ({
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
          { value: "newest", label: "Newest" },
          { value: "oldest", label: "Oldest" },
        ]
      : [
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
export default ProductsSortingFilter;
