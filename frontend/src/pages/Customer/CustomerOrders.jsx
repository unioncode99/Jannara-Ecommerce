import { useEffect, useState } from "react";
import TableView from "../../components/CustomerOrders/TableView";
import { read } from "../../api/apiWrapper";

const CustomerOrders = () => {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [view, setView] = useState("table"); // 'table' or 'card'
  const customerId = 1; // for test

  useEffect(() => {
    const fetchCustomerOrders = async () => {
      try {
        setLoading(true);
        const data = await read(`orders?customerId=${customerId}`);
        console.log("data -> ", data);

        setOrders(data?.data);
      } catch (err) {
        console.error(err);
        // setError("Failed to load order details.");
      } finally {
        setLoading(false);
      }
    };

    fetchCustomerOrders();
  }, [customerId]);

  if (loading) return <p>Loading orders...</p>;
  if (orders.length === 0) return <p>No orders found.</p>;

  return (
    <div>
      <h1>My Orders</h1>
      {/* View Switch */}
      <div></div>
      <TableView orders={orders} />
    </div>
  );
};
export default CustomerOrders;
