import { useEffect, useState } from "react";
import { useAuth } from "../../hooks/useAuth";
import { read, update } from "../../api/apiWrapper";
import { useDebounce } from "../../hooks/useDebounce";
import { useLanguage } from "../../hooks/useLanguage";
import SpinnerLoader from "../../components/ui/SpinnerLoader";
import FilterContainer from "../../components/SellerOrders/FilterContainer";
import ViewSwitcher from "../../components/CustomerOrders/ViewSwitcher";
import TableView from "../../components/SellerOrders/TableView";
import CardView from "../../components/SellerOrders/CardView";
import OrderInfoModal from "../../components/SellerOrders/OrderInfoModal";
import ConfirmModal from "../../components/ui/ConfirmModal";
import { toast } from "../../components/ui/Toast";
import { useNavigate } from "react-router-dom";

const SellerOrders = () => {
  const [orders, setOrders] = useState([]);
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(true);
  const [view, setView] = useState("card"); // 'table' or 'card'
  const [isOrderInfoModalOpen, setIsOrderInfoModalOpen] = useState(false);
  const [isUpdateOrderConfirmModalOpen, setIsUpdateOrderConfirmModalOpen] =
    useState(false);

  const [selectedOrder, setSelectedOrder] = useState(null);
  const [selectedOrderStatus, setSelectedStatus] = useState(-1);

  // Filters
  const [searchText, setSearchText] = useState("");
  const [sortingTerm, setSortingTerm] = useState("");
  const [totalOrders, setTotalOrders] = useState(0);
  const [currentPage, setCurrentPage] = useState(1);
  const pageSize = 10; // Items per page
  const { user } = useAuth();
  const userId = user?.id;

  const debouncedSearch = useDebounce(searchText);

  const { translations } = useLanguage();
  const {
    no_orders,
    my_orders,
    confirm,
    confirm_modal_title,
    cant_cancel_not_pending,
    cancel,
    load_failed,
    order_cancelled_successfully,
  } = translations.general.pages.customer_orders;

  const {
    update_confirm_message,
    order_status_update_success,
    order_status_update_failed,
  } = translations.general.pages.seller_orders;

  const navigate = useNavigate();

  const fetchSellerOrders = async () => {
    try {
      setLoading(true);
      const queryParams = new URLSearchParams();
      // Pagination
      queryParams.append("pageNumber", currentPage);
      queryParams.append("pageSize", pageSize);
      queryParams.append("userId", userId);
      // Optional search term
      if (debouncedSearch && debouncedSearch.trim() !== "") {
        queryParams.append("searchTerm", debouncedSearch.trim());
      }
      // Optional sort
      if (sortingTerm && sortingTerm.trim() !== "") {
        queryParams.append("SortBy", sortingTerm.trim());
      }
      // queryParams.append("isFavoritesOnly", false);
      // Final URL
      const url = `seller-orders?${queryParams.toString()}`;
      const data = await read(url);
      console.log("data -> ", data);
      console.log("data ->", data);
      console.log("isSuccess ->", data.isSuccess);
      setOrders(data?.data?.items);
      setTotalOrders(data?.data?.total);
    } catch (err) {
      console.error(err);
      setError(load_failed);
      setOrders([]);
      setTotalOrders(0);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    console.log("debouncedSearch -> ", debouncedSearch);
    console.log("userId -> ", userId);
    console.log("sortingTerm -> ", sortingTerm);
    console.log("currentPage -> ", currentPage);
    console.log("pageSize -> ", pageSize);

    fetchSellerOrders();
  }, [debouncedSearch, userId, sortingTerm, currentPage, pageSize]);

  useEffect(() => {
    setCurrentPage(1);
  }, [sortingTerm]);

  const handleSearchInputChange = (e) => {
    console.log("search -> ", e.target.value);
    setSearchText(e.target.value);
  };

  const handleSortingTermChange = (e) => {
    console.log("Sorting Term -> ", e.target.value);
    setSortingTerm(e.target.value);
  };

  const handlePageChange = (page) => {
    setCurrentPage(page);
  };

  function viewOrder(order) {
    setIsOrderInfoModalOpen(true);
    setSelectedOrder(order);
    console.log("Opened 0-> ", order);
  }

  function handleUpdateOrderStatus(orderStauss) {
    console.log("orderStauss -> ", orderStauss);
    setSelectedStatus(orderStauss);
    setIsUpdateOrderConfirmModalOpen(true);
  }

  async function updateOrder() {
    console.log("updateOrder -> ");

    try {
      const payload = {
        orderId: selectedOrder?.id,
        publicId: selectedOrder?.publicId,
        orderStatus: selectedOrderStatus,
      };

      const result = await update(`seller-orders/update-status`, payload);

      console.log("result -> ", result);

      if (translations.general.server_messages[result?.message?.message]) {
        toast.show(
          translations.general.server_messages[result?.message?.message],
          "success",
        );
      } else {
        toast.show(order_status_update_success, "success");
      }
      closeModal();
      await fetchSellerOrders();
    } catch (error) {
      console.error(error);
      if (translations.general.server_messages[error.message]) {
        toast.show(
          translations.general.server_messages[error.message],
          "error",
        );
      } else {
        toast.show(order_status_update_failed, "error");
      }
    }
  }

  function closeModal() {
    setIsOrderInfoModalOpen(false);
    setIsUpdateOrderConfirmModalOpen(false);
    setSelectedOrder(null);
  }

  return (
    <div>
      <h1>{my_orders}</h1>
      <FilterContainer
        searchText={searchText}
        handleSearchInputChange={handleSearchInputChange}
        sortingTerm={sortingTerm}
        handleSortingTermChange={handleSortingTermChange}
      />
      {loading ? (
        <div className="loader-container">
          <SpinnerLoader />
        </div>
      ) : error ? (
        <div className="error-container">
          <h2>{error}</h2>
        </div>
      ) : !orders || orders?.length <= 0 ? (
        <div className="not-found-container">
          <h2>{no_orders}</h2>
        </div>
      ) : (
        <>
          <ViewSwitcher view={view} setView={setView} />
          {view == "table" && (
            <TableView orders={orders} viewOrder={viewOrder} />
          )}
          {view == "card" && <CardView orders={orders} viewOrder={viewOrder} />}
          <OrderInfoModal
            show={isOrderInfoModalOpen}
            onClose={() => closeModal()}
            onConfirm={handleUpdateOrderStatus}
            order={selectedOrder}
          />
          <ConfirmModal
            show={isUpdateOrderConfirmModalOpen}
            onClose={() => setIsUpdateOrderConfirmModalOpen(false)}
            onConfirm={updateOrder}
            title={update_confirm_message}
            cancelLabel={cancel}
            confirmLabel={confirm}
          />
        </>
      )}
    </div>
  );
};
export default SellerOrders;
