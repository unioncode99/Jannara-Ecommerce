import { Pencil, User } from "lucide-react";
import { useLanguage } from "../../hooks/useLanguage";
import Table from "../ui/Table";
import "./TableView.css";
import { formatDateTime } from "../../utils/utils";

const TableView = ({ users, handleDactivateUser }) => {
  const { translations, language } = useLanguage();
  const { name, roles, join_at, status, active, inactive, actions } =
    translations.general.pages.users_management;
  return (
    <div className="table-view">
      <Table
        headers={[name, roles, status, join_at, actions]}
        data={users.map((user) => ({
          name: (
            <div className="user-name-container">
              {!user.person.imageUrl && (
                <span>
                  {" "}
                  <User />
                </span>
              )}

              <span className="profile-image-container">
                {user.person.imageUrl && (
                  <img src={user.person.imageUrl} alt="Profile Image" />
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
          status: (
            <small
              className={`user-status ${
                user?.roles[0]?.isActive ? "active" : "inactive"
              }`}
            >
              {user?.roles[0]?.isActive ? active : inactive}
            </small>
          ),
          join_at: formatDateTime(user?.createdAt),
          Actions: (
            <div className="user-actions-btn-container">
              <button
                onClick={() => handleDactivateUser(user)}
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
