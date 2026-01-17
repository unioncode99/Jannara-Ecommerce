import { useState } from "react";
import "./SearchableSelect.css";
import Input from "./Input";
import { Search } from "lucide-react";
import { useLanguage } from "../../hooks/useLanguage";

const SearchableSelect = ({
  options = [],
  placeholder = "Search...",
  inputValue = "",
  onInputChange,
  onSelect,
  loading = false,
  valueKey = "id",
  labelKey = "name",
}) => {
  const [open, setOpen] = useState(false);

  const { language } = useLanguage();

  const handleChange = (e) => {
    const value = e.target.value;
    onInputChange?.(value);
    setOpen(value.trim().length > 0);
  };

  return (
    <div className="searchable-select">
      <Input
        type="search"
        placeholder={placeholder}
        icon={<Search />}
        value={inputValue}
        onChange={handleChange}
        onFocus={() => {
          inputValue && setOpen(true);
        }}
      />

      {open && (
        <div className="dropdown">
          {loading && (
            <div className="dropdown-item">
              {" "}
              {language == "en" ? "Loading..." : "جاري التحميل..."}
            </div>
          )}

          {!loading && options.length === 0 && (
            <div className="dropdown-item">
              {language == "en" ? "No results" : "لا توجد نتائج"}
            </div>
          )}

          {!loading &&
            options.map((item) => (
              <div
                key={item[valueKey]}
                className="dropdown-item"
                onClick={() => {
                  onSelect(item);
                  setOpen(false);
                }}
              >
                {item[labelKey]}
              </div>
            ))}
        </div>
      )}
    </div>
  );
};
export default SearchableSelect;
