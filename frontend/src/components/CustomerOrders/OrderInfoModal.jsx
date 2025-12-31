import { CircleCheckBig } from "lucide-react";
import Button from "../ui/Button";
import Modal from "../ui/Modal";
import "./OrderInfoModal.css";
import { formatMoney } from "../../utils/utils";
import { useLanguage } from "../../hooks/useLanguage";

const OrderInfoModal = ({ show, onClose, onConfirm, order }) => {
  const { translations, language } = useLanguage();
  const { order_details, order_id, seller, cancel_order, cancel, items } =
    translations.general.pages.customer_orders;

  const { pending, processing, shipped, delivered, cancelled } =
    translations.general.order_statuses;
  return (
    <div className="customer-order-info-modal">
      <Modal
        show={show}
        onClose={onClose}
        title={
          <>
            <span>{order_details}</span>
            <small>
              {order_id}: {order?.publicOrderId}
            </small>
          </>
        }
        className="confirm-modal"
      >
        <div className="customer-order-status-container">
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
        </div>
        <div className="sub-orders-container">
          {order?.sellerOrders?.map((sellerOrder) => (
            <div className="sub-order-container">
              {/* {JSON.stringify(sellerOrder)} */}
              <div className="seller-info-container">
                <h4>{seller}:</h4>
                <p>
                  <small>{sellerOrder?.seller?.businessName}</small>
                </p>
                <p>
                  <small>{sellerOrder?.seller?.email}</small>
                </p>
                <p>
                  <small>{sellerOrder?.seller?.phone}</small>
                </p>
              </div>
              <div className="sub-order-items-container">
                <h4>
                  <span>{items}:</span>
                  <span>{formatMoney(sellerOrder?.grandTotal)}</span>
                </h4>
                <ul>
                  {sellerOrder?.sellerOrderItems?.map((sellerOrderItem) => (
                    <li>
                      <span>
                        {language == "en"
                          ? sellerOrderItem.nameEn
                          : sellerOrderItem.nameAr}{" "}
                        ({sellerOrderItem.quantity})
                      </span>
                      <span>{formatMoney(sellerOrderItem.totalPrice)}</span>
                    </li>
                  ))}
                </ul>
              </div>
            </div>
          ))}
        </div>
        <div className="confirm-modal-actions">
          <Button className="btn btn-primary" onClick={onClose}>
            {cancel}
          </Button>
          {order && order?.statusNameEn == "Pending" && (
            <Button className="btn btn-primary" onClick={onConfirm}>
              {cancel_order}
            </Button>
          )}
        </div>
      </Modal>
    </div>
  );
};
export default OrderInfoModal;
