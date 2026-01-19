import "./CardView.css";
import SellerOrderCard from "./SellerOrderCard";

const CardView = ({ orders, viewOrder }) => {
  return (
    <div className="card-view">
      {orders.map((order) => (
        <SellerOrderCard key={order.id} order={order} viewOrder={viewOrder} />
      ))}
    </div>
  );
};
export default CardView;
