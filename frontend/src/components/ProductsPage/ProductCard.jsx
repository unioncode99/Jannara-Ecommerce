import { Pencil, Trash2 } from "lucide-react";
import { useLanguage } from "../../hooks/useLanguage";
import "./ProductCard.css";

const ProductCard = ({ product, handleEditProduct, handleDeleteProduct }) => {
  const { language } = useLanguage();
  return (
    <div className="product-card">
      <div className="image-container">
        <img src={product.defaultImageUrl} alt={product.nameEn} />
      </div>
      <div className="product-info">
        <h3 className="product-name">
          {language == "en" ? product.nameEn : product.nameAr}
        </h3>
        <p className="product-category">
          {language == "en" ? product.categoryNameEn : product.categoryNameAr}
        </p>
        <p className="product-brand">
          {language == "en" ? product.brandNameEn : product.brandNameAr}
        </p>
        <p className="product-description">
          {language == "en" ? product.descriptionEn : product.descriptionAr}
        </p>
      </div>
      <div className="product-actions">
        <button
          onClick={() => handleEditProduct(product)}
          className="edit-product-btn"
        >
          <Pencil />
        </button>
        <button
          onClick={() => handleDeleteProduct(product)}
          className="delete-product-btn"
        >
          <Trash2 />
        </button>
      </div>
    </div>
  );
};
export default ProductCard;
