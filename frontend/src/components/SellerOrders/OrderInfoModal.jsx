import { CircleCheckBig } from "lucide-react";
import { useLanguage } from "../../hooks/useLanguage";
import Modal from "../ui/Modal";
import { formatMoney, formatSudanPhoneLocal } from "../../utils/utils";
import Button from "../ui/Button";
import "./OrderInfoModal.css";
import { useEffect, useState } from "react";

const ORDER_STATUS = {
  PENDING: 1,
  PROCESSING: 2,
  SHIPPED: 3,
  DELIVERED: 4,
  CANCELLED: 5,
};

const STATUS_LIST = [
  { id: ORDER_STATUS.PENDING, key: "pending", apiText: "Pending" },
  { id: ORDER_STATUS.PROCESSING, key: "processing", apiText: "Processing" },
  { id: ORDER_STATUS.SHIPPED, key: "shipped", apiText: "Shipped" },
  { id: ORDER_STATUS.DELIVERED, key: "delivered", apiText: "Delivered" },
  { id: ORDER_STATUS.CANCELLED, key: "cancelled", apiText: "Cancelled" },
];

const OrderInfoModal = ({ show, onClose, onConfirm, order }) => {
  const [selectedStatusId, setSelectedStatusId] = useState(null);

  const { translations, language } = useLanguage();

  const {
    order_details,
    order_id,
    customer,
    update_order,
    cancel,
    items,
    name,
    email,
    phone,
  } = translations.general.pages.seller_orders;

  // const { pending, processing, shipped, delivered, cancelled } =
  //   translations.general.order_statuses;
  const orderStatuses = translations.general.order_statuses;

  // Set selected status when modal opens
  useEffect(() => {
    if (order?.orderStatus !== undefined) {
      setSelectedStatusId(order.orderStatus);
    }
  }, [order]);

  const handleConfirm = () => {
    if (selectedStatusId === order?.orderStatus) return;

    const selectedStatusObj = STATUS_LIST.find(
      (s) => s.id === selectedStatusId,
    );

    console.log("Status ID:", selectedStatusId);
    console.log("Status API Text:", selectedStatusObj?.apiText);
    console.log("Status UI Text:", orderStatuses[selectedStatusObj?.key]);

    onConfirm(selectedStatusId);
  };

  return (
    <div className="seller-order-info-modal">
      <Modal
        show={show}
        onClose={onClose}
        title={
          <>
            <span>{order_details}</span>
            <small>
              {order_id}: {order?.publicId}
            </small>
          </>
        }
        className="confirm-modal"
      >
        <div className="seller-order-status-container">
          {STATUS_LIST.map((status) => {
            const isActive = selectedStatusId === status.id;

            return (
              <span
                key={status.id}
                className={isActive ? "active" : ""}
                onClick={() => setSelectedStatusId(status.id)}
                style={{ cursor: "pointer" }}
              >
                {isActive && <CircleCheckBig />}
                <small>{orderStatuses[status.key]}</small>
              </span>
            );
          })}
        </div>

        {/* <div className="seller-order-status-container">
          <span className={order?.statusNameEn == "Pending" ? "active" : ""}>
            {order?.statusNameEn == "Pending" && <CircleCheckBig />}
            <small>{pending}</small>
          </span>
          <span className={order?.statusNameEn == "Processing" ? "active" : ""}>
            {order?.statusNameEn == "Processing" && <CircleCheckBig />}
            <small>{processing}</small>
          </span>
          <span className={order?.statusNameEn == "Shipped" ? "active" : ""}>
            {order?.statusNameEn == "Shipped" && <CircleCheckBig />}
            <small>{shipped}</small>
          </span>
          <span className={order?.statusNameEn == "Delivered" ? "active" : ""}>
            {order?.statusNameEn == "Delivered" && <CircleCheckBig />}
            <small>{delivered}</small>
          </span>
          <span className={order?.statusNameEn == "Cancelled" ? "active" : ""}>
            {order?.statusNameEn == "Cancelled" && <CircleCheckBig />}
            <small>{cancelled}</small>
          </span>
        </div> */}

        {/* Customer */}
        <div className="customer-info-container">
          <h4>{customer}:</h4>
          <p>
            <small>
              {name}:{" "}
              {order?.customer?.firstName + " " + order?.customer?.lastName}
            </small>
          </p>
          <p>
            <small>
              {email}: {order?.customer?.email}
            </small>
          </p>
          <p>
            <small>
              {phone}: {formatSudanPhoneLocal(order?.customer?.phone)}
            </small>
          </p>
        </div>
        {/* Order Items */}
        <div className="seller-order-items-container">
          <h4>
            <span>{items}:</span>
            <span>{formatMoney(order?.grandTotal)}</span>
          </h4>
          <ul>
            {order?.sellerOrderItems?.map((sellerOrderItem) => (
              <li>
                <span>
                  {language == "en"
                    ? sellerOrderItem.productNameEn
                    : sellerOrderItem.productNameAr}{" "}
                  ({sellerOrderItem.quantity})
                </span>
                <span>{formatMoney(sellerOrderItem.totalPrice)}</span>
              </li>
            ))}
          </ul>
        </div>
        {/* Actions */}
        <div className="confirm-modal-actions">
          <Button className="btn btn-primary" onClick={onClose}>
            {cancel}
          </Button>
          <Button
            className="btn btn-primary"
            disabled={
              selectedStatusId === ORDER_STATUS.CANCELLED ||
              selectedStatusId === order?.orderStatus ||
              selectedStatusId < order?.orderStatus
            }
            onClick={handleConfirm}
          >
            {update_order}
          </Button>
        </div>
      </Modal>
    </div>
  );
};
export default OrderInfoModal;
