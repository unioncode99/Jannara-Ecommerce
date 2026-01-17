import "./RecentOrders.css";
import { useLanguage } from "../../hooks/useLanguage";
import { Link } from "react-router-dom";
import { ShoppingBag } from "lucide-react";
import { formatDateTime, formatMoney } from "../../utils/utils";

const RecentOrders = ({ recentOrders }) => {
  const { translations, language } = useLanguage();
  const { recent_orders, view_all } =
    translations.general.pages.seller_dashboard;

  function getDateFormat(date) {
    const yyyy = date.getFullYear();
    const mm = String(date.getMonth() + 1).padStart(2, "0");
    const dd = String(date.getDate()).padStart(2, "0");
    return `${yyyy}-${mm}-${dd}`;
  }

  return (
    <div className="seller-recent-orders-container">
      <h3>
        <span>{recent_orders}</span>
        <Link to="/seller-orders" className="view-all-orders-link">
          <small>{view_all}</small>
        </Link>
      </h3>
      {recentOrders?.map((order) => (
        <div className="recent-seller-order-item">
          <div>
            <span>
              <ShoppingBag />
            </span>
          </div>
          <div>
            <small>{order.customerName}</small>
            <small>{formatDateTime(order.orderDate)}</small>
          </div>
          <div>
            <strong>{formatMoney(order.totalAmount)}</strong>
            <small>
              {language == "en" ? order.statusNameEn : order.statusNameAr}
            </small>
          </div>
        </div>
      ))}
    </div>
  );
};
export default RecentOrders;
