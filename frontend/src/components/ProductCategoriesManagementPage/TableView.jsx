import { Eye, Pencil, Trash2 } from "lucide-react";
import Table from "../../components/ui/Table";
import { useLanguage } from "../../hooks/useLanguage";

const TableView = ({
  productCategories,
  handleDeleteProductCategory,
  handleEditProductCategory,
}) => {
  const { translations } = useLanguage();
  const { name_en, name_ar, description_en, description_ar, actions } =
    translations.general.pages.product_categories_management;

  return (
    <div className="table-view">
      <Table
        headers={[name_en, name_ar, description_en, description_ar, actions]}
        data={productCategories.map((cat) => ({
          name_en: cat.nameEn,
          name_ar: cat.nameAr,
          description_en: cat.descriptionEn,
          description_ar: cat.descriptionAr,
          Actions: (
            <div className="product-category-actions-btn">
              <button
                onClick={() => handleEditProductCategory(cat)}
                className="edit-product-category-btn"
              >
                <Pencil />
              </button>
              <button
                onClick={() => handleDeleteProductCategory(cat)}
                className="delete-product-category-btn"
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
