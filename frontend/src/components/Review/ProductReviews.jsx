import { useEffect, useState } from "react";
import SpinnerLoader from "../ui/SpinnerLoader";
import ReviewList from "./ReviewList";
import "./ProductReviews.css";
import { read } from "../../api/apiWrapper";
import { useLanguage } from "../../hooks/useLanguage";
import Pagination from "../ui/Pagination";

const ProductReviews = ({ productId }) => {
  const [reviews, setReviews] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  // Filters
  const [totalReviews, setTotalReviews] = useState(0);
  const [currentPage, setCurrentPage] = useState(1);
  const pageSize = 10; // Items per page

  const { translations } = useLanguage();
  const { customer_reviews } = translations.general.reviews;

  const fetchReviews = async () => {
    try {
      setIsLoading(true);
      const queryParams = new URLSearchParams();
      // Pagination
      queryParams.append("pageNumber", currentPage);
      queryParams.append("pageSize", pageSize);
      queryParams.append("productId", productId);

      // Final URL
      const url = `product-ratings?${queryParams.toString()}`;

      const result = await read(url);
      console.log("result -> ", result);

      setReviews(result?.items || []);
      setTotalReviews(result?.total);
    } catch (err) {
      console.error("Failed to fetch reviews", err);
      setTotalReviews(0);
      setReviews([]);
    } finally {
      setIsLoading(false);
    }
  };

  const handlePageChange = (page) => {
    setCurrentPage(page);
  };

  useEffect(() => {
    fetchReviews();
  }, [productId, currentPage]);

  return (
    <div className="product-reviews-container">
      <h3>
        {customer_reviews} ({totalReviews})
      </h3>
      {isLoading ? (
        <SpinnerLoader />
      ) : (
        <>
          <ReviewList reviews={reviews} />
          <Pagination
            currentPage={currentPage}
            totalItems={totalReviews}
            onPageChange={handlePageChange}
            pageSize={pageSize}
          />
        </>
      )}
    </div>
  );
};

export default ProductReviews;
