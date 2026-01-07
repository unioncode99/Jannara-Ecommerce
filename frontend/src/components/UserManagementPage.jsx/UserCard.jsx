import { Pencil, Trash2, User } from "lucide-react";
import "./UserCard.css";
import { useLanguage } from "../../hooks/useLanguage";
import { formatDateTime } from "../../utils/utils";

const UserCard = ({ user, handleToggleUserStatus }) => {
  const { language, translations } = useLanguage();
  const { roles, join_at, status, active, inactive } =
    translations.general.pages.users_management;

  return (
    <div className="user-card">
      <h3 className="user-name-container">
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
      </h3>
      <p className="user-roles-container">
        <span>{roles}: </span>
        {user?.roles?.map((role) => (
          <small className="role">
            {language == "en" ? role.nameEn : role.nameAr}
          </small>
        ))}
      </p>
      <p>
        {join_at}: {formatDateTime(user?.createdAt)}
      </p>
      <p>
        {status}:{" "}
        <small
          className={`user-status ${
            user?.roles[0]?.isActive ? "active" : "inactive"
          }`}
        >
          {user?.roles[0]?.isActive ? active : inactive}
        </small>
      </p>
      <div className="user-actions-btn-container">
        <button
          onClick={() => handleToggleUserStatus(user)}
          className="edit-user-btn"
        >
          <Pencil />
        </button>
      </div>
    </div>
  );
};
export default UserCard;
