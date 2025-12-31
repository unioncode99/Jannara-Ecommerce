import { Clock, DollarSign, ShoppingBag } from "lucide-react";
import DashboardStat from "../../components/CustomerDashboard/DashboardStat";
import WelcomeContainer from "../../components/CustomerDashboard/WelcomeContainer";
import { useEffect, useState } from "react";
import { read } from "../../api/apiWrapper";
import { formatMoney } from "../../utils/utils";
import { useLanguage } from "../../hooks/useLanguage";
import SpinnerLoader from "../../components/ui/SpinnerLoader";
import RecentOrders from "../../components/CustomerDashboard/RecentOrders";
import Wishlist from "../../components/CustomerDashboard/Wishlist";

const CustomerDashboard = () => {
  const [dashboardData, setDashboardData] = useState(null);
  const [loading, setLoading] = useState(false);
  const { translations } = useLanguage();
  const customerId = 1; // for test

  const { total_orders, total_spent, total_pending } =
    translations.general.pages.customer_dashboard;

  const fetchCustomerDashboardData = async () => {
    try {
      setLoading(true);
      const data = await read(`dashboard/customer/${customerId}`);
      console.log("data -> ", data);
      setDashboardData(data);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchCustomerDashboardData();
  }, [customerId]);

  const stats = dashboardData
    ? [
        {
          label: total_orders,
          value: dashboardData.totalOrders,
          icon: <ShoppingBag />,
        },
        {
          label: total_spent,
          value: formatMoney(dashboardData.totalSpent),
          icon: <DollarSign />,
        },
        {
          label: total_pending,
          value: dashboardData.totalPendingOrders,
          icon: <Clock />,
        },
      ]
    : [];

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
      <DashboardStat stats={stats || []} />
      <RecentOrders orders={dashboardData?.latestOrders || []} />
      <Wishlist wishlist={dashboardData?.wishlist || []} />
    </div>
  );
};
export default CustomerDashboard;
