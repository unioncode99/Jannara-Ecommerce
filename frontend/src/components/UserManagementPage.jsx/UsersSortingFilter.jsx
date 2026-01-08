import { useLanguage } from "../../hooks/useLanguage";
import Select from "../ui/Select";

const UsersSortingFilter = ({
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
          { value: "email_asc", label: "Email: Ascending" },
          { value: "email_desc", label: "Email: Descending" },
          { value: "username_asc", label: "Username: Ascending" },
          { value: "username_desc", label: "Username: Descending" },
        ]
      : [
          { value: "newest", label: "الأحدث" },
          { value: "oldest", label: "الأقدم" },
          { value: "email_asc", label: "البريد الإلكتروني: تصاعدي" },
          { value: "email_desc", label: "البريد الإلكتروني: تنازلي" },
          { value: "username_asc", label: "اسم المستخدم: تصاعدي" },
          { value: "username_desc", label: "اسم المستخدم: تنازلي" },
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
export default UsersSortingFilter;
