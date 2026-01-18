import { useEffect, useState } from "react";
import { read, remove } from "../../api/apiWrapper";
import { useNavigate } from "react-router-dom";
import { Package, Plus } from "lucide-react";
import ViewSwitcher from "../../components/CustomerOrders/ViewSwitcher";
import Button from "../../components/ui/Button";
import ProductsFilterContainer from "../../components/SellerProducts/ProductsFilterContainer";
import CardView from "../../components/SellerProducts/CardView";
import TableView from "../../components/SellerProducts/TableView";
import Pagination from "../../components/ui/Pagination";
import SpinnerLoader from "../../components/ui/SpinnerLoader";
import "./SellerProducts.css";
import { useLanguage } from "../../hooks/useLanguage";
import ConfirmModal from "../../components/ui/ConfirmModal";
import { toast } from "../../components/ui/Toast";

const SellerProducts = () => {
  const [view, setView] = useState("card"); // 'table' or 'card'
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(false);
  const [selectedProduct, setSelectedProduct] = useState(null);
  const [isDeleteProductModalOpen, setIsDeleteProductModalOpen] =
    useState(false);

  // Filters
  const [searchText, setSearchText] = useState("");
  const [debouncedSearch, setDebouncedSearch] = useState("");
  const [sortingTerm, setSortingTerm] = useState("");
  const [productCategoryId, setProductCategoryId] = useState("");
  const [brandId, setBrandId] = useState("");
  const [totalProducts, setTotaProducts] = useState(0);
  const [currentPage, setCurrentPage] = useState(1);
  const pageSize = 10; // Items per page

  const { translations } = useLanguage();

  const {
    title,
    add_product,
    delete_confirm_title,
    delete_confirm_message,
    delete_confirm_button,
    delete_cancel_button,
    seller_product_delete_success,
    seller_product_delete_failed,
  } = translations.general.pages.seller_products;

  const navigate = useNavigate();

  const fetchProducts = async () => {
    try {
      setLoading(true);
      const queryParams = new URLSearchParams();
      // Pagination
      queryParams.append("pageNumber", currentPage);
      queryParams.append("pageSize", pageSize);
      // Optional search term
      if (debouncedSearch && debouncedSearch.trim() !== "") {
        queryParams.append("searchTerm", debouncedSearch.trim());
      }
      // Optional sort
      if (sortingTerm && sortingTerm.trim() !== "") {
        queryParams.append("SortBy", sortingTerm.trim());
      }
      // CategoryId
      if (productCategoryId > 0) {
        queryParams.append("CategoryId", parseInt(productCategoryId));
      }
      // brandId
      if (brandId > 0) {
        queryParams.append("brandId", parseInt(brandId));
      }

      // queryParams.append("isFavoritesOnly", false);
      // Final URL
      const url = `seller-products?${queryParams.toString()}`;
      const data = await read(url); // for test
      setProducts(data?.data?.items);
      setTotaProducts(data?.data?.total);
      console.log("data", data);
      console.log("total", data?.total);
    } catch (error) {
      console.error("Failed to fetch categories:", error);
      setTotaProducts(0);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchProducts();
  }, [
    debouncedSearch,
    productCategoryId,
    brandId,
    sortingTerm,
    currentPage,
    pageSize,
  ]);

  function handleAddProduct() {
    console.log("handleAddProduct");
    navigate(`/add-seller-product`);
  }

  function handleDeleteProduct(product) {
    setSelectedProduct(product);
    setIsDeleteProductModalOpen(true);
    console.log("handleDeleteProduct -> ", product);
  }
  function handleEditProduct(product) {
    setSelectedProduct(product);
    navigate(`/edit-seller-product/${product?.sellerProductId}`);
    console.log("handleDeleteProduct -> ", product);
  }

  const handleSearchInputChange = (e) => {
    console.log("search -> ", e.target.value);
    setSearchText(e.target.value);
  };

  const handleSortingTermChange = (e) => {
    console.log("Sorting Term -> ", e.target.value);
    setSortingTerm(e.target.value);
  };

  const handleProductCategoryIdChange = (e) => {
    console.log("productCategoryId -> ", e.target.value);
    setProductCategoryId(e.target.value);
  };

  const handleBrandIdChange = (e) => {
    console.log("handleBrandIdChange -> ", e.target.value);
    setBrandId(e.target.value);
  };

  const handleClearFilters = () => {
    setSearchText("");
    setSortingTerm("");
    setBrandId("");
    setProductCategoryId("");
  };

  const handlePageChange = (page) => {
    setCurrentPage(page);
  };

  function closeModal() {
    setIsDeleteProductModalOpen(false);
    setSelectedProduct(null);
  }

  async function DeleteSellerProduct() {
    try {
      const result = await remove(
        `seller-products/${selectedProduct?.sellerProductId}`,
      );

      console.log("result -> ", result);

      if (translations.general.server_messages[result?.message?.message]) {
        toast.show(
          translations.general.server_messages[result?.message?.message],
          "success",
        );
      } else {
        toast.show(seller_product_delete_success, "success");
      }

      closeModal();
      await fetchProducts();
    } catch (error) {
      console.error(error);
      if (translations.general.server_messages[error.message]) {
        toast.show(
          translations.general.server_messages[error.message],
          "error",
        );
      } else {
        toast.show(seller_product_delete_failed, "error");
      }
    }
  }

  return (
    <div className="seller-products-container">
      <header>
        <h1>
          <Package /> {title}
        </h1>
        <Button onClick={handleAddProduct} className="btn btn-primary">
          <Plus /> {add_product}
        </Button>
      </header>
      <ViewSwitcher view={view} setView={setView} />
      <ProductsFilterContainer
        searchText={searchText}
        handleSearchInputChange={handleSearchInputChange}
        sortingTerm={sortingTerm}
        handleSortingTermChange={handleSortingTermChange}
        productCategoryId={productCategoryId}
        handleProductCategoryIdChange={handleProductCategoryIdChange}
        brandId={brandId}
        handleBrandIdChange={handleBrandIdChange}
        handleClearFilters={handleClearFilters}
      />

      {loading ? (
        <div className="loader-container">
          <SpinnerLoader />
        </div>
      ) : (
        <>
          {totalProducts > 0 && (
            <>
              <Pagination
                currentPage={currentPage}
                totalItems={totalProducts}
                onPageChange={handlePageChange}
                pageSize={pageSize}
              />
              {view == "card" && (
                <CardView
                  handleEditProduct={handleEditProduct}
                  handleDeleteProduct={handleDeleteProduct}
                  products={products}
                />
              )}
              {view == "table" && (
                <TableView
                  products={products}
                  handleEditProduct={handleEditProduct}
                  handleDeleteProduct={handleDeleteProduct}
                />
              )}
              <Pagination
                currentPage={currentPage}
                totalItems={totalProducts}
                onPageChange={handlePageChange}
                pageSize={pageSize}
              />
            </>
          )}
        </>
      )}

      <ConfirmModal
        show={isDeleteProductModalOpen}
        onClose={() => closeModal()}
        onConfirm={() => DeleteSellerProduct()}
        title={delete_confirm_message}
        cancelLabel={delete_cancel_button}
        confirmLabel={delete_confirm_button}
      />
    </div>
  );
};
export default SellerProducts;
