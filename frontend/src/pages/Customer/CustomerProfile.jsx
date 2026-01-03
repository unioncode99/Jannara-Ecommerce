import { useState } from "react";
import SettingsTabs from "../../components/CustomerProfile/SettingsTabs";
import ProfileTab from "../../components/CustomerProfile/ProfileTab";
import SecurityTab from "../../components/CustomerProfile/SecurityTab";
import NotificationsTab from "../../components/CustomerProfile/NotificationsTab";
import "./CustomerProfile.css";
import { Bell, Shield, User } from "lucide-react";
import { useLanguage } from "../../hooks/useLanguage";

const CustomerProfile = () => {
  const [activeTab, setActiveTab] = useState("profile");
  const { translations } = useLanguage();
  const { settings_title, profile, security, notifications } =
    translations.general.pages.customer_profile;

  const tabs = [
    { id: "profile", label: profile, icon: <User /> },
    { id: "notifications", label: notifications, icon: <Bell /> },
    { id: "security", label: security, icon: <Shield /> },
  ];

  return (
    <div>
      <h1>{settings_title}</h1>
      <div className="customer-settings-container">
        <SettingsTabs
          tabs={tabs}
          activeTab={activeTab}
          setActiveTab={setActiveTab}
        />
        <div className="tabs-container">
          {activeTab == "profile" && <ProfileTab />}
          {activeTab == "security" && <SecurityTab />}
          {activeTab == "notifications" && <NotificationsTab />}
        </div>
      </div>
    </div>
  );
};
export default CustomerProfile;
