import { Pencil, Trash2 } from "lucide-react";
import Button from "../ui/Button";
import Table from "../ui/Table";
import { formatDateTime, formatMoney } from "../../utils/utils";
import "./TableView.css";

const TableView = ({ orders }) => {
  console.log("orders", orders);
  const handleEdit = (order) => {};
  function setDeleteId(id) {}
  function setIsConfirmOpen(id) {}
  //   return <div>TableView</div>;
  return (
    <div className="table-view">
      <Table
        headers={["Order #", "Date", "Total", "Status", "Actions"]}
        data={orders.map((order) => ({
          ID: order.publicOrderId,
          Date: formatDateTime(order.placedAt),
          Total: formatMoney(order.grandTotal),
          Status: order.orderStatus,
          Actions: (
            <>
              <Button
                className="edit-order-btn"
                onClick={() => handleEdit(order)}
              >
                <Pencil />
              </Button>
              <Button
                className="delete-order-btn"
                onClick={() => {
                  setDeleteId(order.id);
                  setIsConfirmOpen(true);
                }}
              >
                <Trash2 />
              </Button>
            </>
          ),
        }))}
      />
    </div>
  );
};
export default TableView;
