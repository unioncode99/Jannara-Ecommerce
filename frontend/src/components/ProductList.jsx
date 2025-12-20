import { useEffect, useState } from "react";
import ProductCard from "./ProductCard";
import "./ProductList.css";
import { create, read, remove } from "../api/apiWrapper";
import SpinnerLoader from "./ui/SpinnerLoader";
import Pagination from "./ui/Pagination";

const ProductList = ({
  searchTerm,
  categoryId,
  CustomerId = -1,
  sortingTerm,
}) => {
  const [products, setProducts] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [totalProducts, setTotalProducts] = useState(0);
  const [currentPage, setCurrentPage] = useState(1);
  const pageSize = 10; // Items per page

  // Calculate the total number of pages

  // Handle page change
  const handlePageChange = (page) => {
    setCurrentPage(page);
  };

  useEffect(() => {
    const getProducts = async () => {
      try {
        setIsLoading(true);
        const queryParams = new URLSearchParams();
        // Pagination
        queryParams.append("pageNumber", currentPage);
        queryParams.append("pageSize", pageSize);
        // Optional search term
        if (searchTerm && searchTerm.trim() !== "") {
          queryParams.append("SearchTerm", searchTerm.trim());
        }
        // Optional categoryId (assume -1 or null = no filter)
        if (categoryId && categoryId !== -1) {
          queryParams.append("categoryId", categoryId);
        }
        // Optional sort
        if (sortingTerm && sortingTerm.trim() !== "") {
          queryParams.append("SortBy", sortingTerm.trim());
        }
        // Optional customerId (if needed)
        if (CustomerId) {
          queryParams.append("CustomerId", CustomerId);
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
  }, [searchTerm, categoryId, CustomerId, sortingTerm, currentPage, pageSize]);

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
      <div>
        <Pagination
          currentPage={currentPage}
          totalItems={totalProducts}
          onPageChange={handlePageChange}
          pageSize={pageSize}
        />
      </div>
      {isLoading ? (
        <div className="loader-container">
          <SpinnerLoader />
        </div>
      ) : (
        <div className="product-grid">
          {products?.map((product) => (
            <ProductCard
              key={product.id}
              product={product}
              onToggleFavorite={handleToggleFavorite}
            />
          ))}
        </div>
      )}
      <div>
        <Pagination
          currentPage={currentPage}
          totalItems={totalProducts}
          onPageChange={handlePageChange}
          pageSize={pageSize}
        />
      </div>
    </>
  );
};
export default ProductList;
