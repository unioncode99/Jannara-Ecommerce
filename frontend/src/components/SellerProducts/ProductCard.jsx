import { Pencil, Trash2 } from "lucide-react";
import { useLanguage } from "../../hooks/useLanguage";
import { formatMoney } from "../../utils/utils";

const ProductCard = ({ product, handleEditProduct, handleDeleteProduct }) => {
  const { language, translations } = useLanguage();

  const {
    name_label,
    category_label,
    brand_label,
    sku_label,
    price_label,
    stock_label,
  } = translations.general.pages.seller_products;

  return (
    <div className="product-card">
      <div className="image-container">
        <img src={product.productImage} alt={product.productImage} />
      </div>
      <div className="product-info">
        <h3 className="product-name">
          {name_label}:{" "}
          {language == "en" ? product.productNameEn : product.productNameAr}
        </h3>
        <p className="product-category">
          {category_label}:{" "}
          {language == "en" ? product.categoryNameEn : product.categoryNameAr}
        </p>
        <p className="product-brand">
          {brand_label}:{" "}
          {language == "en" ? product.brandNameEn : product.brandNameAr}
        </p>
        <p className="product-sku">
          {sku_label}: {product.sku}
        </p>
        <p className="seller-product-price">
          {price_label}: {formatMoney(product.price)}
        </p>
        <p className="product-stock">
          {stock_label}: {product.stockQuantity}
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
