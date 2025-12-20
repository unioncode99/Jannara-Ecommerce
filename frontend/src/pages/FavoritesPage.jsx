import { Heart } from "lucide-react";
import ProductList from "../components/ProductList";
import { useEffect, useState } from "react";
import { read, remove } from "../api/apiWrapper";
import SpinnerLoader from "../components/ui/SpinnerLoader";
import Pagination from "../components/ui/Pagination";
import { useLanguage } from "../hooks/useLanguage";

const FavoritesPage = () => {
  const [products, setProducts] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [totalProducts, setTotalProducts] = useState(0);
  const [currentPage, setCurrentPage] = useState(1);
  const pageSize = 10; // Items per page
  const customerId = 5; // for test
  const { translations } = useLanguage();

  const getFavoritesProducts = async () => {
    try {
      setIsLoading(true);
      const queryParams = new URLSearchParams();
      // Pagination
      queryParams.append("pageNumber", currentPage);
      queryParams.append("pageSize", pageSize);
      queryParams.append("isFavoritesOnly", true);
      // Optional customerId (if needed)
      if (customerId) {
        queryParams.append("customerId", customerId);
      }
      // queryParams.append("isFavoritesOnly", false);
      // Final URL
      const url = `products?${queryParams.toString()}`;
      const data = await read(url);
      console.log("data ->", data);
      console.log("isSuccess ->", data.isSuccess);
      // If page is empty and not the first page, go back one page
      if ((data?.data?.items?.length || 0) === 0 && currentPage > 1) {
        setCurrentPage(currentPage - 1);
        return;
      }
      setProducts(data?.data?.items || []);
      setTotalProducts(data?.data?.total || 0);
      console.log("data?.items -> ", data?.data?.items);
    } catch (error) {
      console.error("Failed to fetch categories:", error);
      setProducts([]);
      setTotalProducts(0);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    getFavoritesProducts();
  }, [customerId, currentPage, pageSize]);

  const handlePageChange = (page) => {
    setCurrentPage(page);
  };

  const removeFromFavorites = async (productId) => {
    console.log("removeFromFavorites:", productId);
    setProducts((prev) => prev.filter((p) => p.id !== productId));
    setTotalProducts((prev) => prev - 1);
    if (products.length === 1 && currentPage > 1) {
      setCurrentPage((prev) => prev - 1);
    }
    try {
      await remove("customer-wish-list", {
        customerId: 5, // for test
        productId: productId,
      });
    } catch (error) {
      console.error("removeFromFavorites error:", error);
      getFavoritesProducts();
    }
  };

  return (
    <div>
      <h1>
        <Heart fill="red" color="red" />{" "}
        {translations.general.pages.favorites.favorites_title}{" "}
      </h1>
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
            onToggleFavorite={removeFromFavorites}
            favoriteActionType="delete"
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
    </div>
  );
};
export default FavoritesPage;
