import { Pencil, User } from "lucide-react";
import { useLanguage } from "../../hooks/useLanguage";
import Table from "../ui/Table";
import "./TableView.css";
import { formatDateTime } from "../../utils/utils";

const TableView = ({ users, handleUserRoles }) => {
  const { translations, language } = useLanguage();
  const { name, roles, join_at, actions } =
    translations.general.pages.users_management;
  return (
    <div className="table-view">
      <Table
        headers={[name, roles, join_at, actions]}
        data={users.map((user) => ({
          name: (
            <div className="user-name-container">
              <span className="profile-image-container">
                {user.person.imageUrl ? (
                  <img src={user.person.imageUrl} alt="Profile Image" />
                ) : (
                  <User />
                )}
              </span>
              <div>
                <small>
                  {user?.person?.firstName + " " + user?.person?.lastName}
                </small>
                <small>{user?.email}</small>
              </div>
            </div>
          ),
          roles: (
            <p className="user-roles-container">
              {user?.roles?.map((role) => (
                <small className="role">
                  {language == "en" ? role.nameEn : role.nameAr}
                </small>
              ))}
            </p>
          ),
          join_at: formatDateTime(user?.createdAt),
          Actions: (
            <div className="user-actions-btn-container">
              <button
                onClick={() => handleUserRoles(user)}
                className="edit-user-btn"
              >
                <Pencil />
              </button>
            </div>
          ),
        }))}
      />
    </div>
  );
};
export default TableView;
