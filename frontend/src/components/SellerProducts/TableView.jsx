import { Pencil, Trash2 } from "lucide-react";
import { useLanguage } from "../../hooks/useLanguage";
import Table from "../ui/Table";
import "./TableView.css";
import { formatMoney } from "../../utils/utils";

const TableView = ({ products, handleEditProduct, handleDeleteProduct }) => {
  const { translations, language } = useLanguage();
  const { product_name, category, brand, actions } =
    translations.general.pages.products;

  const {
    name_label,
    category_label,
    brand_label,
    sku_label,
    price_label,
    stock_label,
  } = translations.general.pages.seller_products;

  return (
    <div className="table-view">
      <Table
        headers={[
          product_name,
          category,
          brand,
          "sku",
          price_label,
          stock_label,
          actions,
        ]}
        data={products.map((product) => ({
          product_name: (
            <div className="product-name-container">
              <span className="product-image-container">
                <img src={product.productImage} alt="Profile Image" />
              </span>
              <div>
                {language == "en"
                  ? product.productNameEn
                  : product.productNameAr}
              </div>
            </div>
          ),
          category: (
            <small>
              {language == "en"
                ? product.categoryNameEn
                : product.categoryNameAr}
            </small>
          ),
          brand: (
            <small>
              {language == "en" ? product.brandNameEn : product.brandNameAr}
            </small>
          ),
          sku: <small>{product.sku}</small>,
          price_label: <small>{formatMoney(product.price)}</small>,
          stock_label: <small>{product.stockQuantity}</small>,
          actions: (
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
          ),
        }))}
      />
    </div>
  );
};
export default TableView;
