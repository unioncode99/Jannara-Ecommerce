import React, { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import "./OrderSuccessPage.css";
import { read } from "../api/apiWrapper";
import { useLanguage } from "../hooks/useLanguage";
import { formatMoney, formatDateTime } from "../utils/utils";
import { ArrowLeft, ArrowRight } from "lucide-react";
import SpinnerLoader from "../components/ui/SpinnerLoader";

const OrderSuccessPage = () => {
  const { publicOrderId } = useParams();
  const [order, setOrder] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const { language, translations } = useLanguage();
  const {
    title,
    subtitle,
    order: orderLabel,
    status,
    paid,
    pending,
    placed_at,
    items,
    subtotal,
    tax,
    shipping,
    grand_total,
    payment_success,
    continue_shopping,
    thank_you,
    no_order,
  } = translations.general.pages.checkout;

  useEffect(() => {
    const fetchOrder = async () => {
      setLoading(true);
      try {
        const data = await read(`orders/${publicOrderId}`);
        console.log("data -> ", data);

        setOrder(data?.data);
      } catch (err) {
        console.error(err);
        setError("Failed to load order details.");
      } finally {
        setLoading(false);
      }
    };

    fetchOrder();
  }, [publicOrderId]);

  if (loading) {
    return (
      <div className="loader-container">
        <SpinnerLoader />
      </div>
    );
  }

  if (error) {
    return (
      <div className="error-container">
        <h1>{error}</h1>
      </div>
    );
  }
  if (!order) {
    return (
      <div className="no-order-container">
        <h1>{no_order}</h1>
      </div>
    );
  }

  return (
    <div className="order-success-page">
      <h1>{payment_success}</h1>
      <p>{thank_you}</p>

      <div className="order-summary">
        <p>
          <span>{orderLabel}</span> <span>#{order.publicOrderId}</span>
        </p>
        <p>
          <span>{status}:</span>{" "}
          <span>{order.orderStatus === 2 ? "Paid" : "Pending"}</span>
        </p>
        <p>
          <span>{placed_at}:</span>
          <span>{formatDateTime(order.placedAt)}</span>
        </p>

        <h3>{items}:</h3>
        <ul>
          {order.orderItems?.map((item) => (
            <li key={item.id}>
              <span>
                {language == "en" ? item.nameEn : item.nameAr} ({item.quantity})
              </span>
              <span>{formatMoney(item.unitPrice)}</span>
            </li>
          ))}
        </ul>

        <div className="totals">
          <p>
            <span>{subtotal}:</span>
            <span>{formatMoney(order.subTotal)}</span>
          </p>
          <p>
            <span>{tax}:</span>
            <span>{formatMoney(order.taxCost)}</span>
          </p>
          <p>
            <span>{shipping}:</span>
            <span>{formatMoney(order.shippingCost)}</span>
          </p>
          <p>
            <strong>{grand_total}: </strong>{" "}
            <span>{formatMoney(order.grandTotal)}</span>
          </p>
        </div>
      </div>

      <div className="btn-continue-container">
        <Link to="/" className="btn btn-primary btn-continue">
          {continue_shopping}
          {language == "en" ? <ArrowRight /> : <ArrowLeft />}
        </Link>
      </div>
    </div>
  );
};

export default OrderSuccessPage;
