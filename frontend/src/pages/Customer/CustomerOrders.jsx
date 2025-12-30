import { useEffect, useState } from "react";
import TableView from "../../components/CustomerOrders/TableView";
import { read } from "../../api/apiWrapper";
import CardView from "../../components/CustomerOrders/CardView";
import "./CustomerOrders.css";
import ViewSwitcher from "../../components/CustomerOrders/ViewSwitcher";
import SpinnerLoader from "../../components/ui/SpinnerLoader";
import { useLanguage } from "../../hooks/useLanguage";
import OrderInfoModal from "../../components/CustomerOrders/OrderInfoModal";

const CustomerOrders = () => {
  const [orders, setOrders] = useState([]);
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(true);
  const [view, setView] = useState("table"); // 'table' or 'card'
  const [isOrderInfoModalOpen, setIsOrderInfoModalOpen] = useState(false);
  const [selectedOrder, setSelectedOrder] = useState(null);
  const customerId = 1; // for test
  const { translations } = useLanguage();

  const { no_orders, my_orders } = translations.general.pages.customer_orders;

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

  function viewOrder(order) {
    setIsOrderInfoModalOpen(true);
    setSelectedOrder(order);
    console.log("Opened 0-> ", order);
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
      <h1>{my_orders}</h1>
      <ViewSwitcher view={view} setView={setView} />
      {view == "table" && <TableView orders={orders} viewOrder={viewOrder} />}
      {view == "card" && <CardView orders={orders} viewOrder={viewOrder} />}
      <OrderInfoModal
        show={isOrderInfoModalOpen}
        onClose={() => setIsOrderInfoModalOpen(false)}
        onConfirm={() => console.log("Hello")}
        order={selectedOrder}
      />
    </div>
  );
};
export default CustomerOrders;
