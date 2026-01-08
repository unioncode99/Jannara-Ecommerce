import { User } from "lucide-react";
import { useLanguage } from "../../hooks/useLanguage";
import "./RecentRegistrations.css";
import { Link } from "react-router-dom";

const RecentRegistrations = ({ lastRegisteredUsers }) => {
  const { translations, language } = useLanguage();

  const { recent_registrations, view_all_users } =
    translations.general.pages.admin_dashboard;

  return (
    <div className="admin-recent-registrations-container">
      <h3>
        <span>{recent_registrations}</span>
        <Link to="/users" className="view-all-orders-link">
          <small>{view_all_users}</small>
        </Link>
      </h3>
      {lastRegisteredUsers?.map((user) => (
        <div className="recent-registration-item">
          <div className="user-name-container">
            <span className="profile-image-container">
              {user.profileImage ? (
                <img src={user.profileImage} alt="Profile Image" />
              ) : (
                <User />
              )}
            </span>
            <div>
              <small>{user?.firstName + " " + user?.lastName}</small>
              <small>{user?.email}</small>
            </div>
          </div>

          <p className="user-roles-container">
            {user?.roles?.map((role) => (
              <small className="role">
                {language == "en" ? role.nameEn : role.nameAr}
              </small>
            ))}
          </p>
        </div>
      ))}
    </div>
  );
};
export default RecentRegistrations;
