import { Eye } from "lucide-react";
import { useLanguage } from "../../hooks/useLanguage";
import { formatDateTime, formatMoney } from "../../utils/utils";
import Button from "../ui/Button";
import "./CustomerOrderCard.css";

const CustomerOrderCard = ({ order, viewOrder }) => {
  const { language } = useLanguage();
  return (
    <div className="customer-order-card">
      <p>
        <span>Order #</span>
        <span>{order.publicOrderId}</span>
      </p>
      <p>
        <span>Status:</span>
        <span className="order-status">
          {language == "en" ? order.statusNameEn : order.statusNameAr}
        </span>
      </p>
      <p>
        <span>Total:</span>
        <span>{formatMoney(order.grandTotal)}</span>
      </p>
      <p>
        <span>Placed At:</span>
        <span>{formatDateTime(order.placedAt)}</span>
      </p>
      {order.orderItems && (
        <>
          <h4>Items:</h4>
          <ul>
            {order?.orderItems?.map((item) => (
              <li key={item.Id}>
                <span>
                  {language == "en" ? item.nameEn : item.nameAr} (
                  {item.quantity})
                </span>
                <span>{formatMoney(item.totalPrice)}</span>
              </li>
            ))}
          </ul>
        </>
      )}
      <Button className="view-order-btn" onClick={() => viewOrder(order)}>
        <Eye />
      </Button>
    </div>
  );
};
export default CustomerOrderCard;
