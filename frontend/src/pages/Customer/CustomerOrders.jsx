import { useEffect, useState } from "react";
import TableView from "../../components/CustomerOrders/TableView";
import { patch, read, update } from "../../api/apiWrapper";
import CardView from "../../components/CustomerOrders/CardView";
import "./CustomerOrders.css";
import ViewSwitcher from "../../components/CustomerOrders/ViewSwitcher";
import SpinnerLoader from "../../components/ui/SpinnerLoader";
import { useLanguage } from "../../hooks/useLanguage";
import OrderInfoModal from "../../components/CustomerOrders/OrderInfoModal";
import ConfirmModal from "../../components/ui/ConfirmModal";
import { toast } from "../../components/ui/Toast";

const CustomerOrders = () => {
  const [orders, setOrders] = useState([]);
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(true);
  const [view, setView] = useState("table"); // 'table' or 'card'
  const [isOrderInfoModalOpen, setIsOrderInfoModalOpen] = useState(false);
  const [isCancelOrderConfirmModalOpen, setIsCancelOrderConfirmModalOpen] =
    useState(false);
  const [selectedOrder, setSelectedOrder] = useState(null);
  const customerId = 1; // for test
  const { translations } = useLanguage();

  const {
    no_orders,
    my_orders,
    confirm,
    confirm_modal_title,
    cant_cancel_not_pending,
    cancel,
    load_failed,
    order_cancelled_successfully,
  } = translations.general.pages.customer_orders;

  const fetchCustomerOrders = async () => {
    try {
      setLoading(true);
      const data = await read(`orders?customerId=${customerId}`);
      console.log("data -> ", data);
      setOrders(data?.data);
    } catch (err) {
      console.error(err);
      setError(load_failed);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchCustomerOrders();
  }, [customerId]);

  function viewOrder(order) {
    setIsOrderInfoModalOpen(true);
    setSelectedOrder(order);
    console.log("Opened 0-> ", order);
  }

  function handleCancelOrder() {
    setIsCancelOrderConfirmModalOpen(true);
  }

  async function cancelOrder() {
    if (selectedOrder && selectedOrder?.statusNameEn != "Pending") {
      toast.show(cant_cancel_not_pending, "error");
      setIsCancelOrderConfirmModalOpen(false);
      return;
    }

    try {
      // setLoading(true);
      await patch(`orders/cancel`, {
        publicId: selectedOrder.publicOrderId,
        id: null,
      });
      toast.show(order_cancelled_successfully, "success");
      setIsCancelOrderConfirmModalOpen(false);
      setIsOrderInfoModalOpen(false);
      await fetchCustomerOrders();
    } catch (err) {
      toast.show(cant_cancel_not_pending, "error");
      setIsCancelOrderConfirmModalOpen(false);
      console.error(err);
    } finally {
      // setLoading(false);
    }
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
        onConfirm={() => handleCancelOrder()}
        order={selectedOrder}
      />
      <ConfirmModal
        show={isCancelOrderConfirmModalOpen}
        onClose={() => setIsCancelOrderConfirmModalOpen(false)}
        onConfirm={() => cancelOrder()}
        title={confirm_modal_title}
        cancelLabel={cancel}
        confirmLabel={confirm}
      />
    </div>
  );
};
export default CustomerOrders;
