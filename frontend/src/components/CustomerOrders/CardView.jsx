import CustomerOrderCard from "./CustomerOrderCard";
import "./CardView.css";

const CardView = ({ orders, viewOrder }) => {
  return (
    <div className="card-view">
      {orders.map((order) => (
        <CustomerOrderCard key={order.id} order={order} viewOrder={viewOrder} />
      ))}
    </div>
  );
};
export default CardView;
