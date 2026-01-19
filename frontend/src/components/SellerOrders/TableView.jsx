import { Eye } from "lucide-react";
import { useLanguage } from "../../hooks/useLanguage";
import { formatDateTime, formatMoney } from "../../utils/utils";
import Button from "../ui/Button";
import "./TableView.css";
import Table from "../ui/Table";

const TableView = ({ orders, viewOrder }) => {
  console.log("orders", orders);
  const { translations, language } = useLanguage();
  const {
    order: order_label,
    actions,
    total,
    status,
    placed_at,
  } = translations.general.pages.customer_orders;

  return (
    <div className="table-view">
      <Table
        headers={[order_label, placed_at, total, status, actions]}
        data={orders.map((order) => ({
          ID: order.publicId,
          Date: formatDateTime(order.createdAt),
          Total: formatMoney(order.grandTotal),
          Status: (
            <span className="order-status">
              {language == "en" ? order.statusNameEn : order.statusNameAr}
            </span>
          ),
          Actions: (
            <>
              <Button
                className="view-order-btn"
                onClick={() => viewOrder(order)}
              >
                <Eye />
              </Button>
            </>
          ),
        }))}
      />
    </div>
  );
};
export default TableView;
