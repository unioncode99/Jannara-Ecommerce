import {
  Heart,
  House,
  Layers,
  LayoutDashboard,
  LogOut,
  Package,
  Settings,
  ShoppingBag,
  ShoppingCart,
  User,
  X,
} from "lucide-react";
import "./Sidebar.css";
import { NavLink, useNavigate } from "react-router-dom";
import { useLanguage } from "../../hooks/useLanguage";
import { useAuth } from "../../hooks/useAuth";
import Button from "./Button";

const menus = {
  unknown_user: [
    { key: "shop_now", path: "/", icon: <House /> },
    { key: "cart", path: "/cart", icon: <ShoppingCart /> },
    { key: "favorites", path: "/favorites", icon: <Heart /> },
  ],
  customer: [
    {
      key: "dashboard",
      path: "/customer-dashboard",
      icon: <LayoutDashboard />,
    },
    { key: "shop_now", path: "/", icon: <House /> },
    { key: "favorites", path: "/favorites", icon: <Heart /> },
    { key: "cart", path: "/cart", icon: <ShoppingCart /> },
    { key: "orders", path: "/customer-orders", icon: <ShoppingBag /> },
  ],
  seller: [
    { key: "dashboard", path: "/seller-dashboard", icon: <LayoutDashboard /> },
    { key: "products", path: "/products", icon: <Package /> },
    { key: "add_product", path: "/add-product", icon: <Package /> },
    { key: "orders", path: "/orders", icon: <ShoppingBag /> },
  ],
  admin: [
    { key: "dashboard", path: "/admin-dashboard", icon: <LayoutDashboard /> },
    { key: "users", path: "/users", icon: <User /> },
    { key: "categories", path: "/categories", icon: <Layers /> },
  ],
  superadmin: [
    {
      key: "dashboard",
      path: "/superadmin-dashboard",
      icon: <LayoutDashboard />,
    },
    { key: "users", path: "/users", icon: <User /> },
    { key: "categories", path: "/categories", icon: <Layers /> },
  ],
};

const Sidebar = (props) => {
  let { isSibebarOpen, onClose } = props;
  const { translations } = useLanguage();
  const { user, logout, person } = useAuth();
  const navigate = useNavigate();
  console.log("translations -> ", translations.general.sidebar);
  console.log("person -> ", person);

  const handleLogout = () => {
    logout();
    onClose();
    navigate("/login");
  };
  const handleLogin = () => {
    onClose();
    navigate("/login");
  };
  const getFullName = () => {
    return person?.firstName + " " + person?.lastName;
  };

  const role = user?.roles[0]?.nameEn.toLowerCase();

  let links = [];
  if (role) {
    links = menus[role] || [];
  } else {
    links = menus.unknown_user || [];
  }

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
                onClick={onClose}
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
      {user ? (
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
      ) : (
        <div className="sidebar-bottom">
          <Button className="btn btn-primary btn-block" onClick={handleLogin}>
            {translations.general.form.login_button}
          </Button>
        </div>
      )}
    </aside>
  );
};
export default Sidebar;
