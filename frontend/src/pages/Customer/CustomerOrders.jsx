import { useEffect, useState } from "react";
import TableView from "../../components/CustomerOrders/TableView";
import { patch, read, update } from "../../api/apiWrapper";
import CardView from "../../components/CustomerOrders/CardView";
import "./CustomerOrders.css";
import ViewSwitcher from "../../components/CustomerOrders/ViewSwitcher";
import SpinnerLoader from "../../components/ui/SpinnerLoader";
import { useLanguage } from "../../hooks/useLanguage";
import OrderInfoModal from "../../components/CustomerOrders/OrderInfoModal";
import ConfirmModal from "../../components/ui/ConfirmModal";
import { toast } from "../../components/ui/Toast";
import Input from "../../components/ui/Input";
import FilterContainer from "../../components/CustomerOrders/FilterContainer";
import Pagination from "../../components/ui/Pagination";
import { useAuth } from "../../hooks/useAuth";

const CustomerOrders = () => {
  const [orders, setOrders] = useState([]);
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(true);
  const [view, setView] = useState("table"); // 'table' or 'card'
  const [isOrderInfoModalOpen, setIsOrderInfoModalOpen] = useState(false);
  const [isCancelOrderConfirmModalOpen, setIsCancelOrderConfirmModalOpen] =
    useState(false);
  const [selectedOrder, setSelectedOrder] = useState(null);
  const { translations } = useLanguage();

  const [searchText, setSearchText] = useState("");
  const [debouncedSearch, setDebouncedSearch] = useState("");
  const [sortingTerm, setSortingTerm] = useState("");
  const [totalOrders, setTotalOrders] = useState(0);
  const [currentPage, setCurrentPage] = useState(1);
  const pageSize = 10; // Items per page
  const { user } = useAuth();
  const userId = user?.id;

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

  const fetchCustomerOrders = async () => {
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
      const url = `orders?${queryParams.toString()}`;
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

    fetchCustomerOrders();
  }, [debouncedSearch, userId, sortingTerm, currentPage, pageSize]);

  useEffect(() => {
    const timer = setTimeout(() => {
      setDebouncedSearch(searchText);
    }, 500);

    return () => clearTimeout(timer);
  }, [searchText]);

  useEffect(() => {
    setCurrentPage(1);
  }, [sortingTerm]);

  function viewOrder(order) {
    setIsOrderInfoModalOpen(true);
    setSelectedOrder(order);
    console.log("Opened 0-> ", order);
  }

  function handleCancelOrder() {
    setIsCancelOrderConfirmModalOpen(true);
  }

  async function cancelOrder() {
    if (selectedOrder && selectedOrder?.statusNameEn != "Pending") {
      toast.show(cant_cancel_not_pending, "error");
      setIsCancelOrderConfirmModalOpen(false);
      return;
    }

    try {
      // setLoading(true);
      await patch(`orders/cancel`, {
        publicId: selectedOrder.publicOrderId,
        id: null,
      });
      toast.show(order_cancelled_successfully, "success");
      setIsCancelOrderConfirmModalOpen(false);
      setIsOrderInfoModalOpen(false);
      await fetchCustomerOrders();
    } catch (err) {
      toast.show(cant_cancel_not_pending, "error");
      setIsCancelOrderConfirmModalOpen(false);
      console.error(err);
    } finally {
      // setLoading(false);
    }
  }

  if (!userId) {
    return;
  }

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
  if (!orders || orders?.length <= 0) {
    return (
      <div className="not-found-container">
        <h1>{no_orders}</h1>
      </div>
    );
  }

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

  return (
    <div>
      <h1>{my_orders}</h1>
      <FilterContainer
        searchText={searchText}
        handleSearchInputChange={handleSearchInputChange}
        sortingTerm={sortingTerm}
        handleSortingTermChange={handleSortingTermChange}
      />
      {totalOrders && (
        <Pagination
          currentPage={currentPage}
          totalItems={totalOrders}
          onPageChange={handlePageChange}
          pageSize={pageSize}
        />
      )}
      <ViewSwitcher view={view} setView={setView} />
      {view == "table" && <TableView orders={orders} viewOrder={viewOrder} />}
      {view == "card" && <CardView orders={orders} viewOrder={viewOrder} />}
      <OrderInfoModal
        show={isOrderInfoModalOpen}
        onClose={() => setIsOrderInfoModalOpen(false)}
        onConfirm={() => handleCancelOrder()}
        order={selectedOrder}
      />
      <ConfirmModal
        show={isCancelOrderConfirmModalOpen}
        onClose={() => setIsCancelOrderConfirmModalOpen(false)}
        onConfirm={() => cancelOrder()}
        title={confirm_modal_title}
        cancelLabel={cancel}
        confirmLabel={confirm}
      />
      {totalOrders && (
        <Pagination
          currentPage={currentPage}
          totalItems={totalOrders}
          onPageChange={handlePageChange}
          pageSize={pageSize}
        />
      )}
    </div>
  );
};
export default CustomerOrders;
