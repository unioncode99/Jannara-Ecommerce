import ProductCard from "./ProductCard";
import "./ProductList.css";
import SpinnerLoader from "./ui/SpinnerLoader";
import Pagination from "./ui/Pagination";

// const ProductList = ({
//   searchTerm,
//   categoryId,
//   CustomerId = -1,
//   sortingTerm,
// }) => {
const ProductList = ({
  products,
  isLoading,
  onToggleFavorite,
  favoriteActionType,
}) => {
  // Calculate the total number of pages

  return (
    <>
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
              onToggleFavorite={onToggleFavorite}
              actionType={favoriteActionType}
            />
          ))}
        </div>
      )}
      <div></div>
    </>
  );
};
export default ProductList;
