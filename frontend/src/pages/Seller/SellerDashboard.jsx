import { useEffect, useState } from "react";
import { useLanguage } from "../../hooks/useLanguage";
import { read } from "../../api/apiWrapper";
import { formatDateTime, formatMoney } from "../../utils/utils";
import { DollarSign, Package, ShoppingBag } from "lucide-react";
import SpinnerLoader from "../../components/ui/SpinnerLoader";
import WelcomeContainer from "../../components/SellerDashboard/WelcomeContainer";
import RevenueChart from "../../components/SellerDashboard/RevenueChart";
import DashboardStat from "../../components/SellerDashboard/DashboardStat";
import RecentOrders from "../../components/SellerDashboard/RecentOrders";

// for test
const mockSellerDashboardData = {
  totalSales: 12450.75,
  totalOrders: 154,
  activeProducts: 23,
  recentOrders: [
    {
      orderId: 101,
      customerName: "Alice Johnson",
      totalAmount: 259.98,
      orderDate: "2024-03-10T10:30:00",
      statusNameEn: "Delivered",
      statusNameAr: "تم التوصيل",
    },
    {
      orderId: 102,
      customerName: "Bob Smith",
      totalAmount: 89.99,
      orderDate: "2024-03-11T14:20:00",
      statusNameEn: "Processing",
      statusNameAr: "قيد المعالجة",
    },
    {
      orderId: 103,
      customerName: "Charlie Brown",
      totalAmount: 450.0,
      orderDate: "2024-03-12T09:15:00",
      statusNameEn: "Shipped",
      statusNameAr: "تم الشحن",
    },
  ],
  monthlyRevenue: [
    { year: 2024, month: 1, revenue: 1200 },
    { year: 2024, month: 2, revenue: 1800 },
    { year: 2024, month: 3, revenue: 1500 },
    { year: 2024, month: 4, revenue: 2000 },
    { year: 2024, month: 5, revenue: 2200 },
    { year: 2024, month: 6, revenue: 2500 },
    { year: 2024, month: 7, revenue: 2700 },
    { year: 2024, month: 8, revenue: 3000 },
    { year: 2024, month: 9, revenue: 3200 },
    { year: 2024, month: 10, revenue: 3400 },
    { year: 2024, month: 11, revenue: 3600 },
    { year: 2024, month: 12, revenue: 4000 },
  ],
};

function SellerDashboard() {
  const [dashboardData, setDashboardData] = useState(null);
  const [loading, setLoading] = useState(false);
  const { translations } = useLanguage();

  const {
    total_sales,
    total_orders,
    active_products,
    recent_orders,
    view_all_sales_overview,
  } = translations.general.pages.seller_dashboard;

  const fetchSellerDashboardData = async () => {
    try {
      setLoading(true);
      const data = await read(`dashboard/seller`);
      console.log("data -> ", data);
      setDashboardData(data);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  // useEffect(() => {
  //   fetchSellerDashboardData();
  // }, []);

  // for test

  useEffect(() => {
    // simulate loading
    setTimeout(() => {
      setDashboardData(mockSellerDashboardData);
      setLoading(false);
    }, 500);
  }, []);

  const stats = dashboardData
    ? [
        {
          label: total_sales,
          value: formatMoney(dashboardData.totalSales),
          icon: <DollarSign />,
        },
        {
          label: total_orders,
          value: dashboardData.totalOrders,
          icon: <ShoppingBag />,
        },
        {
          label: active_products,
          value: dashboardData.activeProducts,
          icon: <Package />,
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
      <DashboardStat stats={stats} />
      <RevenueChart data={dashboardData?.monthlyRevenue} />
      <RecentOrders recentOrders={dashboardData?.recentOrders} />
    </div>
  );
}
export default SellerDashboard;
