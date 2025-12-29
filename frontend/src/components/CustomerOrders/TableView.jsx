import { Eye, Pencil, Trash2 } from "lucide-react";
import Button from "../ui/Button";
import Table from "../ui/Table";
import { formatDateTime, formatMoney } from "../../utils/utils";
import "./TableView.css";
import { useLanguage } from "../../hooks/useLanguage";

const TableView = ({ orders, viewOrder }) => {
  console.log("orders", orders);
  const { language } = useLanguage();

  return (
    <div className="table-view">
      <Table
        headers={["Order #", "Date", "Total", "Status", "Actions"]}
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
