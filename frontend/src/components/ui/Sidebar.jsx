import { LogOut, Settings, X } from "lucide-react";
import "./Sidebar.css";
import { NavLink, Link, useNavigate, useLocation } from "react-router-dom";
import { useLanguage } from "../../hooks/useLanguage";
import { useAuth } from "../../hooks/useAuth";

const menus = {
  customer: [
    { key: "dashboard", path: "/customer-dashboard", icon: "🏠" },
    { key: "shop_now", path: "/shop-now", icon: "🏠" },
    { key: "favorites", path: "/favorites", icon: "🏠" },
    { key: "cart", path: "/cart", icon: "🏠" },
    { key: "orders", path: "/orders", icon: "👤" },
  ],
  seller: [
    { key: "dashboard", path: "/seller-dashboard", icon: "🏠" },
    { key: "products", path: "/products", icon: "👤" },
  ],
  admin: [
    { key: "dashboard", path: "/admin-dashboard", icon: "🏠" },
    { key: "users", path: "/users", icon: "👤" },
  ],
  superadmin: [
    { key: "dashboard", path: "/superadmin-dashboard", icon: "🏠" },
    { key: "settings", path: "/settings", icon: "👤" },
  ],
};

const Sidebar = (props) => {
  let { isSibebarOpen, onClose } = props;
  const { translations, language } = useLanguage();
  const { user, logout, person } = useAuth();
  const location = useLocation();
  const navigate = useNavigate();
  console.log("translations -> ", translations.general.sidebar);
  console.log("person -> ", person);

  const handleLogout = async () => {
    await logout();
    navigate("/login");
  };

  const getFullName = () => {
    return person.firstName + " " + person.lastName;
  };

  const role = user?.roles[0]?.nameEn.toLowerCase();

  const links = menus[role] || [];

  return (
    <aside className={`sidebar ${isSibebarOpen ? "" : "close"}`}>
      {/* Top */}
      <div className="sidebar-top">
        <div className="logo">{translations.general.form.login_title}</div>
        <button onClick={onClose}>
          <X />
        </button>
      </div>
      {/* Links */}
      <nav className="sidebar-links">
        <ul>
          {links.map((link) => (
            <li key={link.label}>
              <NavLink
                to={link.path}
                className={({ isActive }) =>
                  `sidebar-link ${isActive ? "active" : ""}`
                }
              >
                <span className="icon">{link.icon}</span>
                <span className="text">
                  {translations.general.sidebar[link.key]}
                </span>
              </NavLink>
            </li>
          ))}
        </ul>
      </nav>
      {/* Bottom */}
      <div className="sidebar-bottom">
        <button className="profile-btn">
          <span className="profile-avatar">
            <img
              src="https://images.unsplash.com/photo-1506794778202-cad84cf45f1d"
              alt="Profile"
            />
          </span>

          <span className="profile-info">
            <span className="profile-name">{getFullName()}</span>
            <span className="profile-role">
              {translations.general.sidebar.profile.role[role]}
            </span>
          </span>

          <span className="profile-settings">
            <Settings size={18} />
          </span>
        </button>

        <button className="logout-btn" onClick={handleLogout}>
          <span>
            <LogOut />
          </span>
          <span className="logo-text">
            {translations.general.sidebar.logout}
          </span>
        </button>
      </div>
    </aside>
  );
};
export default Sidebar;
