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
  Tags,
  User,
  X,
} from "lucide-react";
import "./Sidebar.css";
import { NavLink, useNavigate } from "react-router-dom";
import { useLanguage } from "../../hooks/useLanguage";
import { useAuth } from "../../hooks/useAuth";
import Button from "./Button";
import { useEffect, useState } from "react";
import Select from "./Select";
import ConfirmModal from "./ConfirmModal";
import { create } from "../../api/apiWrapper";
import { toast } from "./Toast";
import BecomeSellerModal from "../Sidebar.jsx/BecomeSellerModal";

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
    { key: "products", path: "/seller-products", icon: <Package /> },
    // { key: "add_product", path: "/add-product", icon: <Package /> },
    { key: "orders", path: "/seller-orders", icon: <ShoppingBag /> },
  ],
  admin: [
    { key: "dashboard", path: "/admin-dashboard", icon: <LayoutDashboard /> },
    { key: "users", path: "/users", icon: <User /> },
    { key: "categories", path: "/product-categories", icon: <Layers /> },
    { key: "brands", path: "/brands", icon: <Tags /> },
    { key: "products", path: "/products", icon: <Package /> },
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

const Sidebar = ({ isSibebarOpen, onClose }) => {
  const [isBecomeSellerModalOpen, setIsBecomeSellerModalOpen] = useState(false);
  const [
    isBecomeCustomerConfirmModalOpen,
    setIsBecomeCustomerConfirmModalOpen,
  ] = useState(false);
  const { translations, language } = useLanguage();
  const { user, logout, person, setUser } = useAuth();
  const navigate = useNavigate();

  const [currentRole, setCurrentRole] = useState(
    user?.roles?.[0]?.nameEn.toLowerCase() || "unknown_user"
  );

  useEffect(() => {
    setCurrentRole(user?.roles?.[0]?.nameEn.toLowerCase() || "unknown_user");
  }, [user]);

  const {
    become_seller,
    become_customer_confirm,
    become_customer_error,
    become_customer_success,
    become_customer,
    cancel_text,
  } = translations.general.sidebar;

  console.log("currentRole", currentRole);
  console.log("user", user);
  console.log("person", person);
  console.log(
    "user?.roles?.[0]?.nameEn.toLowerCase()",
    user?.roles?.[0]?.nameEn.toLowerCase()
  );

  const roleOptions = user?.roles?.map((role) => ({
    value: role.nameEn.toLowerCase(),
    label: language === "en" ? role.nameEn : role.nameAr,
  }));

  console.log("translations -> ", translations.general.sidebar);
  console.log("person -> ", person);

  let links = menus[currentRole] || menus.unknown_user;

  // user.roles?.length = 1; // for test

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

  function handleProfile() {
    navigate("/customer-profile");
  }

  function handleRoleChange(e) {
    let newRole = e.target.value;
    setCurrentRole(newRole);
    console.log("role", newRole);

    const firstLink = menus[newRole]?.[0] || menus.unknown_user[0];
    if (firstLink) {
      navigate(firstLink.path); // navigate to first link
      onClose(); // optional: close sidebar
    }
  }

  const closeModal = () => {
    setIsBecomeCustomerConfirmModalOpen(false);
    setIsBecomeSellerModalOpen(false);
  };

  async function becomeCustomer() {
    try {
      let result = await create(`sellers/become-customer`);
      setUser((prev) => ({
        ...prev,
        roles: [...(prev.roles || []), result],
      }));
      if (translations.general.server_messages[result?.message?.message]) {
        toast.show(
          translations.general.server_messages[result?.message?.message],
          "success"
        );
      } else {
        toast.show(become_customer_success, "success");
      }
    } catch (error) {
      console.error(error);
      if (translations.general.server_messages[error.message]) {
        toast.show(
          translations.general.server_messages[error.message],
          "error"
        );
      } else {
        toast.show(become_customer_error, "error");
      }
    } finally {
      closeModal();
    }
  }

  return (
    <>
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
              <li key={link.key}>
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
            {/* Profile */}
            <button className="profile-btn" onClick={handleProfile}>
              <span className="profile-avatar">
                <img src={person?.imageUrl} alt="Profile" />
              </span>

              <span className="profile-info">
                <span className="profile-name">{getFullName()}</span>
                <span className="profile-role">
                  {translations.general.sidebar.profile.role[currentRole]}
                </span>
              </span>

              <span className="profile-settings">
                <Settings size={18} />
              </span>
            </button>
            {/* Role Switcher */}
            {user.roles.length > 1 ? (
              <div className="role-switcher">
                <Select
                  options={roleOptions}
                  label={language === "en" ? "Role" : "الدور"}
                  value={currentRole}
                  onChange={handleRoleChange}
                />
              </div>
            ) : currentRole == "customer" ? (
              <Button
                onClick={() => setIsBecomeSellerModalOpen(true)}
                className="btn btn-primary btn-block"
              >
                {become_seller}
              </Button>
            ) : (
              <Button
                onClick={() => setIsBecomeCustomerConfirmModalOpen(true)}
                className="btn btn-primary btn-block"
              >
                {become_customer}
              </Button>
            )}
            {/* Logout  */}
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
      <ConfirmModal
        show={isBecomeCustomerConfirmModalOpen}
        onClose={() => closeModal()}
        onConfirm={() => becomeCustomer()}
        title={become_customer_confirm}
        cancelLabel={cancel_text}
        confirmLabel={become_customer}
      />
      <BecomeSellerModal
        onClose={() => closeModal()}
        show={isBecomeSellerModalOpen}
      />
    </>
  );
};
export default Sidebar;
