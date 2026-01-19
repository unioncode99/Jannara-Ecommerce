import { Eye } from "lucide-react";
import { useLanguage } from "../../hooks/useLanguage";
import { formatDateTime, formatMoney } from "../../utils/utils";
import Button from "../ui/Button";
import "./SellerOrderCard.css";

const SellerOrderCard = ({ order, viewOrder }) => {
  const { language, translations } = useLanguage();
  const {
    order: order_label,
    status,
    items,
    total,
    placed_at,
  } = translations.general.pages.customer_orders;

  return (
    <div className="customer-order-card">
      <p>
        <span>{order_label} #</span>
        <span>{order.publicId}</span>
      </p>
      <p>
        <span>{status}:</span>
        <span className="order-status">
          {language == "en" ? order.statusNameEn : order.statusNameAr}
        </span>
      </p>
      <p>
        <span>{total}:</span>
        <span>{formatMoney(order.grandTotal)}</span>
      </p>
      <p>
        <span>{placed_at}:</span>
        <span>{formatDateTime(order.createdAt)}</span>
      </p>
      {order.sellerOrderItems && (
        <>
          <h4>{items}:</h4>
          <ul>
            {order?.sellerOrderItems?.map((item) => (
              <li key={item.Id}>
                <span>
                  {language == "en" ? item.productNameEn : item.productNameAr} (
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
export default SellerOrderCard;
