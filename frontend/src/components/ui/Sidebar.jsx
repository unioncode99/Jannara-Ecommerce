import { LogOut, Settings, X } from "lucide-react";
import "./Sidebar.css";
import { NavLink } from "react-router-dom";

const Sidebar = (props) => {
  //   const { logo, links, onLogout, collapsed, setCollapsed } = props;
  let { logo, links, onLogout, isOpen, onClose } = props;

  links = [
    { label: "Dashboard", href: "/", icon: "🏠" },
    { label: "Profile", href: "/profile", icon: "👤" },
    { label: "Settings", href: "/settings", icon: "⚙️" },
  ];

  logo = "Jannara";

  return (
    <aside className={`sidebar ${isOpen ? "" : "close"}`}>
      {/* Top */}
      <div className="sidebar-top">
        <div className="logo">
          {typeof logo !== "string" ? <img src={logo} alt="logo" /> : logo}
        </div>

        <button onClick={onClose}>
          <X />
        </button>
      </div>
      {/* Links */}
      <nav className="sidebar-links">
        <ul>
          {links.map((link) => (
            <li key={link.label}>
              <a href={link.href} className="active">
                <span className="icon">{link.icon}</span>
                <span className="text">{link.label}</span>
              </a>
              {/* <NavLink
                to={link.href}
                className={({ isActive }) =>
                  `sidebar-link ${isActive ? "active" : ""}`
                }
              >
                <span className="icon">{link.icon}</span>
                <span className="text">{link.label}</span>
              </NavLink> */}
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
            <span className="profile-name">John Doe</span>
            <span className="profile-role">Customer</span>
          </span>

          <span className="profile-settings">
            <Settings size={18} />
          </span>
        </button>

        <button className="logout-btn" onClick={onLogout}>
          <span>
            <LogOut />
          </span>
          <span className="logo-text">Logout</span>
        </button>
      </div>
    </aside>
  );
};
export default Sidebar;
