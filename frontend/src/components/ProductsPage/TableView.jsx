import { Pencil, Trash2 } from "lucide-react";
import { useLanguage } from "../../hooks/useLanguage";
import Table from "../ui/Table";
import "./TableView.css";

const TableView = ({ products, handleEditProduct, handleDeleteProduct }) => {
  const { translations, language } = useLanguage();
  const { product_name, category, brand, description, actions } =
    translations.general.pages.products;

  return (
    <div className="table-view">
      <Table
        headers={[product_name, category, brand, description, actions]}
        data={products.map((product) => ({
          product_name: (
            <div className="product-name-container">
              <span className="product-image-container">
                <img src={product.defaultImageUrl} alt="Profile Image" />
              </span>
              <div>{language == "en" ? product.nameEn : product.nameAr}</div>
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
          description: (
            <small>
              {language == "en" ? product.descriptionEn : product.descriptionAr}
            </small>
          ),
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
