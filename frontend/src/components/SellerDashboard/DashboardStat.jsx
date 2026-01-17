const DashboardStat = ({ stats }) => {
  return (
    <div className="stats-container">
      {stats?.map((stat) => (
        <div className="stat-item">
          <div className="stat-item-icon-container">{stat.icon}</div>
          <div>
            <p className="stat-item-label">{stat.label}</p>
            <p className="stat-item-value">
              <strong>{stat.value}</strong>
            </p>
          </div>
        </div>
      ))}
    </div>
  );
};
export default DashboardStat;
