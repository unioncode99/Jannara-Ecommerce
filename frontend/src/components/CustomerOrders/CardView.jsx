import CustomerOrderCard from "./CustomerOrderCard";
import "./CardView.css";

const CardView = ({ orders, onEdit }) => {
  return (
    <div className="card-view">
      {orders.map((order) => (
        <CustomerOrderCard order={order} />
      ))}
    </div>
  );
};
export default CardView;
