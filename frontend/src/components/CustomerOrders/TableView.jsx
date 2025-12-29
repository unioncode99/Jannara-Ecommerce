import { Eye, Pencil, Trash2 } from "lucide-react";
import Button from "../ui/Button";
import Table from "../ui/Table";
import { formatDateTime, formatMoney } from "../../utils/utils";
import "./TableView.css";
import { useLanguage } from "../../hooks/useLanguage";

const TableView = ({ orders, viewOrder }) => {
  console.log("orders", orders);
  const { language, translations } = useLanguage();
  const {
    order: order_label,
    status,
    actions,
    total,
    placed_at,
  } = translations.general.pages.customer_orders;

  return (
    <div className="table-view">
      <Table
        headers={[`${order_label} #`, placed_at, total, status, actions]}
        data={orders.map((order) => ({
          ID: order.publicOrderId,
          Date: formatDateTime(order.placedAt),
          Total: formatMoney(order.grandTotal),
          Status: language == "en" ? order.statusNameEn : order.statusNameAr,
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
