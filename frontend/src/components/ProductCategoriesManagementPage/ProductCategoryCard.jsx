import { Layers, Pencil, Trash2 } from "lucide-react";
import { useLanguage } from "../../hooks/useLanguage";
import "./ProductCategoryCard.css";

const ProductCategoryCard = ({
  productCategory,
  handleDeleteProductCategory,
  handleEditProductCategory,
}) => {
  const { language } = useLanguage();
  return (
    <div className="product-category-card">
      <h3>
        <span>
          <Layers />
        </span>
        {language == "en" ? productCategory.nameEn : productCategory.nameAr}
      </h3>
      <p>
        {language == "en"
          ? productCategory.descriptionEn
          : productCategory.descriptionAr}
      </p>
      <div className="product-category-actions-btn">
        <button
          onClick={() => handleEditProductCategory(productCategory)}
          className="edit-product-category-btn"
        >
          <Pencil />
        </button>
        <button
          onClick={() => handleDeleteProductCategory(productCategory)}
          className="delete-product-category-btn"
        >
          <Trash2 />
        </button>
      </div>
    </div>
  );
};
export default ProductCategoryCard;
