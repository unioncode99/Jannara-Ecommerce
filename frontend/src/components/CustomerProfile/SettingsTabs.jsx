import "./SettingsTabs.css";

const SettingsTabs = ({ tabs, activeTab, setActiveTab }) => {
  return (
    <div className="setting-tabs-container">
      {tabs?.map((tab) => (
        <button
          className={tab.id == activeTab ? "active" : ""}
          onClick={() => setActiveTab(tab.id)}
        >
          <span>{tab.icon}</span>
          <span>{tab.label}</span>
        </button>
      ))}
    </div>
  );
};
export default SettingsTabs;
