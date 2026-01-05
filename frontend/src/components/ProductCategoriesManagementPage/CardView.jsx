import ProductCategoryCard from "./ProductCategoryCard";
import "./CardView.css";

const CardView = ({
  productCategories,
  handleDeleteProductCategory,
  handleEditProductCategory,
}) => {
  return (
    <div className="card-view">
      {productCategories.map((productCategory) => (
        <ProductCategoryCard
          handleDeleteProductCategory={handleDeleteProductCategory}
          handleEditProductCategory={handleEditProductCategory}
          key={productCategory.id}
          productCategory={productCategory}
        />
      ))}
    </div>
  );
};
export default CardView;
