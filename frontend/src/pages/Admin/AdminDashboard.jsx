import { useEffect, useState } from "react";
import { read } from "../../api/apiWrapper";
import WelcomeContainer from "../../components/AdminDashboard/WelcomeContainer";
import { DollarSign, Shield, Store, Users } from "lucide-react";
import { useLanguage } from "../../hooks/useLanguage";
import { formatMoney } from "../../utils/utils";
import DashboardStat from "../../components/AdminDashboard/DashboardStat";
import SpinnerLoader from "../../components/ui/SpinnerLoader";
import RecentRegistrations from "../../components/AdminDashboard/RecentRegistrations";
import RevenueChart from "../../components/AdminDashboard/RevenueChart";
import "./AdminDashboard.css";

function AdminDashboard() {
  const [dashboardData, setDashboardData] = useState(null);
  const [loading, setLoading] = useState(false);

  const { translations } = useLanguage();

  const {
    total_revenue,
    active_sellers,
    total_customers,
    pending_verifications,
  } = translations.general.pages.admin_dashboard;

  const fetchCustomerDashboardData = async () => {
    try {
      setLoading(true);
      const data = await read(`dashboard/admin`);
      console.log("data -> ", data);
      setDashboardData(data);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const stats = dashboardData
    ? [
        {
          label: total_revenue,
          value: formatMoney(dashboardData.totalRevenue),
          icon: <DollarSign />,
        },
        {
          label: active_sellers,
          value: dashboardData.activeSellers,
          icon: <Store />,
        },
        {
          label: total_customers,
          value: dashboardData.totalCustomers,
          icon: <Users />,
        },
        {
          label: pending_verifications,
          value: dashboardData.pendingVerifications,
          icon: <Shield />,
        },
      ]
    : [];

  useEffect(() => {
    fetchCustomerDashboardData();
  }, []);

  if (loading) {
    return (
      <div className="loader-container">
        <SpinnerLoader />
      </div>
    );
  }

  return (
    <div>
      <WelcomeContainer />
      <DashboardStat stats={stats} />
      <div className="chart-container">
        <RevenueChart data={dashboardData?.monthlyRevenue} />
        <RecentRegistrations
          lastRegisteredUsers={dashboardData?.lastRegisteredUsers}
        />
      </div>
    </div>
  );
}
export default AdminDashboard;
