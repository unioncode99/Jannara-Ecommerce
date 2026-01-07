import { useLanguage } from "../../hooks/useLanguage";
import Button from "../ui/Button";
import Modal from "../ui/Modal";
import "./UserRolesModal.css";

const UserRolesModal = ({ show, onClose, user, handleToggleUserStatus }) => {
  const { language, translations } = useLanguage();
  const {
    manage_user_roles,
    activate_role,
    deactivate_role,
    active,
    inactive,
  } = translations.general.pages.users_management;
  return (
    <Modal
      show={show}
      onClose={onClose}
      title={manage_user_roles}
      className="confirm-modal"
    >
      <div className="user-roles-list">
        {user?.roles?.map((role) => (
          <div className="role-item" key={role.id}>
            <small className="user-status">
              {language == "en" ? role.nameEn : role.nameAr}
            </small>
            <small
              className={`user-status ${
                user?.roles[0]?.isActive ? "active" : "inactive"
              }`}
            >
              {role.isActive ? active : inactive}
            </small>
            <Button
              className="btn btn-primary"
              onClick={() => handleToggleUserStatus(role)}
            >
              {role.isActive ? deactivate_role : activate_role}
            </Button>
          </div>
        ))}
      </div>
    </Modal>
  );
};
export default UserRolesModal;
