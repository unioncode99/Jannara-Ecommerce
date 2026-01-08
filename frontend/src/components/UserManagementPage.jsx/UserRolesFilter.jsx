import { useEffect, useState } from "react";
import { read } from "../../api/apiWrapper";
import { useLanguage } from "../../hooks/useLanguage";
import Select from "../ui/Select";

const UserRolesFilter = ({ value, onChange }) => {
  const [roles, setRoles] = useState([]);
  const { language, translations } = useLanguage();
  const { role } = translations.general.pages.users_management;

  const fetchUsers = async () => {
    try {
      // setLoading(true);
      const data = await read(`roles`); // for test
      const options = data.map((role) => ({
        value: role.id,
        label: language === "en" ? role.nameEn : role.nameAr,
      }));
      setRoles(options);
      console.log("data", data);
    } catch (error) {
      console.error("Failed to fetch categories:", error);
    } finally {
      // setLoading(false);
    }
  };

  useEffect(() => {
    fetchUsers();
  }, []);

  return (
    <Select options={roles} label={role} value={value} onChange={onChange} />
  );
};
export default UserRolesFilter;
