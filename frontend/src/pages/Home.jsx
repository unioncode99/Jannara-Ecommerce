import { Search } from "lucide-react";
import Input from "../components/ui/Input";
import "./Home.css";
import ProductCategoriesDropdown from "../components/ProductCategoriesDropdown";
import { useEffect, useState } from "react";
import { useLanguage } from "../hooks/useLanguage";
import ProductList from "../components/ProductList";
import ProductSortingFiltersDropdown from "../components/ProductSortingFiltersDropdown";
import Button from "../components/ui/Button";
import Pagination from "../components/ui/Pagination";
import { create, read, remove } from "../api/apiWrapper";
import SpinnerLoader from "../components/ui/SpinnerLoader";

const Home = () => {
  const [searchText, setSearchText] = useState("");
  const [debouncedSearch, setDebouncedSearch] = useState("");
  const [selectProductCategoryId, setSelectProductCategoryId] = useState(-1);
  const [sortingTerm, setSortingTerm] = useState("");
  const { translations } = useLanguage();
  const [products, setProducts] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [totalProducts, setTotalProducts] = useState(0);
  const [currentPage, setCurrentPage] = useState(1);
  const pageSize = 10; // Items per page
  const customerId = -1;

  const handleSearchInputChange = (e) => {
    console.log("search -> ", e.target.value);
    setSearchText(e.target.value);
  };

  useEffect(() => {
    const timer = setTimeout(() => {
      setDebouncedSearch(searchText);
    }, 500);

    return () => clearTimeout(timer);
  }, [searchText]);

  useEffect(() => {
    const getProducts = async () => {
      try {
        setIsLoading(true);
        const queryParams = new URLSearchParams();
        // Pagination
        queryParams.append("pageNumber", currentPage);
        queryParams.append("pageSize", pageSize);
        // Optional search term
        if (debouncedSearch && debouncedSearch.trim() !== "") {
          queryParams.append("searchTerm", debouncedSearch.trim());
        }
        // Optional categoryId (assume -1 or null = no filter)
        if (selectProductCategoryId && selectProductCategoryId !== -1) {
          queryParams.append("categoryId", selectProductCategoryId);
        }
        // Optional sort
        if (sortingTerm && sortingTerm.trim() !== "") {
          queryParams.append("SortBy", sortingTerm.trim());
        }
        // Optional customerId (if needed)
        if (customerId) {
          queryParams.append("customerId", customerId);
        }
        // Final URL
        const url = `products?${queryParams.toString()}`;
        const data = await read(url);
        console.log("data ->", data);
        console.log("isSuccess ->", data.isSuccess);
        setProducts(data?.data?.items);
        setTotalProducts(data?.data?.total);
        console.log("data?.items -> ", data?.data?.items);
      } catch (error) {
        console.error("Failed to fetch categories:", error);
        setProducts([]);
        setTotalProducts(0);
      } finally {
        setIsLoading(false);
      }
    };
    getProducts();
  }, [
    debouncedSearch,
    selectProductCategoryId,
    customerId,
    sortingTerm,
    currentPage,
    pageSize,
  ]);

  const handleProductCategoryChange = (e) => {
    console.log("category -> ", e.target.value);
    setSelectProductCategoryId(e.target.value);
  };

  const handleSortingTermChange = (e) => {
    console.log("Sorting Term -> ", e.target.value);
    setSortingTerm(e.target.value);
  };

  // Handle page change
  const handlePageChange = (page) => {
    setCurrentPage(page);
  };

  const handleToggleFavorite = async (productId, isFavorite) => {
    console.log("Favorite changed:", productId, isFavorite);
    setProducts((prev) =>
      prev.map((p) => (p.id === productId ? { ...p, isFavorite } : p))
    );
    try {
      if (isFavorite) {
        await create("customer-wish-list", {
          customerId: 5,
          productId: productId,
        });
      } else {
        await remove("customer-wish-list", {
          customerId: 5,
          productId: productId,
        });
      }
    } catch (error) {
      console.error("Favorite error:", error);
      setProducts((prev) =>
        prev.map((p) =>
          p.id === productId ? { ...p, isFavorite: !isFavorite } : p
        )
      );
    }
  };

  return (
    <>
      <div className="product-filter-container">
        <Input
          label="Product Search"
          name="productSearch"
          placeholder={
            translations.general.pages.home.search_product_placeholder
          }
          icon={<Search />}
          type="search"
          value={searchText}
          onChange={handleSearchInputChange}
        />
        <ProductCategoriesDropdown
          value={selectProductCategoryId}
          onChange={handleProductCategoryChange}
        />
        <ProductSortingFiltersDropdown
          value={sortingTerm}
          onChange={handleSortingTermChange}
        />
      </div>
      {isLoading ? (
        <div className="loader-container">
          <SpinnerLoader />
        </div>
      ) : (
        <div>
          <div>
            <Pagination
              currentPage={currentPage}
              totalItems={totalProducts}
              onPageChange={handlePageChange}
              pageSize={pageSize}
            />
          </div>
          <ProductList
            products={products}
            isLoading={isLoading}
            totalProducts={totalProducts}
            currentPage={currentPage}
            pageSize={pageSize}
            onPageChange={handlePageChange}
            onToggleFavorite={handleToggleFavorite}
          />
          <div>
            <Pagination
              currentPage={currentPage}
              totalItems={totalProducts}
              onPageChange={handlePageChange}
              pageSize={pageSize}
            />
          </div>
        </div>
      )}
    </>
  );
};
export default Home;
