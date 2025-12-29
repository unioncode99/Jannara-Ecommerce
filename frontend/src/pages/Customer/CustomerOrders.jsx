import { useEffect, useState } from "react";
import TableView from "../../components/CustomerOrders/TableView";
import { read } from "../../api/apiWrapper";
import CardView from "../../components/CustomerOrders/CardView";
import "./CustomerOrders.css";
import ViewSwitcher from "../../components/CustomerOrders/ViewSwitcher";
import SpinnerLoader from "../../components/ui/SpinnerLoader";
import { useLanguage } from "../../hooks/useLanguage";

const CustomerOrders = () => {
  const [orders, setOrders] = useState([]);
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(true);
  const [view, setView] = useState("table"); // 'table' or 'card'
  const customerId = 1; // for test
  const { translations } = useLanguage();
  const { no_orders } = translations.general.pages.customer_orders;

  useEffect(() => {
    const fetchCustomerOrders = async () => {
      try {
        setLoading(true);
        const data = await read(`orders?customerId=${customerId}`);
        console.log("data -> ", data);

        setOrders(data?.data);
      } catch (err) {
        console.error(err);
        setError("Failed to load order details.");
      } finally {
        setLoading(false);
      }
    };

    fetchCustomerOrders();
  }, [customerId]);

  function viewOrder() {
    console.log("test");
  }

  if (loading) {
    return (
      <div className="loader-container">
        <SpinnerLoader />
      </div>
    );
  }

  if (error) {
    return (
      <div className="error-container">
        <h1>{error}</h1>
      </div>
    );
  }
  if (!orders || orders?.length <= 0) {
    return (
      <div className="not-found-container">
        <h1>{no_orders}</h1>
      </div>
    );
  }

  return (
    <div>
      <h1>My Orders</h1>
      <ViewSwitcher view={view} setView={setView} />
      {view == "table" && <TableView orders={orders} viewOrder={viewOrder} />}
      {view == "card" && <CardView orders={orders} viewOrder={viewOrder} />}
    </div>
  );
};
export default CustomerOrders;
