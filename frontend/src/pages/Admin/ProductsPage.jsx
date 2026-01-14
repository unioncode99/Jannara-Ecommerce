import { Package, UserPlus } from "lucide-react";
import Button from "../../components/ui/Button";
import { useLanguage } from "../../hooks/useLanguage";
import "./ProductsPage.css";
import { useEffect, useState } from "react";
import { read } from "../../api/apiWrapper";
import ViewSwitcher from "../../components/CustomerOrders/ViewSwitcher";
import ProductsFilterContainer from "../../components/ProductsPage/ProductsFilterContainer";
import TableView from "../../components/ProductsPage/TableView";
import CardView from "../../components/ProductsPage/CardView";
import Pagination from "../../components/ui/Pagination";
import SpinnerLoader from "../../components/ui/SpinnerLoader";
import ConfirmModal from "../../components/ui/ConfirmModal";

const ProductsPage = () => {
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
  const { title, add_product, edit_product } =
    translations.general.pages.products;

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
      const url = `products/general?${queryParams.toString()}`;
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
  }

  function handleDeleteProduct(product) {
    setSelectedProduct(product);
    // setIsDeleteProductCategoryConfirmModalOpen(true);
    console.log("handleDeleteProduct -> ", product);
  }
  function handleEditProduct(product) {
    setSelectedProduct(product);
    // setIsAddEditProductCategoryModalOpen(true);
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

  async function deleteProduct() {}

  return (
    <div className="products-management-container">
      <header>
        <h1>
          <Package /> {title}
        </h1>
        <Button onClick={handleAddProduct} className="btn btn-primary">
          <UserPlus /> {add_product}
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
        onConfirm={() => deleteProduct()}
        title={"confirm_activate"}
        cancelLabel={"cancel"}
        confirmLabel={"activate_role"}
      />
    </div>
  );
};
export default ProductsPage;
