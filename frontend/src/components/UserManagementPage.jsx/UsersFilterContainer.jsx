import { Search } from "lucide-react";
import Input from "../ui/Input";
import "./UsersFilterContainer.css";
import UsersSortingFilter from "./UsersSortingFilter";
import { useLanguage } from "../../hooks/useLanguage";
import UserRolesFilter from "./UserRolesFilter";

const UsersFilterContainer = ({
  searchText,
  handleSearchInputChange,
  sortingTerm,
  handleSortingTermChange,
  roleId,
  handleRoleIdChange,
}) => {
  const { translations } = useLanguage();
  const { search_label, search_user_placeholder } =
    translations.general.pages.users_management;
  return (
    <div className="users-filter-container">
      <Input
        label={search_label}
        name="userSearch"
        placeholder={search_user_placeholder}
        icon={<Search />}
        type="search"
        value={searchText}
        onChange={handleSearchInputChange}
      />
      <UsersSortingFilter
        value={sortingTerm}
        onChange={handleSortingTermChange}
      />
      <UserRolesFilter value={roleId} onChange={handleRoleIdChange} />
    </div>
  );
};
export default UsersFilterContainer;
