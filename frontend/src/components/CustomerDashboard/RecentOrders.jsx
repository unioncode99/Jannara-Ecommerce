import { Link } from "react-router-dom";
import "./RecentOrders.css";
import { useLanguage } from "../../hooks/useLanguage";
import { formatMoney } from "../../utils/utils";

const RecentOrders = ({ orders }) => {
  const { translations } = useLanguage();
  const { recent_orders, view_all_orders, items_count } =
    translations.general.pages.customer_dashboard;

  function getDateFormat(date) {
    const yyyy = date.getFullYear();
    const mm = String(date.getMonth() + 1).padStart(2, "0");
    const dd = String(date.getDate()).padStart(2, "0");
    return `${yyyy}-${mm}-${dd}`;
  }

  return (
    <div className="customer-recent-orders-container">
      <h3>
        <span>{recent_orders}</span>
        <Link to="/customer-orders" className="view-all-orders-link">
          <small>{view_all_orders}</small>
        </Link>
      </h3>
      {orders?.map((order) => (
        <div className="order-item">
          <div>
            <p>
              <strong>{order.publicOrderId}</strong>
            </p>
            <p>
              <small>{getDateFormat(new Date(order.placedAt))}</small> .{" "}
              <small>
                {order.itemsCount} {items_count}
              </small>
            </p>
          </div>
          <div>
            <strong>{formatMoney(order.grandTotal)}</strong>
          </div>
        </div>
      ))}
    </div>
  );
};
export default RecentOrders;
